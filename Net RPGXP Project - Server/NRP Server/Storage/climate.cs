using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.Storage
{
    class climate
    {
        /*
            [날씨 리스트]

            0. 일반
            1. 일반 비 : [생명의 비] 10초당 에너지 +2
            2. 강한 비 : [오염된 비] 3초당 체력 -1
            3. 눈 : [폭설] 5초마다 5%의 확률로 3틱 스턴
        */

        public static Dictionary<int, climate> climates = new Dictionary<int, climate>();

        public int no { get; set; }
        public int weatherno { get; set; }
        public string name { get; set; }
        public int duration { get; set; }


        public climate(int climateno, int _d) {
            no = climates.Count();
            weatherno = climateno;
            name = getName();
            duration = _d;
            climates[weatherno] = this;
        }

        public string getName() {
            if (weatherno == 1) { return "생명의 비"; }
            else if (weatherno == 2) { return "오염된 비"; }
            else if (weatherno == 3) { return "폭설"; }
            else { return ""; }
        }

        public string getEffect() {
            if (weatherno == 1) { return "지금부터 10초마다 에너지가 +2 됩니다."; }
            else if (weatherno == 2) { return "지금부터 3초마다 체력이 -1 됩니다."; }
            else if (weatherno == 3) { return "지금부터 이동속도가 느려집니다."; }
            else { return ""; }
        }

        public void weatherNotice(Field f) {
            if (weatherno == 0)
            {
                foreach (UserCharacter u in f.Users.Values)
                {
                    u.userData.clientData.SendPacket(Packet.Notice(0, 255, 55, "하늘이 맑아집니다."));
                }
            }
            else {
                foreach (UserCharacter u in f.Users.Values)
                {
                    u.userData.clientData.SendPacket(Packet.Notice(143, 0, 255, name + "이/가 내리기 시작합니다. " + getEffect()));
                    u.userData.clientData.SendPacket(Packet.Weather(weatherno, duration*60));
                }
            }   
        }

        public void weatherNotice(UserCharacter u)
        {
            if (u == null || u.userData.clientData == null) { return; }
            if (weatherno == 0)
            {
                u.userData.clientData.SendPacket(Packet.Notice(0, 255, 55, "하늘이 맑아집니다."));
            }
            else
            {
                u.userData.clientData.SendPacket(Packet.Notice(143, 0, 255, name + "이/가 내리기 시작합니다. " + getEffect()));
                u.userData.clientData.SendPacket(Packet.Weather(weatherno, duration*60));
            }
        }

        public void weatherEffect(Field f) {
            Dictionary<int, UserCharacter> ul = new Dictionary<int, UserCharacter>(f.Users);
            if (weatherno == 0) { return; }
            else if (weatherno == 1)
            {
                foreach (UserCharacter u in ul.Values)
                {
                    if (u.maxmp <= u.mp + 3) { u.mp = u.maxmp; }
                    else { u.mp += 3; }
                    u.userData.clientData.SendPacket(Packet.CharacterStatusUpdate(u));
                }
            }
            else if (weatherno == 2)
            {
                foreach (UserCharacter u in ul.Values)
                {
                    u.damage("1", false);
                    u.userData.clientData.SendPacket(Packet.CharacterStatusUpdate(u));
                }
            }
            else if (weatherno == 3)
            {
                foreach (UserCharacter u in ul.Values)
                {
                    Random r = new Random();
                    int rnum = r.Next(0, 100);
                    if (rnum >= 70) {
                        if (u.canctrl == 5) { continue; }
                        else { u.canctrl += 5; }
                    }
                    u.userData.clientData.SendPacket(Packet.CharacterStatusUpdate(u));
                }
            }
            else { return;  }

        }
    }
}
