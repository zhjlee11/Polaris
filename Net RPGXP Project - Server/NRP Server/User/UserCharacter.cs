using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NRP_Server.Storage;

namespace NRP_Server
{
    class UserCharacter : Character
    {
        #region 컨버터
        private static int ToInt(object data)
        {
            return Convert.ToInt32(data);
        }
        # endregion

        // public Variable
        public UserData userData;
        public Field fieldData;
        public Field FieldData
        {
            set { fieldData = value; mapid = fieldData.mapid; }
        }
        // 변수
        public int index { get; private set; }
        public Dictionary<Item, UserItem> Inventory { get; set; }
        public UserItem[] Equipment { get; set; }
        public Dictionary<Skill, UserSkill> Skills { get; set; }

        public Dictionary<Item, UserItem> ownitems = new Dictionary<Item, UserItem>();

        public int canctrl = 0;

        public int gold { get; private set; }
        // 스탯 오버라이드
        public int Mystr { get; private set; }
        public int Mydex { get; private set; }
        public int Myint { get; private set; }
        public int Myluk { get; private set; }
        public int Mymaxhp { get; private set; }
        public int Mymaxmp { get; private set; }
        public int deltatime { get; private set; } = 0;

        public List<int> address = null;

        public int partyinviteno { get; set; } = -1;

        public int partyno { get; set; } = -1;

        public override int str
        {
            get
            {
                int stat = Mystr;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.str;
                }
                return stat;
            }
            protected set { Mystr = value; }
        }
        public override int dex
        {
            get
            {
                int stat = Mydex;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.dex;
                }
                return stat;
            }
            protected set { Mydex = value; }
        }
        public override int Int
        {
            get
            {
                int stat = Myint;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.Int;
                }
                return stat;
            }
            protected set { Myint = value; }
        }
        public override int luk
        {
            get
            {
                int stat = Myluk;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.luk;
                }
                return stat;
            }
            protected set { Myluk = value; }
        }
        public override int maxhp
        {
            get
            {
                int stat = (level - 1) * 25 + 100;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.hp;
                }
                return stat;
            }
            protected set { Mymaxhp = value; }
        }
        public override int maxmp
        {
            get
            {
                int stat = Mymaxmp;
                foreach (UserItem equip in Equipment)
                {
                    if (equip == null) { continue; }
                    stat += equip.itemData.mp;
                }
                return stat;
            }
            protected set { Mymaxmp = value; }
        }

        //
        public Hashtable variables { get; set; }

        // NPC 대화
        public NPC npcData { get; set; }
        public bool isMessage { get; set; }
        public int page { get; set; }
        public int[] action { get; set; }
        public void resetMessage()
        {
            npcData = null;
            isMessage = false;
            page = 0;
            action = null;
            userData.clientData.SendPacket(Packet.EventTrigger());
        }

        public void UpStr(int num) { str += num; }
        public void UpDex(int num) { dex += num; }
        public void UpInt(int num) { Int += num; }
        public void UpLuk(int num) { luk += num; }
        public void DownPoint(int num) { if (point - num <= 0) { point = 0; } else { point -= num; } }

        public void ReloadStatus() {
            userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
            saveData();
        }

        // Object
        public UserCharacter(UserData u, int _index)
        {
            userData = u;
            index = _index;
            isMessage = false;
            page = 0;
        }

        // 유저 정보 세팅
        public void loadData(DataRow rs)
        {
            no = ToInt(rs["no"]);
            name = rs["name"].ToString();
            image = rs["image"].ToString();
            level = ToInt(rs["level"]);
            job = ToInt(rs["job"]);
            exp = ToInt(rs["exp"]);
            gold = ToInt(rs["gold"]);
            maxhp = ToInt(rs["maxhp"]);
            maxmp = ToInt(rs["maxmp"]);
            hp = ToInt(rs["hp"]);
            mp = ToInt(rs["mp"]);
            str = ToInt(rs["str"]);
            dex = ToInt(rs["dex"]);
            Int = ToInt(rs["int"]);
            luk = ToInt(rs["luk"]);
            point = ToInt(rs["point"]);


            mapid = ToInt(rs["map_id"]);
            x = ToInt(rs["map_x"]);
            y = ToInt(rs["map_y"]);
            direction = ToInt(rs["direction"]);
            move_speed = ToInt(rs["move_speed"]);
        }
        public void loadItems()
        {
            try
            {
                userData.clientData.SendPacket(Packet.ItemClear());
                // 아이템 저장 공간 정의
                Inventory = new Dictionary<Item, UserItem>();
                Equipment = new UserItem[11];

                DataTable ds = Mysql.Query($"SELECT * FROM user_inventory WHERE char_no = '{no}' AND seed = '{fieldData.seed}'");
                
                int data_no;
                foreach (DataRow rs in ds.Rows)
                {
                    data_no = ToInt(rs["item_no"]);
                    if (ToInt(rs["item_type"]) == 0)
                        Inventory.Add(Item.Equipments[data_no], new UserItem(Item.Equipments[data_no], ToInt(rs["number"])));
                    else
                        Inventory.Add(Item.Items[data_no], new UserItem(Item.Items[data_no], ToInt(rs["number"])));
                }
                // 착용 장비 로드
                ds = Mysql.Query($"SELECT * FROM user_equipment WHERE char_no = '{no}' AND seed = '{fieldData.seed}'");
                foreach (DataRow rs in ds.Rows)
                {
                    data_no = ToInt(rs["item_no"]);
                    if (Equipment[Item.Equipments[data_no].equip_type] != null && Item.Equipments[data_no].equip_type == 6)
                        Equipment[10] = new UserItem(Item.Equipments[data_no], 0);
                    else
                        Equipment[Item.Equipments[data_no].equip_type] = new UserItem(Item.Equipments[data_no], 0);
                }

                foreach (UserItem item in Inventory.Values)
                    userData.clientData.SendPacket(Packet.Item(item, 1));

                foreach (UserItem item in Equipment)
                    if (item != null)
                        userData.clientData.SendPacket(Packet.Item(item, 2));
            }
            catch (Exception e)
            {
                Msg.Error(e.ToString());
            }
        }
        public void loadSkills()
        {
            // 스킬 로드
            Skill skill;
            UserSkill obj;
            Skills = new Dictionary<Skill, UserSkill>();
            DataTable ds = Mysql.Query($"SELECT * FROM user_skill WHERE char_no = '{no}'");
            foreach (DataRow rs in ds.Rows)
            {
                skill = Skill.Skills[ToInt(rs["skill_no"])];
                obj = new UserSkill(skill, rs);
                Skills.Add(skill, obj);
            }
            foreach (UserSkill s in Skills.Values)
                userData.clientData.SendPacket(Packet.LoadSkill(s));
        }

        public void gainItem(Item item, int num = 0)
        {
            if (Inventory.ContainsKey(item))
            {
                Mysql.Query($"UPDATE user_inventory SET number = number + {num} WHERE char_no = '{no}' AND item_no = '{item.no}'");
                Inventory[item].number += num;
                userData.clientData.SendPacket(Packet.Item(Inventory[item]));
            }
            else
            {
                Mysql.Query($"INSERT INTO user_inventory (char_no,item_no,item_type,number,seed) VALUES ('{no}','{item.no}','{item.type}','{num}','{fieldData.seed}')");
                Inventory.Add(item, new UserItem(item, num));
                userData.clientData.SendPacket(Packet.Item(Inventory[item], 1));
            }
        }
        


        public void loseItem(Item item, int num = 0)
        {
            if (Inventory.ContainsKey(item))
            {
                DataTable rs = Mysql.Query($"SELECT * FROM user_inventory WHERE char_no = '{no}' AND item_no = '{item.no}'");
                if (ToInt(rs.Rows[0]["number"]) > num)
                {
                    Mysql.Query($"UPDATE user_inventory SET number = number - {num} WHERE char_no = '{no}' AND item_no = '{item.no}'");
                    Inventory[item].number = ToInt(rs.Rows[0]["number"]) - num;
                    userData.clientData.SendPacket(Packet.Item(Inventory[item]));
                }
                else if (ToInt(rs.Rows[0]["number"]) <= num)
                {
                    Mysql.Query($"DELETE FROM user_inventory WHERE char_no = '{no}' AND item_no = '{item.no}'");
                    Inventory.Remove(item);
                    userData.clientData.SendPacket(Packet.ItemClear(3, item.type, item.no));
                }
            }
        }

        public void equipItem(Item item)
        {
            if (item.type != 0)
                return;
            if (!Inventory.ContainsKey(item))
                return;
            removeItem(item, false);
            Mysql.Query($"INSERT INTO user_equipment (char_no,item_no,seed) VALUES ('{no}','{item.no}','{fieldData.seed}')");
            loseItem(item);
            UserItem equip = new UserItem(item, 0);
            Equipment[item.equip_type] = equip;
            if (item.hp != 0)
                damage((-item.hp).ToString(), false);
            userData.clientData.SendPacket(Packet.Item(equip, 2));
            userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
        }

        public void removeItem(Item item, bool packet = true)
        {
            UserItem equip;
            equip = Equipment[item.equip_type];
            if (equip == null)
                return;
            Equipment[item.equip_type] = null;
            gainItem(equip.itemData);
            Mysql.Query($"DELETE FROM user_equipment WHERE char_no = '{no}' AND item_no = '{equip.itemData.no}'");
            userData.clientData.SendPacket(Packet.ItemClear(4, equip.itemData.type, equip.itemData.no));
            if (packet)
                userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
        }

        public void trashItem(Item item, int number)
        {
            if (!Inventory.ContainsKey(item)) { return; }
            if (item.type != 0)
            {
                if (Inventory[item].number < number || number == 0) { return; }
            }
            else if (number != 0)
                return;

            loseItem(item, number);
            fieldData.addDropItem(x, y, item, number);
        }

        

        public void useItem(Item item)
        {
            if (Inventory.ContainsKey(item))
            {
                MethodInfo method = typeof(ItemFunction).GetMethod(item.method_name, BindingFlags.Public | BindingFlags.Static);
                if (method == null) { return; }
                object[] args = { this, item, item.method_arg };
                if ((bool)method.Invoke(null, args))
                    loseItem(item, 1);
            }
        }

        public void learnSkill(Skill skill)
        {
            if (!Skills.ContainsKey(skill))
            {
                // 스킬 안배움
                Mysql.Query($"INSERT INTO user_skill (char_no, skill_no) VALUES ('{no}', '{skill.no}')");
                Skills.Add(skill, new UserSkill(skill));
                userData.clientData.SendPacket(Packet.LoadSkill(Skills[skill]));
                userData.clientData.SendPacket(Packet.Dialog(0, "기술 습득", "성공적으로 기술을 배웠습니다."));
            }
            else
            {
                userData.clientData.SendPacket(Packet.Dialog(0, "기술 습득 오류", "이미 배운 기술입니다."));
            }
        }

        // 레벨 관련
        public void gainExp(int value)
        {
            exp += value;
            while (true)
            {
                if (Exp.getLevel(level) <= exp)
                    levelUp();
                else
                    break;
            }
            userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
        }

        // 골드 획득
        public void gainGold(int value)
        {
            gold += value;
            userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
        }

        public void loseGold(int value)
        {
            gold -= value;
            userData.clientData.SendPacket(Packet.CharacterStatusUpdate(this));
        }

        private void levelUp(int up_value = 1)
        {
            exp -= Exp.getLevel(level);
            level += up_value;
            animation(15);
            damage((-maxhp).ToString(), false);
            if (Job.JobList[job].JobSkillList.ContainsKey(level))
            {
                learnSkill(Skill.Skills[Job.JobList[job].JobSkillList[level]]);
            }
            userData.clientData.SendPacket(Packet.UserChat("\\C[250,250,50]레벨 업!!"));
        }

        // 유저 정보 저장
        public void saveData()
        {
            string str = "";
            str += $"name='{name}',image='{image}',level='{level}',guild='{guild}',`job`='{job}',`exp`='{exp}',gold='{gold}',maxhp='{Mymaxhp}',maxmp='{Mymaxmp}',hp='{hp}',mp='{mp}',str='{Mystr}',dex='{Mydex}',`int`='{Myint}',luk='{Myluk}',point='{point}',";
            str += $"map_id='{mapid}',map_x='{x}',map_y='{y}',direction='{direction}',move_speed='{move_speed}'";
            Mysql.Query($"UPDATE user_character SET {str} WHERE no = '{no}'");
        }

        // 이동관련 함수
        public void move(int dir)
        {
            if (isStun())
            {
                this.userData.clientData.SendPacket(Packet.UserChat("스턴에 걸려서 움직일 수 없습니다.", ""));
            }
            else
            {
                if (fieldData.passable(x, y, dir))
                {
                    x += (dir == 4 ? -1 : dir == 6 ? 1 : 0);
                    y += (dir == 8 ? -1 : dir == 2 ? 1 : 0);
                }
                turn(dir);
            }
            if (!fieldData.portal(this))
                fieldData.AllSendPacket(Packet.Move(this));



        }

        public bool isStun()
        {
            if (this.canctrl > 0) { this.canctrl--; return true; }
            else { return false; }
        }

        public void moveto(int _x, int _y, int _dir = 2)
        {
            x = _x;
            y = _y;
            direction = _dir;
            fieldData.AllSendPacket(Packet.Move(this));
        }
        public void turn(int dir)
        {
            direction = dir;
        }

        // 전투 관련 함수
        public void damage(string dmg, bool critical)
        {
            if (dmg != "Miss")
                hp -= ToInt(dmg);
            if (hp > maxhp)
                hp = maxhp;

            // 죽었다면?
            if (hp <= 0)
            {


                //seed 추출
                int oldseed = this.fieldData.seed;
                Field oldfield = this.fieldData;
                fieldData.leave(no);
                //장비 장착 해제 & 인벤토리에 넣기
                fieldData = oldfield;
                foreach (UserItem ui in this.Equipment)
                {
                    try
                    {
                        gainItem(ui.itemData, 1);
                        removeItem(ui.itemData);
                    }
                    catch (Exception e) { }
                    
                }
                fieldData = null;
                //UI 귀속 설정 UI On
                this.userData.clientData.SendPacket(Packet.SetMakeOwn());
            }
            else { fieldData.AllSendPacket(Packet.CharacterDamage(this, dmg, critical)); }
            
        }
        public void animation(int id)
        {
            fieldData.AllSendPacket(Packet.CharacterAnimation(this, id));
        }

        public bool hasfarm()
        {
            DataTable ds = Mysql.Query($"SELECT * FROM farm_list WHERE no = '{this.no}'");
            try
            {
                if (ds.Rows[0] == null) { return false; }
                else { return true; }
            }
            catch (Exception e) { return false;  }
            
        }

        public void water(UserCharacter u)
        {
            int watermachine = 58; //물뿌리게 아이템 no
            
            if (u.hasfarm() == false)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "물 주기", "가지고 있는 농장이 없습니다."));
                return;
            }
            int waternum = Convert.ToInt32(Mysql.Query($"SELECT * FROM farm_list WHERE no = '{u.no}'").Rows[0]["water"]);
            if (waternum >= 10)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "물 주기", "수확이 가능합니다! 더 이상 물은 줄 수 없습니다."));
                return;
            }
            try
            {
                if (u.Inventory[Item.Items[watermachine]].number < 1)
                {
                    u.userData.clientData.SendPacket(Packet.Dialog(0, "물 주기", $"물을 줄 수 있는 도구가 없습니다."));
                    return;
                }
            }
            catch (Exception e)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "물 주기", $"물을 줄 수 있는 도구가 없습니다."));
                return;
            }
            int num = 1;
            u.loseItem(Item.Items[watermachine], num);
            Mysql.Query($"UPDATE farm_list SET water = {waternum + num} WHERE no = {u.no}");
            u.userData.clientData.SendPacket(Packet.Dialog(0, "물 주기", $"물을 주었습니다! 지금까지 '{waternum + num}'번 물을 주었습니다!"));

        }

        public void getfarm(UserCharacter u)
        {
            if (u.hasfarm() == false)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "수확", "가지고 있는 농장이 없습니다."));
                return;
            }
            if (Convert.ToInt32(Mysql.Query($"SELECT * FROM farm_list WHERE no = '{u.no}'").Rows[0]["water"]) < 10)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "수확", "아직 다 자라지 않았습니다. 수확이 불가능합니다."));
                return;
            }
            string crop = Mysql.Query($"SELECT * FROM farm_list WHERE no = '{u.no}'").Rows[0]["crop"].ToString();
            int num = Convert.ToInt32(Mysql.Query($"SELECT * FROM farm_list WHERE no = '{u.no}'").Rows[0]["num"]);
            int cropid = -1;
            if (crop == "벼")
            {
                cropid = 76;
            }
            u.gainItem(Item.Items[cropid], num);
            Mysql.Query($"DELETE FROM farm_list WHERE no = '{u.no}'");
            u.userData.clientData.SendPacket(Packet.Dialog(0, "수확", $"'{crop}'를 '{num}'개 수확했습니다!"));
        }

        public void seedfarm(UserCharacter u, string cropname, int seedid, int num)
        {
            int cost = 50000; // 농장 구매가
            if (u.hasfarm() == true)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "씨앗 심기", "이미 씨앗을 심었습니다."));
                return;
            }
            if (u.gold < cost)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "씨앗 심기", $"농장 구매 가격인 '{cost}'만큼의 돈이 없습니다."));
                return;
            }
            try
            {
                if (u.Inventory[Item.Items[seedid]].number < num)
                {
                    u.userData.clientData.SendPacket(Packet.Dialog(0, "씨앗 심기", $"'{cropname}'씨앗을 '{num}'개 심을 수 없습니다. 현재 가지고 계신 씨앗의 개수는 '{u.Inventory[Item.Items[seedid]].number}'개입니다."));
                    return;
                }
            }
            catch (Exception e)
            {
                u.userData.clientData.SendPacket(Packet.Dialog(0, "씨앗 심기", $"'{cropname}'씨앗을 '{num}'개 심을 수 없습니다. 가지고 있는 '{cropname}'씨앗이 없습니다."));
                return;
            }

            u.loseGold(cost);
            u.loseItem(Item.Items[seedid], num);
            Mysql.Query($"INSERT INTO farm_list (no,crop,num,water) VALUES ('{u.no}','{cropname}','{num}','{0}')");
            u.userData.clientData.SendPacket(Packet.Dialog(0, "씨앗 심기", $"씨앗 심기에 성공했습니다!"));
        }

        public object Infofarm(String type) {
            if (this.hasfarm() == false) { return "소유하고 있는 농장이 없습니다."; }
            if (type == "farmname") { return Mysql.Query($"SELECT * FROM farm_list WHERE no = '{no}'").Rows[0]["crop"].ToString(); }
            else if (type == "farmnum") { return Convert.ToInt32(Mysql.Query($"SELECT * FROM farm_list WHERE no = '{no}'").Rows[0]["num"]); }
            else if (type == "waternum") { return Convert.ToInt32(Mysql.Query($"SELECT * FROM farm_list WHERE no = '{no}'").Rows[0]["water"]); }
            else { return false; }
        }

        public void attack()
        {
            int new_x, new_y;
            bool critical;
            string dmg;
            new_x = x + (direction == 4 ? -1 : direction == 6 ? 1 : 0);
            new_y = y + (direction == 8 ? -1 : direction == 2 ? 1 : 0);
            foreach (Enemy enemy in fieldData.Enemies)
            {
                if (enemy.IsDead) { continue; }
                if (enemy.x == new_x && enemy.y == new_y)
                {
                    critical = Command.rand.Next(1000) < luk;
                    //크리티컬일시 채팅 띄우고 쉐이크
                    if (critical == true) { this.userData.clientData.SendPacket(Packet.UserChat("크리티컬!", "")); this.userData.clientData.SendPacket(Packet.UserWinEffect()); }
                    dmg = Damage.atk(this, enemy, critical);
                    enemy.damage(dmg, critical, this);
                    // Animation
                    enemy.animation(7);
                    break;
                }
            }
        }

        public void addPassive(string name, int time)
        {
            if (this.passivelist.ContainsKey(name) == true) { this.passivelist[name] += time; }
            else { this.passivelist[name] = time; }
        }

        public void update()
        {
            if (this.deltatime == 10)
            {
                this.deltatime = 0;
                Dictionary<string, double> plist = new Dictionary<string, double>(this.passivelist);
                foreach (string p in plist.Keys)
                {
                    //버프&디버프 비교는 아래의 조건문에다.
                    if (p == "독")
                    {
                        double perd = this.maxhp / 150;
                        this.damage(Math.Round((double)(this.maxhp * 3 / 100), 0, MidpointRounding.AwayFromZero).ToString(), false);
                        this.passivelist[p] -= 1;
                        if (this.passivelist[p] <= 0) { this.passivelist.Remove(p); this.userData.clientData.SendPacket(Packet.UserChat(String.Format("\\C[255,0,51] {0}가 해제되었습니다.", p))); continue; }
                        this.userData.clientData.SendPacket(Packet.UserChat(String.Format("\\C[255,0,51] {0}의 남은 지속시간은 {1}초입니다.", p, this.passivelist[p])));
                    }
                }
            }
            else if (this.deltatime > 10) { Console.WriteLine("deltatime 값이 10 초과입니다."); this.deltatime = 0; }
            else { this.deltatime += 1; }
        }

        public Combination GetRecipe(string name)
        {
            Combination ui = null;
            foreach (Combination c in Combination.recipes)
            {
                if (c.name == name)
                {
                    ui = c;
                    break;
                }
            }
            return ui;
        }

        public bool isininvetory(UserItem ui) {
            if (Inventory.ContainsKey(ui.itemData) && Inventory[ui.itemData].number>= ui.number) { return true; }
            else { return false; }
        }

        public bool combinate(string name)
        {
            Combination c = GetRecipe(name);
            if (c == null) {  return false; } 
            foreach (UserItem ui in c.elements) {
                if (isininvetory(ui) == true) {  }
                else {  return false; }
            }
            foreach (UserItem ui in c.elements) {
                loseItem(ui.itemData, ui.number);
            }
            foreach (UserItem ui in c.results) {
                gainItem(ui.itemData, ui.number);
            }
            return true;
        }
    }
}
