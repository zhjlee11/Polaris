using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace NRP_Server
{   
    //로그라이크 컨텐츠의 클래스를 정의하는 곳입니다.
    class Rogue
    {
        public static Dictionary<int, Rogue> rogues = new Dictionary<int, Rogue>();
        public static Dictionary<int, int> stagetype = new Dictionary<int, int>();
        public static int souldid = -1; //영혼 아이템 코드

        public int no { get; private set; }
        public UserCharacter player { get; set; }

        public int stagenum { get; set; } = 1;

        public Field NextStage { get; set; } = null;
        public Field NowStage { get; set; } = null;
        public Field PreviousStage { get; set; } = null;

        public Rogue(UserCharacter uc) {
            no = rogues.Count();
            uc.rogueno = no;
            uc.soul = 0;
            player = uc;
            rogues[no] = this;
        }

        public void EnterNew() {
            player.userData.clientData.SendPacket(Packet.Flash(0, 0, 0, 240));
            player.fieldData.leave(player.no);
            Random r = new Random();
            Random rr = new Random();
            int rnum = r.Next(0, stagetype.Count());
            int rrnum = rr.Next(0, stagetype.Count());
            this.NowStage = Map.Maps[stagetype[rnum]].newFieldRO(player.no) as Field;
            this.NextStage = Map.Maps[stagetype[rrnum]].newFieldRO(player.no) as Field;
            NowStage.join(player, (Map.Maps[NowStage.mapid].width - 1) / 2, Map.Maps[NowStage.mapid].height - 1);
            player.ReloadUser();
            player.userData.clientData.SendPacket(Packet.RogueReload(this));
            SetupStage();
            player.userData.clientData.SendPacket(Packet.Notice(0, 255, 0, "무질서의 차원, 이너 월드에 들어 왔습니다."));
        }

        public void GoNext() {
            if (PreviousStage != null) { PreviousStage.dispose(); }
            NowStage.leave(player.no);
            PreviousStage = NowStage;
            NowStage = NextStage;
            NextStage.join(player, (Map.Maps[NowStage.mapid].width-1)/2, Map.Maps[NowStage.mapid].height-1);
            player.ReloadUser();
            Random r = new Random();
            int rnum = r.Next(0, stagetype.Count());
            NextStage = Map.Maps[stagetype[rnum]].newFieldRO(player.no);
            SetupStage();
        }

        public void StageClear() {
            player.userData.clientData.SendPacket(Packet.Notice(0, 255, 0, stagenum + " 스테이지가 클리어 되었습니다."));
            stagenum += 1;
            GoNext();
            player.userData.clientData.SendPacket(Packet.RogueReload(this));
        }

        public void SetupStage() {
            switch (NowStage.mapid) { //맵 id로 구분
                case 8:
                    /* [몬스터 배치 Example]
                    int index1 = NowStage.addEnemy(37, 2, 8);
                    (player.fieldData.Enemies[index1] as Enemy).rebirth_time = -1;
                    int index2 = NowStage.addEnemy(37, 20, 5);
                    (player.fieldData.Enemies[index2] as Enemy).rebirth_time = -1;
                    int index3 = NowStage.addEnemy(37, 16, 17);
                    (player.fieldData.Enemies[index3] as Enemy).rebirth_time = -1;
                    int index4 = NowStage.addEnemy(37, 20, 11);
                    (player.fieldData.Enemies[index4] as Enemy).rebirth_time = -1;*/
                    break;
                
            }
            Random r = new Random();
            if (NowStage.Enemies.Count != 0) {
                foreach (Enemy e in NowStage.Enemies)
                {
                    /*  
                        최대 체력 = 반올림[기본 최대 체력+ 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)+0.1}]
                        최대 마나 = 반올림[기본 최대 마나 + 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)+0.1}]
                        힘 스텟    = 반올림[기본 힘 스텟 + 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)}]
                        민첩 스텟 = 반올림[기본 민첩 스텟 + 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)}]
                        운 스텟    = 반올림[기본 운 스텟 + 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)}]
                        지능 스텟 = 반올림[기본 지능 스텟 + 스테이지 ^ {1.8+(0과 1 사이의 랜덤 진분수 값)}]
                     */

                    e.maxhp += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble() + 0.1, 0, MidpointRounding.AwayFromZero));
                    e.maxmp += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble() + 0.1, 0, MidpointRounding.AwayFromZero));
                    e.str += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero));
                    e.dex += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero));
                    e.Int += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero));
                    e.luk += stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero));
                }
            }
        }
    }
}
