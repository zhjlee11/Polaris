﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NRP_Server
{
    class Enemy : Character
    {
        #region 컨버터
        private static int ToInt(object data)
        {
            return Convert.ToInt32(data);
        }
        # endregion
        public Field fieldData;
        public Dictionary<int, EnemyDropData> dropData;
        public UserCharacter target { get; set; }
        public Enemy targetE { get; set; }

        

        public int pos_no { get; private set; }
        public int start_x { get; set; }
        public int start_y { get; set; }
        public bool IsDead { get; private set; }
        public int dead_count { get; private set; }
        public int pattern { get; set; }
        public int delay { get; private set; }
        public int delay_count { get; private set; }
        public int rebirth_time { get; set; }
        public int speed_count { get; private set; }
        public int sight { get; private set; }
        public int animation_id { get; private set; }
        public Dictionary<int, int> skillList { get; private set; } = new Dictionary<int, int>(); // 첫번째 인자는 스킬 번호 & 두번째 인자는 확률(0 < x <= 1000)

        public UserCharacter creator;

        public Enemy(Field _fieldData)
        {
            fieldData = _fieldData;
            IsDead = false;
            speed_count = 0;
            delay_count = 0;
        }

        public void loadData(DataRow rs, int _no, int _mapid, int _x, int _y)
        {
            no = ToInt(rs["no"]);
            name = rs["name"].ToString();
            image = rs["image"].ToString();
            level = ToInt(rs["level"]);
            exp = ToInt(rs["exp"]);

            maxhp = ToInt(rs["maxhp"]);
            maxmp = ToInt(rs["maxmp"]);
            hp = maxhp;
            mp = maxmp;
            str = ToInt(rs["str"]);
            dex = ToInt(rs["dex"]);
            Int = ToInt(rs["int"]);
            luk = ToInt(rs["luk"]);

            pos_no = _no;



            mapid = _mapid;
            start_x = _x;
            start_y = _y;


            x = _x; 
            y = _y; 


            direction = ToInt(rs["direction"]);
            move_speed = ToInt(rs["move_speed"]);
            delay = ToInt(rs["delay"]);
            rebirth_time = ToInt(rs["rebirth_time"]);
            pattern = ToInt(rs["pattern"]);
            sight = ToInt(rs["sight"]);
            animation_id = ToInt(rs["animation_id"]);
            loadDropData();

            if (no == 82) { skillList.Add(7, 80); }
            else if (no == 84) { skillList.Add(9, 50); }
            else if (no == 85) { skillList.Add(10, 50); }
            else if (no == 86) { skillList.Add(11, 100); }
            else if (no == 87) { skillList.Add(12, 70); }
            else if (no == 88) { skillList.Add(13, 70); }
        }

        public void setcreator(UserCharacter chara)
        {
            creator = chara;
        }

        public void loadDropData()
        {
            EnemyDropData obj;
            dropData = new Dictionary<int, EnemyDropData>();
            DataTable ds = Mysql.Query($"SELECT * FROM enemy_dropitem WHERE enemy_no = '{no}'");
            foreach (DataRow rs in ds.Rows)
            {
                obj = new EnemyDropData(rs);
                dropData.Add(obj.no, obj);
            }
        }

        public bool CheckUseSkill(int per) {
            //랜덤 값보다 클경우, 스킬 시전 허락
            return Command.rand.Next(1, 1000) < per;
        }

        public void SetMaxHp(int num) { maxhp = num; }
        public void SetMaxMp(int num) { maxmp = num; }
        public void SetStr(int num) { str = num; }
        public void SetDex(int num) { dex = num; }
        public void SetInt(int num) { Int = num; }
        public void SetLuk(int num) { luk = num; }


        public void mobskilluse(int skillno) {

            Skill skilldata;
            skilldata = Skill.Skills[skillno];
            int skillnum;

            int damage = Convert.ToInt32(skilldata.power + (this.Int + Command.rand.Next(this.luk)) * skilldata.power_factor);
            this.animation(skilldata.use_animation);
            Dictionary<int, UserCharacter> fieldD = new Dictionary<int, UserCharacter>(this.fieldData.Users);
            if (skillno == 5 || skillno == 7)
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {                       
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].damage(damage.ToString(), false);
                        continue;
                    }
                }
            }
            else if (skillno == 8)
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].canctrl += skilldata.power;
                        continue;
                    }
                }
            }
            else if (skillno == 10)
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].damage((fieldD[no].maxhp/5).ToString(), false);
                        continue;
                    }
                }
            }
            else if (skillno == 11)
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]) && !fieldD[no].passivelist.ContainsKey("독"))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].addPassive("독", 20);
                        continue;
                    }
                }
            }
            else if (skillno == 12)
            {
                
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].damage((fieldD[no].maxhp / new Random().Next(2, 7)).ToString(), false);
                        continue;
                    }
                }
            }
            else if (skillno == 13)
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].damage((fieldD[no].maxhp / 4).ToString(), false);
                        if (fieldD[no].soul - 1 > 0) { fieldD[no].soul -= 1; }
                        fieldD[no].userData.clientData.SendPacket(Packet.RogueReload(Rogue.rogues[fieldD[no].rogueno]));
                        continue;
                    }
                }
            }
            else
            {
                foreach (int no in fieldD.Keys)
                {
                    if (fieldD[no].hp <= 0) { continue; }
                    if (this.IsDead) { continue; }
                    if (SkillFunction.IsRange(skilldata.range_type, skilldata.range, this, fieldD[no]))
                    {
                        fieldD[no].animation(skilldata.target_animation);
                        fieldD[no].damage(damage.ToString(), false);
                        continue;
                    }
                }
            }
            
        }




        //int damage = Convert.ToInt32(skilldata.power + (this.Int + Command.rand.Next(u.luk)) * skilldata.power_factor);

        public void move(int _dir)
        {
            int dir = _dir;
            int new_x, new_y;
            if (target != null)
            { 
                dir = 0;
                new_x = x - target.x;
                new_y = y - target.y;
                dir = (new_x > 0 ? 4 : new_x < 0 ? 6 : 0);
                if (dir == 0)
                    dir = (new_y > 0 ? 8 : new_y < 0 ? 2 : 0);
            }
            if (fieldData.passable(x, y, dir))
            {
                x += (dir == 4 ? -1 : dir == 6 ? 1 : 0);
                y += (dir == 8 ? -1 : dir == 2 ? 1 : 0);
            }
            turn(dir);
            // Packet
            fieldData.AllSendPacket(Packet.EnemyMove(this));
        }

        public void movea(int _dir)
        {
            int dir = _dir;
            int new_x, new_y;
            if (targetE != null)
            {
                dir = 0;
                new_x = x - targetE.x;
                new_y = y - targetE.y;
                dir = (new_x > 0 ? 4 : new_x < 0 ? 6 : 0);
                if (dir == 0)
                    dir = (new_y > 0 ? 8 : new_y < 0 ? 2 : 0);
            }
            if (fieldData.passable(x, y, dir))
            {
                x += (dir == 4 ? -1 : dir == 6 ? 1 : 0);
                y += (dir == 8 ? -1 : dir == 2 ? 1 : 0);
            }
            turn(dir);
            // Packet
            fieldData.AllSendPacket(Packet.EnemyMove(this));
        }
        public void missilemove(int _dir)
        {
            if (IsDead) { return; }
            if (fieldData.passable(x, y, _dir))
            {

                        x += (_dir == 4 ? -1 : _dir == 6 ? 1 : 0);
                        y += (_dir == 8 ? -1 : _dir == 2 ? 1 : 0);

                        fieldData.AllSendPacket(Packet.EnemyMove(this));
            }
            else
            {
                dead(creator);
                fieldData.AllSendPacket(Packet.EnemyDead(this));
            }
            
            /*for (int i = 0; i <= fieldData.Enemies.Count; i++)
            {
                if (x == (fieldData.Enemies[i] as Enemy).x && y == (fieldData.Enemies[i] as Enemy).y)
                {
                    if((fieldData.Enemies[i] as Enemy) == this)
                    {
                        continue;
                    }
                    bool critical = Command.rand.Next(1000) < luk;
                    string dmg = Damage.atk(this, (fieldData.Enemies[i] as Enemy), critical);
                    if (dmg == "Miss")
                        critical = false;
                    (fieldData.Enemies[i] as Enemy).animation(animation_id);
                    (fieldData.Enemies[i] as Enemy).damage(dmg, critical, creator);


                    dead(creator);
                    return;
                }
            }*/

        }

            
        public void moveto(int _x, int _y, int _dir = 2)
        {
            x = _x;
            y = _y;
            direction = _dir;
            // Packet
            fieldData.AllSendPacket(Packet.EnemyMove(this));
        }
        public void turn(int dir)
        {
            direction = dir;
        }

        public void damage(string dmg, bool critical, Character attacker)
        {

            if (dmg != "Miss")
                if (hp - Convert.ToInt32(dmg) >= maxhp) { hp = maxhp; }
                else { hp -= pattern == 40 ? 1 : Convert.ToInt32(dmg); }
            fieldData.AllSendPacket(Packet.EnemyDamage(this, dmg, dmg == "Miss" ? false : critical));
            if (hp <= 0)
                dead(attacker);
        }

        public void dead(Character attacker)
        {
            hp = 0;
            IsDead = true;
            dead_count = rebirth_time;
            fieldData.AllSendPacket(Packet.EnemyDead(this));
            //Console.WriteLine("진동 패킷 전송");
            
            //fieldData.Users[attacker.no].userData

            
            if (pattern == 9)
            {
                return;
            }
            // 드랍아이템
            foreach (EnemyDropData item in dropData.Values)
                if (Command.rand.Next(1000) + 1 <= item.rate)
                    fieldData.addDropItem(x, y, item);
            if (attacker is UserCharacter)
            {
                (attacker as UserCharacter).gainExp(exp);
                if ((attacker as UserCharacter).rogueno != -1) { 
                    (attacker as UserCharacter).soul += 1;
                    (attacker as UserCharacter).userData.clientData.SendPacket(Packet.RogueReload(Rogue.rogues[(attacker as UserCharacter).rogueno]));
                }
                
            }

          
        }

        public void animation(int id)
        {
            fieldData.AllSendPacket(Packet.EnemyAnimation(this, id));
        }

        public void rebirth()
        {
            IsDead = false;
            hp = maxhp;
            fieldData.AllSendPacket(Packet.EnemyRebirth(this));
            moveto(start_x, start_y);
        }

        private void attack()
        {
            string dmg = "";
            if (delay_count > 0) { delay_count--; }
            // 타겟 검색 및 데미지 처리 부분
            if (target != null)
            {
                if (Math.Abs(target.x - x) + Math.Abs(target.y - y) > sight)
                    target = null;
                if (delay_count == 0 && target != null)
                {
                    int new_x = x + (direction == 4 ? -1 : direction == 6 ? 1 : 0);
                    int new_y = y + (direction == 8 ? -1 : direction == 2 ? 1 : 0);
                    if (new_x == target.x && new_y == target.y)
                    {
                        bool critical = Command.rand.Next(1000) < luk;
                        dmg = Damage.atk(this, target, critical);
                        if (dmg == "Miss")
                            critical = false;
                        target.animation(animation_id);
                        if (no == 88)
                        {
                            if (new Random().Next(1, 1000) <= 120)
                            {
                                critical = true;
                                direction = 10 - direction;
                                x = target.x * 2 - x;
                                y = target.y * 2 - y;
                            }
                        }
                        target.damage(dmg, critical);
                        delay_count = delay;
                    }
                }
                if (no == 83) {
                    if (new Random().Next(1, 1000) <= 80) {
                        damage((Convert.ToInt32(dmg) * (-1)).ToString(), false, this);
                    }
                }

                

                //주변의 캐릭터 검사해서 공격 줄 수 있는 함수 실행
                foreach (int i in skillList.Keys)
                {
                    if(CheckUseSkill(skillList[i]) == true)
                    {
                        mobskilluse(i);
                        break;
                    }
                }


                
            }
            else
            {
                // 범위 탐지
                foreach (UserCharacter u in fieldData.Users.Values)
                {
                    if (Math.Abs(u.x - x) + Math.Abs(u.y - y) <= sight)
                    {
                        target = u;
                        break;
                    }
                }
            }
        }

        public void attacka()
        {
            if (IsDead) { return; }
            if (delay_count > 0) { delay_count--; }
            if (targetE == null)
            {
                // 범위 탐지
                foreach (Enemy e in fieldData.Enemies)
                {
                    if (e == this || e.IsDead) { continue; }
                    if (Math.Abs(e.x - x) + Math.Abs(e.y - y) <= sight)
                    {
                        targetE = e;
                        break;
                    }
                }
            }
            // 타겟 검색 및 데미지 처리 부분
            if (targetE != null)
            {
                //if (Math.Abs(targetE.x - x) + Math.Abs(targetE.y - y) > sight)
                 //   targetE = null;
                if (delay_count == 0 && targetE != null)
                {
                    int new_x = x + (direction == 4 ? -1 : direction == 6 ? 1 : 0);
                    int new_y = y + (direction == 8 ? -1 : direction == 2 ? 1 : 0);
                    if (new_x == targetE.x && new_y == targetE.y)
                    {
                        Console.WriteLine("충돌");
                        bool critical = Command.rand.Next(1000) < luk;
                        string dmg = Damage.atk(this, targetE, critical);
                        if (dmg == "Miss")
                            critical = false;
                        targetE.animation(animation_id);
                        targetE.damage(dmg, critical, creator);

                        dead(creator);
                        fieldData.AllSendPacket(Packet.EnemyDead(this));
                        return;
                        //delay_count = delay;
                    }
                }
            }
        }



        public void update()
        {

            //Console.WriteLine("Enemy Update");
            if (pattern == 9)
            {
                //Console.WriteLine("Pattern 9");
                if (speed_count > 0) { speed_count--; }
                if (speed_count == 0 && pattern == 9)
                {
                    speed_count = 10 - move_speed;
                    missilemove(direction);
                    attacka();
                }
            }
            else if (pattern == 1)
            {
                //Console.WriteLine("Pattern 9");
                if (speed_count > 0) { speed_count--; }
                if (speed_count == 0 && pattern == 1)
                {
                    speed_count = 10 - move_speed;


                }
            }
            else if (pattern == 0)
            {
                if (IsDead)
                {
                    //Console.WriteLine("Pattern 0");
                    if (dead_count > 0) { dead_count--; }
                    if (dead_count == 0) { rebirth(); }
                }
                else
                {
                    if (speed_count > 0) { speed_count--; }
                    if (speed_count == 0 && pattern == 0)
                    {
                        speed_count = 10 - move_speed;

                        move((Command.rand.Next(4) + 1) * 2);
                    }

                    attack();
                }
            }
            else if (pattern == 40) { 

            }
            else
            {

            }

        }
    }

    class EnemyDropData
    {
        public int no { get; private set; }
        public int enemy_no { get; private set; }
        public int item_no { get; private set; }
        public int rate { get; private set; }
        public string image { get; private set; }
        public int pattern_x { get; private set; }
        public int pattern_y { get; private set; }
        public int min_price { get; private set; }
        public int min_str { get; private set; }
        public int min_dex { get; private set; }
        public int min_int { get; private set; }
        public int min_luk { get; private set; }
        public int min_hp { get; private set; }
        public int min_mp { get; private set; }
        public int min_solid { get; private set; }
        public int min_ability { get; private set; }
        public int min_cost { get; private set; }
        public int max_price { get; private set; }
        public int max_str { get; private set; }
        public int max_dex { get; private set; }
        public int max_int { get; private set; }
        public int max_luk { get; private set; }
        public int max_hp { get; private set; }
        public int max_mp { get; private set; }
        public int max_solid { get; private set; }
        public int max_ability { get; private set; }
        public int max_cost { get; private set; }
        public bool trade { get; private set; }
        public bool sell { get; private set; }
        public bool use { get; private set; }
        public int item_type { get; private set; }

        public EnemyDropData(int _item_no, int _type, string _image, int px, int py)
        {
            item_no = _item_no;
            item_type = _type;
            image = _image;
            pattern_x = px;
            pattern_y = py;
        }

        public EnemyDropData(DataRow rs)
        {
            no = Convert.ToInt32(rs["no"]);
            enemy_no = Convert.ToInt32(rs["enemy_no"]);
            item_no = Convert.ToInt32(rs["item_no"]);
            rate = Convert.ToInt32(rs["rate"]);
            image = rs["image"].ToString();
            pattern_x = Convert.ToInt32(rs["pattern_x"]);
            pattern_y = Convert.ToInt32(rs["pattern_y"]);
            min_price = Convert.ToInt32(rs["min_price"]);
            min_str = Convert.ToInt32(rs["min_str"]);
            min_dex = Convert.ToInt32(rs["min_dex"]);
            min_int = Convert.ToInt32(rs["min_int"]);
            min_luk = Convert.ToInt32(rs["min_luk"]);
            min_hp = Convert.ToInt32(rs["min_hp"]);
            min_mp = Convert.ToInt32(rs["min_mp"]);
            min_solid = Convert.ToInt32(rs["min_solid"]);
            min_ability = Convert.ToInt32(rs["min_ability"]);
            min_cost = Convert.ToInt32(rs["min_cost"]);
            max_price = Convert.ToInt32(rs["max_price"]);
            max_str = Convert.ToInt32(rs["max_str"]);
            max_dex = Convert.ToInt32(rs["max_dex"]);
            max_int = Convert.ToInt32(rs["max_int"]);
            max_luk = Convert.ToInt32(rs["max_luk"]);
            max_hp = Convert.ToInt32(rs["max_hp"]);
            max_mp = Convert.ToInt32(rs["max_mp"]);
            max_solid = Convert.ToInt32(rs["max_solid"]);
            max_ability = Convert.ToInt32(rs["max_ability"]);
            max_cost = Convert.ToInt32(rs["max_cost"]);
            trade = Convert.ToInt32(rs["trade"]) == 1 ? true : false;
            sell = Convert.ToInt32(rs["sell"]) == 1 ? true : false;
            use = Convert.ToInt32(rs["use"]) == 1 ? true : false;
        }
    }

    /*class copytool : ICloneable
    {
        Dictionary<int, UserCharacter> cop = new Dictionary<int, UserCharacter>();
        Dictionary<int, UserCharacter> root = new Dictionary<int, UserCharacter>();
        public object Clone()
        {
            cop = root.Clone
        }
    }*/

}
