using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NRP_Server.NetObject;
using MySqlX.XDevAPI;
using NRP_Server.Storage;

namespace NRP_Server
{
    class Field
    {
        public Dictionary<int, UserCharacter> Users = new Dictionary<int, UserCharacter>();
        public Dictionary<UserCharacter, Home> Homes = new Dictionary<UserCharacter, Home>();
        public Dictionary<int, DropItem> DropItems = new Dictionary<int, DropItem>();
        public ArrayList Enemies;
        public int mapid { get; private set; }
        public int seed { get; private set; }
        public climate fieldclimate { get; set; } = null;

        public double time { get; set; }

        public Field(int _mapid, int _seed)
        {
            mapid = _mapid;
            seed = _seed;
            time = 0;
            loadEnemyData();
        }

        // 필드 몬스터 데이터를 로드합니다.
        public void loadEnemyData()
        {
            int count = 0;
            Enemy obj;
            Enemies = new ArrayList();
            DataTable pos = Mysql.Query($"SELECT * FROM enemy_position WHERE map_id = '{mapid}'");
            DataTable ds;
            foreach (DataRow ps in pos.Rows)
            {
                ds = Mysql.Query($"SELECT * FROM enemy WHERE no = '{ps["enemy_no"]}'");
                obj = new Enemy(this);
                Enemies.Add(obj);
                obj.loadData(ds.Rows[0], Enemies.IndexOf(obj), mapid, Convert.ToInt32(ps["map_x"]), Convert.ToInt32(ps["map_y"]));
                count++;

            }
            Msg.Info($"[맵] {mapid}번 {count} 개의 몬스터 로드 완료");
        }

        // 몬스터를 생성합니다.
        public int addEnemy(int enemy_no, int x, int y)
        {
            Enemy obj;
            DataTable ds;
            int index;
            ds = Mysql.Query($"SELECT * FROM enemy WHERE no = '{enemy_no}'");
            obj = new Enemy(this);
            Enemies.Add(obj);
            index = Enemies.IndexOf(obj);
            obj.loadData(ds.Rows[0], index, mapid, x, y);
            // 패킷
            AllSendPacket(Packet.EnemyCreate(obj));
            return index;
        }
        // 몬스터를 제거합니다.
        public void deleteEnemy(int index)
        {
            if (index >= 0 && index < Enemies.Count)
            {
                Enemy obj = Enemies[index] as Enemy;
                // del Packet

                // del
                Enemies.Remove(obj);
            }
        }

        // 충돌 판정
        public bool passable(int x, int y, int dir)
        {
            int new_x, new_y;
            Map map = Map.Maps[mapid];
            new_x = x + (dir == 4 ? -1 : dir == 6 ? 1 : 0);
            new_y = y + (dir == 8 ? -1 : dir == 2 ? 1 : 0);
            // 범위 외 값 판정
            if (new_x >= map.width || new_y >= map.height || new_x < 0 || new_y < 0)
                return false;
            // 유저 충돌판정
            foreach (UserCharacter c in Users.Values)
                if (c.x == new_x && c.y == new_y) { return false; }
            // NPC 충돌판정
            foreach (NPC npc in map.npcData.Values)
                if (npc.x == new_x && npc.y == new_y) { return false; }
            // 넷이벤트 충돌판정
            foreach (Enemy enemy in Enemies)
                if (enemy.x == new_x && enemy.y == new_y && !enemy.IsDead) { return false; }
            // 갈 수 없는 곳 판정
            return (map.flagData[new_x, new_y] == 1 ? false : true);
        }
        // 포탈(장소이동) 판정
        public bool portal(UserCharacter u)
        {
            if (Users.ContainsKey(u.no))
            {
                Map map = Map.Maps[mapid];
                // 포탈 판정
                if (map.portalData.ContainsKey($"{u.x},{u.y}"))
                {
                    Portal portal = map.portalData[$"{u.x},{u.y}"];
                    // 이동시키기
                    leave(u.no);
                    Map.Maps[portal.move_mapid].Fields[0].join(u, portal.move_x, portal.move_y);

                    return true;
                }
            }
            return false;
        }

        public bool isportal(int mapid, int x, int y)
        {
            Map map = Map.Maps[mapid];
            // 포탈 판정
            if (map.portalData.ContainsKey($"{x},{y}"))
            {
                return true;
            }
            return false;
        }

        // 필드의 유저들을 해당 유저에게 로드합니다.
        public bool loadUser(UserCharacter u)
        {
            if (!Users.ContainsKey(u.no))
                return false;
            foreach (UserCharacter c in Users.Values)
            {
                if (c == u) { continue; }
                u.userData.clientData.SendPacket(Packet.CharacterCreate(c));
            }
            return true;
        }
        // 필드의 NPC들을 해당 유저에게 로드합니다.
        public bool loadNPC(UserCharacter u)
        {
            if (!Users.ContainsKey(u.no))
                return false;
            foreach (NPC obj in Map.Maps[mapid].npcData.Values)
                u.userData.clientData.SendPacket(Packet.NPCCreate(obj));

            return true;
        }
        // 필드의 몬스터들을 해당 유저에게 로드합니다.
        public bool loadEnemy(UserCharacter u)
        {
            if (!Users.ContainsKey(u.no))
                return false;
            foreach (Enemy obj in Enemies)
                u.userData.clientData.SendPacket(Packet.EnemyCreate(obj));

            return true;
        }
        // 필드의 드랍아이템들을 해당 유저에게 로드합니다.
        public bool loadDropItem(UserCharacter u)
        {
            if (!Users.ContainsKey(u.no))
                return false;
            foreach (DropItem obj in DropItems.Values)
                u.userData.clientData.SendPacket(Packet.DropItemCreate(obj));

            return true;
        }


        // 드랍아이템을 생성합니다.
        public void addDropItem(int x, int y, EnemyDropData item)
        {
            int index = 0;
            for (int i = 0; i < 10000; i++)
                if (!DropItems.ContainsKey(i)) { index = i; break; }

            DropItem obj = new DropItem(index, x, y, item);
            DropItems.Add(index, obj);

            // 모든유저 패킷
            AllSendPacket(Packet.DropItemCreate(obj));
        }
        public void addDropItem(int x, int y, Item item, int number = 1)
        {
            int index = 0;
            for (int i = 0; i < 10000; i++)
                if (!DropItems.ContainsKey(i)) { index = i; break; }

            DropItem obj = new DropItem(index, x, y, item, number);
            DropItems.Add(index, obj);

            // 모든유저 패킷
            AllSendPacket(Packet.DropItemCreate(obj));
        }
        // 드랍아이템을 획득합니다.
        public void gainDropItem(UserCharacter u, int index)
        {
            DropItem item = DropItems[index];
            item.gain(u);
            deleteDropItem(index);
        }
        // 드랍아이템을 제거합니다.
        public void deleteDropItem(int index)
        {
            DropItems.Remove(index);
            AllSendPacket(Packet.DropItemDelete(index));
        }

        public void deleteDropItems()
        {
            foreach (int index in new Dictionary<int, DropItem>(DropItems).Keys) {
                DropItems.Remove(index);
                AllSendPacket(Packet.DropItemDelete(index));
            }
        }

        public bool isalldead() {
            foreach (Enemy ei in Enemies) {
                if (!ei.IsDead) { return false; }
            }
            return true;
        }
        // 필드에 해당 유저를 입장시킵니다.
        public bool join(UserCharacter u, int _x, int _y)
        {
            if (Users.ContainsKey(u.no)) { return false; }
            u.FieldData = this;

            if (fieldclimate != null && u.fieldData.fieldclimate == this.fieldclimate)
            { u.fieldData.fieldclimate.weatherNotice(u); }
            else { u.userData.clientData.SendPacket(Packet.WeatherClear()); }
            Users.Add(u.no, u);
            AllSendPacket(Packet.CharacterCreate(u));
            u.moveto(_x, _y, u.direction);
            loadUser(u);
            loadNPC(u);
            loadEnemy(u);
            loadDropItem(u);


            return true;
        }

        // 필드에서 해당 유저를 퇴장시킵니다.
        public bool leave(int no)
        {
            if (!Users.ContainsKey(no))
                return false;

            
            
            foreach (UserCharacter _char in Users.Values)
            {
                if (_char.no == no) { continue; }
                _char.userData.clientData.SendPacket(Packet.DeleteCharacter(Users[no]));
            }
            foreach (Enemy enemy in Enemies)
                if (enemy.target == Users[no])
                    enemy.target = null;
            Users[no].fieldData = null;
            Users[no].EverReload = 0;
            Users.Remove(no);

            
            return true;
        }

        // 필드에 있는 모든 유저에게 패킷 전송
        public void AllSendPacket(Hashtable data)
        {
            try
            {
                foreach (UserCharacter c in Users.Values)
                    c.userData.clientData.SendPacket(data);
            }
            catch (Exception e)
            {
                Msg.Error(e.ToString());
            }

        }

        // 0.1초마다 실행되는 함수
        public void update()
        {
            

            if (Users.Count == 0) { return; }
            ArrayList enemylist = Enemies;
            foreach (Enemy obj in enemylist) { obj.update(); }
            ArrayList Userlist = new ArrayList(Users.Keys);
            try { foreach (int i in Userlist) { Users[i].update(); } }
            catch (Exception e) { Console.WriteLine(e); }
            foreach (UserCharacter ui in Users.Values) {
                if (ui.EverReload == 0) {
                    loadUser(ui);
                    loadNPC(ui);
                    loadEnemy(ui);
                    loadDropItem(ui);
                    ui.EverReload = 1;
                }
            }
                

            time += 1;

            //(자연수) 초일때만, 즉 1초마다 실행
            if (time%10 == 0) {
                if (Rogue.stagetype.Values.ToList().Contains(mapid)) { return; }
                if (fieldclimate != null) {
                    int sec = Convert.ToInt32(time / 10);
                    //이미 날씨가 있어서, 그에 대한 효과 실행
                    if (sec % 10 == 0 && fieldclimate.weatherno == 1) {
                        fieldclimate.weatherEffect(this);
                    }
                    else if (sec % 3 == 0 && fieldclimate.weatherno == 2)
                    {
                        fieldclimate.weatherEffect(this);
                    }
                    else if (sec % 5 == 0 && fieldclimate.weatherno == 3)
                    {
                        fieldclimate.weatherEffect(this);
                    }
                    else { }

                    if (fieldclimate.duration - 1 <= 0) { 
                        fieldclimate = null;
                        climate.climates.Remove(fieldclimate.no);
                        foreach (UserCharacter u in Users.Values)
                        {
                            u.userData.clientData.SendPacket(Packet.Notice(0, 255, 55, "하늘이 맑아집니다."));
                            u.userData.clientData.SendPacket(Packet.WeatherClear());
                        }
                    }
                    else { fieldclimate.duration-=1;}
                   }
                else {
                    //날씨가 없어서, 날씨 랜덤 생성
                    Random r = new Random();
                    int rn = r.Next(0, 1000);
                    int dur = r.Next(60, 60 * 5);
                    if (rn <= 30) {
                        this.fieldclimate = new climate(1, dur);
                        this.fieldclimate.weatherNotice(this);
                    }
                    else if (30 < rn && rn <= 60)
                    {
                        this.fieldclimate = new climate(2, dur);
                        this.fieldclimate.weatherNotice(this);
                    }
                    else if (60 < rn && rn <= 90)
                    {
                        this.fieldclimate = new climate(3, dur);
                        this.fieldclimate.weatherNotice(this);
                    }
                }
                
            }
            
        }

        public bool dispose() {
            if (!Map.Maps[mapid].Fields.ContainsKey(seed))
                return false;
            Map.Maps[mapid].Fields.Remove(seed);
            return true;
        }
    }
}
