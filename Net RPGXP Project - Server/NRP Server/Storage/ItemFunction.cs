using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NRP_Server
{
    class ItemFunction
    {
        // Recovery Value
        public static bool RecoveryHpValue(UserCharacter u, Item item, int value)
        {
            u.animation(item.animation_id);
            u.damage((-value).ToString(), false);
            return true;
        }
        public static bool RecoveryMpValue(UserCharacter u, Item item, int value)
        {
            return true;
        }
        public static bool RecoveryAllValue(UserCharacter u, Item item, int value)
        {
            return true;
        }

        // Recovery Rate
        public static bool RecoveryHpRate(UserCharacter u, Item item, int value)
        {
            return true;
        }
        public static bool RecoveryMpRate(UserCharacter u, Item item, int value)
        {
            return true;
        }
        public static bool RecoveryAllRate(UserCharacter u, Item item, int value)
        {
            return true;
        }

        // Skill Book
        public static bool LearnCharacterSkill(UserCharacter u, Item item, int value)
        {
            if (!u.Skills.ContainsKey(Skill.Skills[value]))
            {
                u.learnSkill(Skill.Skills[value]);
            }
            else
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "아이템 사용 불가", "이미 배운 스킬입니다."));
                return false;
            }
            return true;
        }
    }
    /*public static bool spawnmonster(UserCharacter u, Item item, int value)
    {
        int count = 0;
        Enemy obj;

        //no값이 skill.~~ 인것을 가져옴
        DataTable pos = Mysql.Query($"SELECT * FROM enemy_position WHERE map_id = '{u.mapid}'");
        DataTable ds;
        //pos에 있는 것중에 no가 skill.~인 열(가로)을 가져옴
        foreach (DataRow ps in pos.Rows)
        {
            ds = Mysql.Query($"SELECT * FROM enemy WHERE no = '{value}'");
            foreach (DataRow rs in ds.Rows)
            {

                //enemy 생성자으 인자는 몬스터가 소환될 field값 | map 값은 seed 미 포함임으로 안됨
                obj = new Enemy(u.fieldData);
                obj.loadData(rs, ps);
                obj.mapid = u.mapid;
                if (u.direction == 8)
                {
                    Console.WriteLine("북");
                    obj.start_x = u.x;
                    obj.start_y = u.y + 1;
                    obj.direction = u.direction;
                }
                else if (u.direction == 4)
                {
                    Console.WriteLine("서");
                    obj.start_x = u.x - 1;
                    obj.start_y = u.y;
                    obj.direction = u.direction;
                }
                else if (u.direction == 6)
                {
                    Console.WriteLine("동");
                    obj.start_x = u.x + 1;
                    obj.start_y = u.y;
                    obj.direction = u.direction;
                }
                else if (u.direction == 2)
                {
                    Console.WriteLine("남");
                    obj.start_x = u.x;
                    obj.start_y = u.y - 1;
                    obj.direction = u.direction;
                }
                else
                {
                    return;
                }

                //$"{u.fieldData.Enemies.Count()}".Length;                   
                //u.fieldData.Enemies.Add(obj.no + $"{u.fieldData.Enemies.Count()}".Length -1, obj);
                u.fieldData.AllSendPacket(Packet.EnemyCreate(obj));
                Console.WriteLine("패킷 전송");
            }
        }
        return true;
    }*/
}
