using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRP_Server
{
    class NPCFunction
    {
        public static void testFunction(UserCharacter u, NPC npcData)
        {
            Msg.Info(u.name);
            Msg.Info(npcData.name);
        }

        public static void InsteadHpToStatus(UserCharacter u, NPC npcData)
        {
            if (u.hp >= u.maxhp / 10 * 9)
            {
                u.UpStr(3);
                u.UpDex(3);
                u.UpInt(3);
                u.UpLuk(3);
                u.damage((u.maxhp / 10 * 9).ToString(), false);
                u.ReloadStatus();
                u.userData.clientData.SendPacket(Packet.Notice(0, 255, 0, "제단을 성공적으로 가동하였습니다."));
            }
            else {
                u.userData.clientData.SendPacket(Packet.Notice(255, 0, 0, "충분한 HP가 없습니다."));
            }
            
        }
    }
}
