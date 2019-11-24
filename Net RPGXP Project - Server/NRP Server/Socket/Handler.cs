﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Threading;
using NRP_Server.Storage;
using System.Data;
using System.Globalization;

namespace NRP_Server
{
    // 강제 연결 끊기
    // (Socket).Shutdown(System.Net.Sockets.SocketShutdown.Both);

    // Thread safe
    // https://stackoverflow.com/questions/6180066/socket-locking-across-threads

    class Handler
    {
        static DateTime dateTime = DateTime.MinValue;
        static int ticks = 0;
        static int already = 0;
        private readonly object receiveSyncRoot = new object();

        public static ArrayList emotions = new ArrayList { 
            //이모티콘 애니메이션 no만 작성
            3, 6, 7
        };

        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private Thread runUpdate = new Thread(delegate() { while (true) { update(); } });
        private Thread runSkillUpdate = new Thread(delegate () { while (true) { Skill.update(); } });

        public void start()
        {
            // Cancel Key
            Console.CancelKeyPress += new ConsoleCancelEventHandler(console_close_event);

            // Socket
            Socket serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSock.Bind(new IPEndPoint(IPAddress.Any, Config.PORT));
                serverSock.Listen(Config.MAX_NUM);

                // MySQL 연결
                Mysql.connect(Config.SERVER_IP, Config.DATABASE, Config.USER_NAME, Config.PASSWORD);
                Msg.Connect("MySQL 서버에 연결되었습니다.");
                // 경험치 로드
                Exp.loadData();
                // 아이템 등등 로드
                Item.loadData();
                Store.loadData();
                // 스킬 로드
                Skill.loadData();

                // 맵 세팅
                Map.loadData();
                Job.loadData();
                Combination.loadrecipe();

                //서버의 비정상적 종료로 인해, 영구적으로 온라인 상태로 표기되어있는 유저를 오프라인 상태로 변경
                Mysql.Query("UPDATE user_information SET online=0");

                Rogue.stagetype.Add(0, 8);
                Rogue.stagetype.Add(1, 9);
                Rogue.stagetype.Add(2, 10);

                Msg.Info("서버가 정상적으로 실행되었습니다.");
                // 멀티쓰레드 시작
                runUpdate.Start();
                runSkillUpdate.Start();
                while (true)
                {
                    allDone.Reset();
                    // 승인이 끝난 경우 AcceptCallBack을 실행 - 대상 소켓은 서버 소켓
                    serverSock.BeginAccept(new AsyncCallback(AcceptCallBack), serverSock);
                    allDone.WaitOne();
                    
                }
            }
            catch (Exception e)
            {
                Msg.Error(e.ToString());
            }

        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            allDone.Set();
            try
            {
                lock (receiveSyncRoot)
                {
                    // 요청이 끝난 대상의 소켓은 서버의 소켓
                    Socket listener = (Socket)ar.AsyncState;
                    // 서버의 소켓에서 마지막 요청을 보냈던 소켓을 받음
                    Socket clientSock = listener.EndAccept(ar);

                    Msg.Info("[" + ((IPEndPoint)clientSock.RemoteEndPoint).Address.ToString() + "] " + "유저가 접속했습니다.");

                    // 유저정보를 대입. 가장 중요한 소켓은 살린다
                    ClientInfo clientData = new ClientInfo(clientSock);
                    clientData.BufferSize = 512;
                    // 모든 유저 브로드캐스트에 추가
                    Packet.BroadCast += clientData.SendPacket;
                    clientSock.BeginReceive(clientData.Buffer, 0, clientData.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallBack), clientData);
                }
            }
            catch (Exception e)
            {
                Msg.Error(e.ToString());
            }
        }
        private void ReadCallBack(IAsyncResult ar)
        {
            // 보낸 유저의 데이터를 받아옴
            // 다시 대기한다.
            ClientInfo clientData = (ClientInfo)ar.AsyncState;
            try
            {
                // 일괄 처리로 옮김
                if (Packet.RecvData(ar))
                {
                    clientData.socket.BeginReceive(clientData.Buffer, 0, clientData.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallBack), clientData);
                }
                else
                {
                    // 연결이 끊긴 경우
                    removeClient(clientData, ((IPEndPoint)clientData.socket.RemoteEndPoint).Address.ToString());
                }
            }
            catch (Exception e)
            {
                // 보통은 연결 끊김
                Msg.Error(e.ToString());
                if (!clientData.closed)
                    removeClient(clientData, ((IPEndPoint)clientData.socket.RemoteEndPoint).Address.ToString());
            }
        }
        private void removeClient(ClientInfo clientData, string ip)
        {
            // 브로드캐스트 제거
            Packet.BroadCast -= clientData.SendPacket;
            // 강제 종료
            clientData.close();
            // 변수 클리어
            if (UserData.Users.ContainsKey(clientData))
            {
                UserData.Users[clientData].saveData(true);
                UserData.Users.Remove(clientData);
            }
            Msg.Info("[" + ip + "] " + "유저가 접속 종료했습니다.");
        }

        // 종료 이벤트 (Ctrl + C)
        void console_close_event(object sender, ConsoleCancelEventArgs e)
        {
            foreach(UserData u in UserData.Users.Values)
            {
                    u.saveData(true);
            }
        }

        // 업데이트 메서드
        static void update()
        {
            foreach (Map map in Map.Maps.Values)
                map.update();

            if (ticks == 10)
            {
                try
                {
                    using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                        dateTime = DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                }
                catch (Exception e) { dateTime = DateTime.Now; }                    
                //매 시간 실행 내용
                if (dateTime.Minute == 0 && dateTime.Millisecond == 0)
                {
                    if (already == 0)
                    {
                        Mysql.Query($"UPDATE user_information SET ban = ban - 1 WHERE ban > 0");
                        Mysql.Query($"UPDATE user_information SET mute = mute - 1 WHERE mute > 0");
                        already = 1;
                    }
                }
                else { already = 0; }
                ticks = 0;
            }
            else { ticks += 1; }


            Thread.Sleep(Config.WAIT_TIME);
        }
    }
}