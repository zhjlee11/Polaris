using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace NRP_Server
{
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
        }
    }
}
