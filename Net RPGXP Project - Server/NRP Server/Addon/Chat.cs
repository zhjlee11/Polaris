﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NRP_Server.Group;

namespace NRP_Server
{
    class Chat
    {
        public static string Filter(string msg)
        {
            return msg;
        }

        public static bool Command(ClientInfo clientData, string msg)
        {
            Command admin = new Command("/admin");
            Command test = new Command("/test");
            //이너월드 명령어
            Command killall = new Command("/ka");
            Command nodeath = new Command("/nodeath");
            Command setnextroom = new Command("/snr ([0-9]+)");
            Command InnerReload = new Command("/iwr");
            // /give user_name item_no item_num
            Command give = new Command("/give (.*) ([0-9]+) ([0-9]+)");
            Command tppos = new Command("/tppos ([0-9]+) ([0-9]+) ([0-9]+) ([0-9]+)");
            Command notice = new Command("/notice (.*)");
            

            Command hpadd = new Command("/hpset ([0-9]+)");
            Command mpadd = new Command("/mpset ([0-9]+)");



            /* Command seed = new Command("/seed");
             Command water = new Command("/water");
             Command get = new Command("/get");

             if (seed.isMatch(msg)) {
                 UserData.Users[clientData].character.seedfarm(UserData.Users[clientData].character, "벼", 75, 5);
             }
             if (water.isMatch(msg))
             {
                 UserData.Users[clientData].character.water(UserData.Users[clientData].character);
             }
             if (get.isMatch(msg))
             {
                 UserData.Users[clientData].character.getfarm(UserData.Users[clientData].character);
             }*/

            UserCharacter uc = UserData.Users[clientData].character;

            if (admin.isMatch(msg))
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name))
                {
                    UserData.Users[clientData].admin = true;
                    clientData.SendPacket(Packet.UserChat("\\C[50,250,50][관리] 관리자 모드로 변경되었습니다."));
                    return true;
                }
            if (test.isMatch(msg))
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name))
                {
                    if (UserData.Users[clientData].character.rogueno != -1)
                    {
                        UserData.Users[clientData].character.userData.clientData.SendPacket(Packet.Notice(255, 0, 0, "이미 이너 월드에 들어와 있습니다."));
                        return true;
                    }
                    Rogue rogue = new Rogue(UserData.Users[clientData].character);
                    rogue.EnterNew();
                    return true;
                }

            if (nodeath.isMatch(msg))
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name))
                {
                    if (UserData.Users[clientData].character.NoDamage == 0) { UserData.Users[clientData].character.NoDamage = 1; clientData.SendPacket(Packet.Notice(0, 255, 0, "무적 모드가 활성화 되었습니다.")); }
                    else if (UserData.Users[clientData].character.NoDamage == 1) { UserData.Users[clientData].character.NoDamage = 0; clientData.SendPacket(Packet.Notice(255, 0, 0, "무적 모드가 비활성화 되었습니다.")); }
                    return true;
                }

            if (killall.isMatch(msg)) {
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name)) {
                    foreach (Enemy e in UserData.Users[clientData].character.fieldData.Enemies) {
                        if (UserData.Users[clientData].character == null || e.IsDead) { continue; }
                        e.dead(UserData.Users[clientData].character);
                    }
                    return true;
                }
            }

            if (setnextroom.isMatch(msg))
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name))
                {
                    string[] data = setnextroom.MatchData(msg);

                    if (UserData.Users[clientData].character.rogueno == -1)
                    {
                        clientData.SendPacket(Packet.Notice(255, 0, 0, "이너 월드에 들어가 있지 않습니다."));
                        return true;
                    }
                    else {
                        Rogue.rogues[UserData.Users[clientData].character.rogueno].setNext(Convert.ToInt32(data[1]));
                        return true;
                    }
                }

            if (InnerReload.isMatch(msg))
                if (Packet.ADMIN.Contains(UserData.Users[clientData].character.name))
                {

                    if (UserData.Users[clientData].character.rogueno == -1)
                    {
                        clientData.SendPacket(Packet.Notice(255, 0, 0, "이너 월드에 들어가 있지 않습니다."));
                        return true;
                    }
                    else
                    {
                        Rogue.rogues[UserData.Users[clientData].character.rogueno].SetupStage();
                        UserData.Users[clientData].character.ReloadField();
                        return true;
                    }
                }


            if (notice.isMatch(msg))
                if (UserData.Users[clientData].admin)
                {
                    string[] data = notice.MatchData(msg);

                    foreach (UserData u in UserData.Users.Values)
                    {
                        if (u.character == null) { continue; }
                        u.clientData.SendPacket(Packet.Notice(51, 0, 204, data[1], 30));
                    }

                    return true;
                }

            if (hpadd.isMatch(msg))
                if (UserData.Users[clientData].admin)
                {
                    string[] data = hpadd.MatchData(msg);

                    if (UserData.Users[clientData].character.maxhp <= Convert.ToInt32(data[1])) { UserData.Users[clientData].character.hp = UserData.Users[clientData].character.maxhp; }
                    else if (0 >= Convert.ToInt32(data[1])) { UserData.Users[clientData].character.damage(UserData.Users[clientData].character.hp.ToString(), false); }
                    UserData.Users[clientData].character.hp = Convert.ToInt32(data[1]);
                    clientData.SendPacket(Packet.CharacterStatusUpdate(UserData.Users[clientData].character));
                    return true;
                }
            if (mpadd.isMatch(msg))
                if (UserData.Users[clientData].admin)
                {
                    string[] data = mpadd.MatchData(msg);

                    if (UserData.Users[clientData].character.maxmp <= Convert.ToInt32(data[1])) { UserData.Users[clientData].character.mp = UserData.Users[clientData].character.maxmp; }
                    else if (0 >= Convert.ToInt32(data[1])) { UserData.Users[clientData].character.mp = 0; }
                    UserData.Users[clientData].character.mp = Convert.ToInt32(data[1]);
                    clientData.SendPacket(Packet.CharacterStatusUpdate(UserData.Users[clientData].character));
                    return true;
                }

            if (give.isMatch(msg))
                if (UserData.Users[clientData].admin)
                {
                    string[] data = give.MatchData(msg);

                    // 데이터 검사
                    DataTable rs = Mysql.Query($"SELECT * FROM storage_item WHERE no = '{data[2]}'");
                    if (rs.Rows.Count == 0)
                    {
                        clientData.SendPacket(Packet.UserChat("\\C[50,250,50][관리] 존재하지 않는 아이템 식별 번호입니다."));
                        return true;
                    }

                    foreach (UserData u in UserData.Users.Values)
                    {
                        if (u.character == null) { continue; }
                        if (u.character.name == data[1])
                        {
                            if (Item.Items[Convert.ToInt32(data[2])] != null)
                            {
                                NRP_Server.Command.gainItem(u.character, Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
                                clientData.SendPacket(Packet.UserChat("\\C[50,250,50][관리] 성공적으로 아이템을 보냈습니다."));
                                u.clientData.SendPacket(Packet.UserChat("\\C[50,250,50][관리] 관리자에게서 아이템을 받았습니다."));
                                return true;
                            }
                        }
                    }
                    clientData.SendPacket(Packet.UserChat("\\C[50,250,50][관리] 유저가 접속중이 아닙니다."));
                    return true;
                }

            if (tppos.isMatch(msg))
                if (UserData.Users[clientData].admin)
                {
                    string[] data = tppos.MatchData(msg);

                    UserCharacter c = UserData.Users[clientData].character;

                    UserData.Users[clientData].character.fieldData.leave(UserData.Users[clientData].character.no);
                    if (!Map.Maps[Convert.ToInt32(data[1])].Fields.ContainsKey(Convert.ToInt32(data[2]))) { Map.Maps[Convert.ToInt32(data[1])].newField(Convert.ToInt32(data[2])); }
                    Map.Maps[Convert.ToInt32(data[1])].Fields[Convert.ToInt32(data[2])].join(c, Convert.ToInt32(data[3]), Convert.ToInt32(data[4]));

                    return true;
                }



            return false;
        }
    }
}
