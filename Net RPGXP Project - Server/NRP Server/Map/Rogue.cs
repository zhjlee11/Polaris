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
            int rnum, rrnum;
            while (true)
            {
                rnum = r.Next(0, stagetype.Count());
                if (rnum == 11)
                {
                    if (player.soul >= 20) { break; }
                    else { continue; }
                }
                else if (rnum == 12)
                {
                    if (player.maxhp / 10 >= player.hp) { break; }
                    else { continue; }

                }
                else if (rnum == 13)
                {
                    if (player.soul >= 100) { break; }
                    else { continue; }
                }
                break;
            }
            while (true)
            {
                rrnum = r.Next(0, stagetype.Count());
                if (rnum == rrnum) { continue; }
                if (rrnum == 11)
                {
                    if (player.soul >= 20) { break; }
                    else { continue; }
                }
                else if (rrnum == 12)
                {
                    if (player.maxhp / 10 >= player.hp) { break; }
                    else { continue; }

                }
                else if (rrnum == 13)
                {
                    if (player.soul >= 100) { break; }
                    else { continue; }
                }
                break;
            }
            this.NowStage = Map.Maps[stagetype[rnum]].newFieldRO(player.no) as Field;
            this.NextStage = Map.Maps[stagetype[rrnum]].newFieldRO(player.no) as Field;
            NowStage.join(player, (Map.Maps[NowStage.mapid].width - 1) / 2, Map.Maps[NowStage.mapid].height - 1);
            player.ReloadUser();
            player.userData.clientData.SendPacket(Packet.RogueReload(this));
            SetupStage();
            player.userData.clientData.SendPacket(Packet.Notice(0, 255, 0, "무질서의 차원, 이너 월드에 들어 왔습니다."));
            player.ReloadField();
        }

        public void GoNext() {
            if (PreviousStage != null) { 
                PreviousStage.dispose(); 
            }
            NowStage.deleteDropItems();
            NowStage.leave(player.no);
            NowStage.dispose();
            NowStage = NextStage;
            NextStage.join(player, (Map.Maps[NowStage.mapid].width-1)/2, Map.Maps[NowStage.mapid].height-1);
            player.ReloadUser();
            Random r = new Random();
            int rnum;
            while (true) {
                rnum = r.Next(0, stagetype.Count());
                if (NowStage.mapid == stagetype[rnum]) { continue; }
                if (rnum == 11)
                {
                    if (player.soul >= 20) { break; }
                    else { continue; }
                }
                else if (rnum == 12)
                {
                    if (player.maxhp / 10 >= player.hp) { break; }
                    else {continue;}
                
                }
                else if (rnum == 13) {
                    if (player.soul >= 100) { break; }
                    else { continue; }
                }
                break;
            }
            NextStage = Map.Maps[stagetype[rnum]].newFieldRO(player.no);
            SetupStage();
            player.ReloadField();
        }

        public void StageClear() {
            player.userData.clientData.SendPacket(Packet.Notice(0, 255, 0, stagenum + " 스테이지가 클리어 되었습니다."));
            stagenum += 1;
            GoNext();
            player.userData.clientData.SendPacket(Packet.RogueReload(this));
        }

        public static void LoadStage() {
            Rogue.stagetype.Add(0, 8);
            Rogue.stagetype.Add(1, 9);
            Rogue.stagetype.Add(2, 10);
            Rogue.stagetype.Add(3, 11);
            Rogue.stagetype.Add(4, 12);
            Rogue.stagetype.Add(5, 13);
            Rogue.stagetype.Add(6, 14);
            Rogue.stagetype.Add(7, 15);
            Rogue.stagetype.Add(8, 16);
            Rogue.stagetype.Add(9, 17);
            Rogue.stagetype.Add(10, 18);
            Rogue.stagetype.Add(11, 19);
            Rogue.stagetype.Add(12, 20);
            Rogue.stagetype.Add(13, 21);
        }

        public void SetupStage() {
            NowStage.addEnemy(37, 13, 20);
            switch (NowStage.mapid) { //맵 id로 구분
                case 8:
                    /* [몬스터 배치 Example]
                    
                    NowStage.addEnemy(37, 20, 5);
                    NowStage.addEnemy(37, 16, 17);
                    NowStage.addEnemy(37, 20, 11);*/
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

                    e.SetMaxHp(e.maxhp + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble() + 0.1, 0, MidpointRounding.AwayFromZero)));
                    e.SetMaxMp(e.maxmp + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble() + 0.1, 0, MidpointRounding.AwayFromZero)));
                    e.SetStr(e.str + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero)));
                    e.SetDex(e.dex + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero)));
                    e.SetInt(e.Int + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero)));
                    e.SetLuk(e.luk + stagenum ^ Convert.ToInt32(Math.Round(1.8 + r.NextDouble(), 0, MidpointRounding.AwayFromZero)));
                    e.rebirth_time = -1;
                }
            }
            player.ReloadField();
        }
    }
}
