using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server
{
    class Room
    {
        public static Dictionary<int, Room> Rooms = new Dictionary<int, Room>();

        public static void Create(UserData u, string title, string password, int maxnum)
        {
            int rid = setId();
            Room obj = new Room(u, rid, title, password, maxnum);

            Rooms.Add(rid, obj);
        }
        public static void Delete(int rid)
        {
            if (Include(rid))
            {
                // 퇴장명령

                // 제거
                Rooms.Remove(rid);
            }
        }
        public static bool Include(int rid)
        {
            return Rooms.ContainsKey(rid);
        }
        public static int setId()
        {
            for (int rid = 1; rid < 65432; rid++)
                if (!Rooms.ContainsKey(rid))
                    return rid;
            return 65432;
        }

        // 여기부터 인스턴스
        public Dictionary<ClientInfo, RoomUser> users { get; private set; }
        public int no { get; private set; }
        public string title { get; private set; }
        public string password { get; private set; }
        public int maxnum { get; private set; }
        public RoomUser master { get; private set; }
        public ArrayList ships { get; private set; }

    public RoomTimer Timer { get; private set; }


        public Room(UserData u, int rid, string _title, string _password, int _maxnum)
        {
            no = rid;
            title = _title;
            password = _password;
            maxnum = _maxnum;
            users = new Dictionary<ClientInfo, RoomUser>();
            // 타이머 관리
            Timer = new RoomTimer();
            // 방 생성자 입장처리
            Enter(u);
        }

        // Thread Update
        public void update()
        {
            //Timer.update();
        }

        public void Enter(UserData u)
        {
            // 입장자 오브젝트화
            RoomUser obj = new RoomUser(u);
            // 만약 첫번째 입장자라면 방장으로
            if (users.Count == 0)
                master = obj;
            users.Add(u.clientData, obj);
            // 입장 패킷

        }
        public void Exit(UserData u)
        {
            // 지운다
            users.Remove(u.clientData);

            // 만약 나간 사람이 방장이라면
            if (master.uData == u)
            {
                // 방장 변경
                master = users.Values.First();
                // 방장이 u.name 님에서 master.uData.name 님으로 변경되었습니다.
            }
            // 퇴장패킷 

        }


        public void RedTeamChange(UserData u)
        {
            RoomUser ru = new RoomUser(u);
            ru.team = "red";
        }

        public void BlueTeamChange(UserData u)
        {
            RoomUser ru = new RoomUser(u);
            ru.team = "blue";
        }

        public void AllUserSendPacket(Hashtable data)
        {
            foreach (RoomUser u in users.Values)
                u.uData.clientData.SendPacket(data);
        }
    }

    class RoomUser
    {
        public UserData uData { get; private set; }
        public string team { get; set; }
        public int kill { get; private set; }
        public int death { get; private set; }

        public RoomUser(UserData u)
        {
            uData = u;
            team = "none";
            kill = 0;
            death = 0;
        }
    }
    class RoomTimer
    {
        public int hour { get; private set; }
        public int min { get; private set; }
        public int sec { get; private set; }
        public int msec { get; private set; }

        public RoomTimer()
        {
            hour = 0;
            min = 0;
            sec = 0;
            msec = 0;
        }

        public void set(int h, int m, int s, int ms)
        {
            hour = h;
            min = m;
            sec = s;
            msec = ms;
        }
        public void update()
        {
            if (msec > 0) { msec -= Config.WAIT_TIME; }
            if (msec == 0)
            {
                if (sec == 0)
                {
                    if (min == 0)
                    {
                        if (hour == 0)
                        {
                            return;
                        }
                        else { min += 60; hour--; }
                    }
                    else { sec += 60; min--; }
                }
                else { msec += 1000; sec--; }
            }
            else { return; }
        }
    }
}
