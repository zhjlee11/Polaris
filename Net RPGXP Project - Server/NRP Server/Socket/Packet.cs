using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using NRP_Server.Storage;
using NRP_Server.Group;

namespace NRP_Server
{
    class Packet
    {
        #region 상수 값 / 관리자 정의
        // Client Part
        private const int VERSION_CHECK = 2;
        private const int DIALOG = 3;

        // Character Part
        private const int CHARACTER_JOIN = 100;
        private const int CHARACTER_CREATE = 101;
        private const int CHARACTER_MOVE = 102;
        private const int CHARACTER_MOVETO = 103;
        private const int CHARACTER_DELETE = 104;
        private const int CHARACTER_ITEM = 105;
        private const int CHARACTER_ITEM_CLEAR = 106;
        private const int CHARACTER_KEY_TRIGGER = 107;
        private const int CHARACTER_DAMAGE = 108;
        private const int CHARACTER_ANIMATION = 109;
        private const int CHARACTER_DEAD = 110;
        private const int CHARACTER_REBIRTH = 111;
        private const int CHARACTER_STATUS = 112;
        private const int CHARACTER_TRASH_ITEM = 113;
        private const int CHARACTER_STORE_LOAD = 114;
        private const int CHARACTER_STORE_BUY = 115;
        private const int CHARACTER_STORE_SELL = 116;
        private const int CHARACTER_SKILL_TRIGGER = 117;
        private const int CHARACTER_SKILL_LOAD = 118;

        // NetEvent Part
        private const int NETEVENT_CREATE = 200;
        private const int NETEVENT_DELETE = 201;
        private const int NETEVENT_ANIMATE = 202;
        private const int NETEVENT_MOVE = 203;
        private const int NETEVENT_TRIGGER = 204;

        // Enemy Part
        private const int ENEMY_CREATE = 300;
        private const int ENEMY_DELETE = 301;
        private const int ENEMY_MOVE = 302;
        private const int ENEMY_DAMAGE = 303;
        private const int ENEMY_ANIMATION = 304;
        private const int ENEMY_DEAD = 305;
        private const int ENEMY_REBIRTH = 306;

        // User Part
        private const int USER_LOGIN = 500;
        private const int USER_IDCHECK = 501;
        private const int USER_SIGNUP = 502;
        private const int USER_CREATE_CHARACTER = 503;
        private const int USER_SELECT_CHARACTER = 504;
        private const int USER_DELETE_CHARACTER = 505;
        private const int USER_CHAT = 506;



        // Custom Part
        private const int CUSTOM_PACKET = 900;

        private const int ENEMY_WIN_EFFECT = 901;
        private const int ROOM_CHANGE = 902;

        private const int FarmUI_OPEN = 903;
        private const int FarmSEED = 904;

        private const int FINISH_MAKE_OWN = 905;
        private const int SET_MAKE_OWN = 906;
        private const int COMBINATION = 907;

        private const int ITEM_COMBINATION = 908;
        private const int ITEM_ELEMENTS = 909;
        private const int ITEM_RESULTS = 910;
        private const int ITEM_ASK = 911;
        private const int ITEM_LOAD = 912;

        private const int FARM_INFO = 913;
        private const int FARM_SEED = 914;
        private const int FARM_WATER = 915;
        private const int FARM_GET = 916;
        private const int FARM_INFO_REPLY = 917;

        private const int NOTICE = 918;

        private const int PARTY_DENY = 919;
        private const int PARTY_CREATE = 920;
        private const int PARTY_INVITE = 921;
        private const int PARTY_ACCEPT = 922;
        private const int PARTY_INFO = 923;
        private const int PARTY_EXIT = 924;
        private const int PARTY_DESTORY = 925;
        private const int PARTY_INFO_REPLY = 926;

        private const int STATUS_STR = 927;
        private const int STATUS_DEX = 928;
        private const int STATUS_INT = 929;
        private const int STATUS_LUK = 930;


        // Admin
        public static string[] ADMIN = { "admin" };
        #endregion
        #region 컨버터
        private static int ToInt(object data)
        {
            return Convert.ToInt32(data);
        }
        #endregion
        public static event BroadCastHndlr BroadCast;
        public delegate void BroadCastHndlr(Hashtable data);

        public static bool RecvData(IAsyncResult ar)
        {

            // 보낸 유저 정보 세팅
            ClientInfo clientData = (ClientInfo)ar.AsyncState;
            Hashtable recv = new Hashtable();
            int readLen, psize;
            string readStr;

            try
            {
                #region 데이터 인코딩

                // 총 데이터의 길이를 구함
                readLen = clientData.socket.EndReceive(ar);
                if (readLen == 0) { return false; }
                // 현재까지 온 패킷을 변환 버퍼에 저장
                clientData.PacketBufferAdd(clientData.Buffer, readLen);

                // 모든 버퍼를 제거할때까지 루프
                while (clientData.PacketBuffer != null)
                {
                    // 읽을 길이를 판별
                    try
                    {
                        // 정상적인(약속된) 패킷인지 판정
                        readStr = Encoding.UTF8.GetString(clientData.PacketBuffer, 0, 3);
                        psize = Convert.ToInt32(readStr);
                        if (clientData.PacketBuffer.Length >= psize + 3)
                        {
                            // 정상적인 사이즈의 버퍼가 들어온 경우
                            // 인코딩하여 대입
                            clientData.LastDataString = Encoding.UTF8.GetString(clientData.PacketBuffer, 3, psize);
                            // 최종 변환 데이터를 저장
                            recv = StringConverter.Decode(clientData.LastDataString);
                            // 버퍼 청소
                            clientData.PacketBufferDelete(psize);
                            // 최종 변환된 패킷을 보냄
                            if (!RecvPacket(recv, clientData))
                                return false;
                        }
                        else
                            break;
                    }
                    catch (Exception e)
                    {
                        // 형식에 어긋나는 패킷은 강제 연결 종료
                        Msg.Error(e.ToString());
                        return false;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                // 연결 실패
                //Msg.Error(e.ToString());
                return false;
            }
            return true;
        }

        private static bool RecvPacket(Hashtable recv, ClientInfo clientData)
        {
            #region Custom Variable
            UserData u;
            UserCharacter c;
            DataTable ds;
            string id, pw, mail, question, answer;
            string image, name, text;
            int index, no;

            
            #endregion

            try
            {
                switch (ToInt(recv["part"]))
                {
                    #region Version Part
                    // 버전 체크
                    case VERSION_CHECK:
                        clientData.SendPacket(Version(2));
                        break;
                    #endregion
                    #region Character Part
                    // 유저 이동
                    case CHARACTER_MOVE:

                        if (UserData.Users[clientData].character != null)
                        {

                            UserData.Users[clientData].character.move(ToInt(recv["dir"]));
                        }


                        break;
                    // 유저 키 입력
                    case CHARACTER_KEY_TRIGGER:
                        if (UserData.Users[clientData].character != null)
                        {
                            string key = recv["key"].ToString();
                            switch (key)
                            {
                                case "Z":
                                    UserData.Users[clientData].character.attack();
                                    break;

                            }
                        }
                        clientData.SendPacket(KeyTrigger());
                        break;
                    // 유저 아이템 사용
                    case CHARACTER_ITEM:
                        if (UserData.Users[clientData].character != null)
                        {
                            c = UserData.Users[clientData].character;
                            if (c.hp > 0)
                            {
                                Item obj = null;
                                int type = ToInt(recv["type"]);
                                no = ToInt(recv["no"]);
                                if (type != 0 && NRP_Server.Item.Items.ContainsKey(no))
                                    obj = NRP_Server.Item.Items[no];
                                if (type == 0 && NRP_Server.Item.Equipments.ContainsKey(no))
                                    obj = NRP_Server.Item.Equipments[no];
                                if (obj != null)
                                {
                                    if (type == 0)
                                    {
                                        if (UserData.Users[clientData].character.Inventory.ContainsKey(obj))
                                            UserData.Users[clientData].character.equipItem(obj);
                                        else
                                            UserData.Users[clientData].character.removeItem(obj);
                                    }
                                    else if (type == 1)
                                    {
                                        UserData.Users[clientData].character.useItem(obj);
                                    }
                                }
                            }
                            //귀속
                            else
                            {
                                int makeownitem = 68; //귀속시키는 아이템 ID
                                Item moi = NRP_Server.Item.Items[makeownitem];
                                if (c.Inventory.ContainsKey(moi) == true)
                                {
                                    //아이템 귀속권을 가지고 있을 경우
                                    Item obj = null;
                                    int type = ToInt(recv["type"]);
                                    no = ToInt(recv["no"]);

                                    if (type != 0 && NRP_Server.Item.Items.ContainsKey(no))
                                    {
                                        obj = NRP_Server.Item.Items[no];
                                    }
                                    if (type == 0 && NRP_Server.Item.Equipments.ContainsKey(no))
                                    {
                                        obj = NRP_Server.Item.Equipments[no];
                                    }
                                    else { }
                                    if (c.Inventory.ContainsKey(obj))
                                    {
                                        if (c.ownitems.ContainsKey(obj) == true) {
                                            int numberItem = c.ownitems[obj].number;
                                            c.ownitems[obj] = new UserItem(obj, numberItem + 1);
                                        }
                                        else { c.ownitems[obj] = new UserItem(obj, 1); }
                                        c.loseItem(moi, 1);
                                        c.loseItem(obj, 1);

                                    }
                                    else {

                                    }

                                }
                                else
                                {
                                    //아이템 귀속권을 가지고 있지 않을 경우
                                    clientData.SendPacket(Dialog(0, "귀속권 없음", "아이템 귀속권을 가지고 계시지 않습니다."));
                                }
                            }

                        }
                        break;
                    // 유저 아이템 버림
                    case CHARACTER_TRASH_ITEM:
                        if (UserData.Users[clientData].character != null)
                        {
                            Item item;
                            c = UserData.Users[clientData].character;
                            if (ToInt(recv["type"]) == 0)
                                item = NRP_Server.Item.Equipments[ToInt(recv["no"])];
                            else
                                item = NRP_Server.Item.Items[ToInt(recv["no"])];
                            c.trashItem(item, ToInt(recv["num"]));
                        }
                        break;
                    // 유저 아이템 구매
                    case CHARACTER_STORE_BUY:
                        if (UserData.Users[clientData].character != null)
                        {
                            Store.Buy(UserData.Users[clientData].character, ToInt(recv["store_no"]), ToInt(recv["no"]), ToInt(recv["num"]));
                        }
                        break;

                    // 유저 아이템 판매
                    case CHARACTER_STORE_SELL:
                        if (UserData.Users[clientData].character != null)
                        {
                            Item item;
                            if (ToInt(recv["type"]) == 0)
                                item = NRP_Server.Item.Equipments[ToInt(recv["no"])];
                            else
                                item = NRP_Server.Item.Items[ToInt(recv["no"])];
                            Store.Sell(UserData.Users[clientData].character, item, ToInt(recv["num"]));
                        }
                        break;
                    // 유저 스킬 사용
                    case CHARACTER_SKILL_TRIGGER:
                        if (UserData.Users[clientData].character != null)
                        {
                            Skill skill = Skill.Skills[ToInt(recv["no"])];
                            c = UserData.Users[clientData].character;
                            if (c.Skills.ContainsKey(skill))
                            {
                                c.Skills[skill].active(c);
                            }
                        }
                        break;
                    #endregion
                    #region Netevent Part
                    // 넷이벤트 생성
                    case NETEVENT_CREATE:
                        BroadCast(recv);
                        break;

                    // 넷이벤트 제거
                    case NETEVENT_DELETE:
                        BroadCast(recv);
                        break;

                    // 넷이벤트 애니메이션
                    case NETEVENT_ANIMATE:
                        BroadCast(recv);
                        break;

                    // 넷이벤트 트리거
                    case NETEVENT_TRIGGER:
                        if (UserData.Users[clientData].character != null)
                        {
                            no = ToInt(recv["no"]);
                            if (no < 10000)
                            {
                                index = ToInt(recv["select"]);
                                u = UserData.Users[clientData];
                                if (u.character.action != null)
                                    u.character.page = u.character.action[index];
                                if (u.character.npcData == null)
                                    Map.Maps[u.character.mapid].npcData[no].startEvent(u.character);
                                else
                                    u.character.npcData.startEvent(u.character);
                            }
                            else
                            {
                                no -= 10000;
                                if (UserData.Users[clientData].character.fieldData.DropItems.ContainsKey(no))
                                    UserData.Users[clientData].character.fieldData.gainDropItem(UserData.Users[clientData].character, no);
                            }
                        }
                        break;

                    #endregion
                    #region User Part
                    // 유저 로그인 요청
                    case USER_LOGIN:
                        id = recv["id"].ToString();
                        pw = recv["pw"].ToString();

                        // 생성된 아이디가 있는지 판별
                        ds = Mysql.Query($"SELECT * FROM user_information WHERE id = '{id}' AND pw = '{pw}'");
                        if (ds == null) { break; }
                        if (ds.Rows.Count == 1)
                        {
                            DataRow rs = ds.Rows[0];
                            if (ToInt(rs["online"]) != 1 && !UserData.Users.ContainsKey(clientData))
                            {
                                u = new UserData(clientData);
                                u.loadData(rs);
                                UserData.Users.Add(clientData, u);

                                // 온라인으로 변경
                                Mysql.Query($"UPDATE user_information SET online = '1' WHERE no = '{u.no}'");
                                clientData.SendPacket(Login(u));

                                foreach (Combination ca in Combination.recipes)
                                {
                                    clientData.SendPacket(ItemCombination(ca));
                                    foreach (UserItem ui in ca.elements)
                                    {
                                        clientData.SendPacket(ItemElements(ca, ui));
                                    }
                                    foreach (UserItem ui in ca.results)
                                    {
                                        clientData.SendPacket(ItemResults(ca, ui));
                                    }
                                }
                            }
                            else
                                clientData.SendPacket(Dialog(0, "로그인 실패", "이미 접속중인 계정입니다."));
                        }
                        else
                            clientData.SendPacket(Dialog(0, "로그인 실패", "아이디와 비밀번호를 확인해주세요."));

                        break;

                    // 유저 아이디 체크
                    case USER_IDCHECK:
                        id = recv["id"].ToString();
                        ds = Mysql.Query($"SELECT * FROM user_information WHERE id = '{id}'");
                        if (ds == null) { break; }
                        if (ds.Rows.Count == 0 && !id.Equals(""))
                            clientData.SendPacket(IdCheck(1));
                        else
                            clientData.SendPacket(IdCheck(0));
                        break;

                    // 유저 회원가입
                    case USER_SIGNUP:
                        id = recv["id"].ToString();
                        pw = recv["pw"].ToString();
                        mail = recv["mail"].ToString();
                        question = recv["question"].ToString();
                        answer = recv["answer"].ToString();
                        // 유효성 검사
                        ds = Mysql.Query($"SELECT * FROM user_information WHERE id = '{id}'");
                        if (ds == null) { break; }
                        if (ds.Rows.Count == 0 && !id.Equals(""))
                        {
                            if (Mysql.Query($"INSERT INTO user_information (id, pw, mail, pass_question, pass_answer) VALUES ('{id}','{pw}','{mail}','{question}','{answer}')") != null)
                                clientData.SendPacket(Dialog(0, "회원가입 완료", "가입되었습니다. 새로 로그인해주세요.", "UI.getForm(SignUp).dispose if UI.include?(SignUp);Login.new"));
                            else
                                clientData.SendPacket(Dialog(0, "회원가입 오류", "가입에 실패했습니다. 다시 시도해주세요."));
                        }
                        break;

                    // 유저 닉네임 체크
                    case USER_CREATE_CHARACTER:
                        image = recv["image"].ToString();
                        name = recv["name"].ToString();
                        if (UserData.Users[clientData].characters.Count > 4) { clientData.SendPacket(Dialog(0, "캐릭터 생성 오류", "캐릭터를 더이상 생성할 수 없습니다.")); break; }
                        ds = Mysql.Query($"SELECT * FROM user_character WHERE name = '{name}'");
                        if (ds == null) { clientData.SendPacket(Dialog(0, "캐릭터 생성 오류", "생성 도중 오류가 발생했습니다.")); break; }
                        if (ds.Rows.Count == 0)
                        {
                            if (Mysql.Query($"INSERT INTO user_character (user_no, name, image) VALUES ('{UserData.Users[clientData].no.ToString()}','{name}','{image}')") != null)
                            {
                                UserData.Users[clientData].loadCharacter();
                                clientData.SendPacket(Login(UserData.Users[clientData]));
                                clientData.SendPacket(Dialog(0, "캐릭터 생성 완료", "캐릭터를 정상적으로 생성했습니다.", "UI.getForm(CharacterCreate).dispose if UI.include?(CharacterCreate)"));
                            }
                            else
                                clientData.SendPacket(Dialog(0, "캐릭터 생성 오류", "생성 도중 오류가 발생했습니다."));
                        }
                        else
                            clientData.SendPacket(Dialog(0, "캐릭터 생성 오류", "이미 존재하는 닉네임입니다."));
                        break;

                    // 유저 캐릭터 선택
                    case USER_SELECT_CHARACTER:
                        index = ToInt(recv["index"]);
                        if (UserData.Users[clientData].selectCharacter(index))
                        {
                            clientData.SendPacket(SelectCharacter(UserData.Users[clientData].character));
                        }
                        else
                        {
                            clientData.SendPacket(Login(UserData.Users[clientData]));
                            clientData.SendPacket(Dialog(0, "캐릭터 접속 오류", "접속 도중 오류가 발생했습니다."));
                        }

                        break;

                    // 유저 캐릭터 제거
                    case USER_DELETE_CHARACTER:
                        index = ToInt(recv["index"]);
                        if (UserData.Users[clientData].deleteCharacter(index))
                            clientData.SendPacket(Login(UserData.Users[clientData]));
                        else
                        {
                            clientData.SendPacket(Login(UserData.Users[clientData]));
                            clientData.SendPacket(Dialog(0, "캐릭터 삭제 오류", "삭제 도중 오류가 발생했습니다."));
                        }
                        break;



                    // 유저 채팅
                    case USER_CHAT:
                        name = recv["name"].ToString();
                        text = recv["text"].ToString();
                        if (UserData.Users[clientData].character != null)
                            if (!Chat.Command(clientData, text))
                                UserData.Users[clientData].character.fieldData.AllSendPacket(UserChat(text, name));
                        break;


                    //유저 방 선택
                    case ROOM_CHANGE:
                        int roomno = ToInt(recv["roomno"]);
                        c = UserData.Users[clientData].character;
                        if (UserData.Users[clientData].character != null)
                        {
                            //here
                            Map.Maps[1].newField(roomno);
                            Field a = Map.Maps[1].Fields[roomno];
                            int x, y;
                            while (true)
                            {
                                x = Command.rand.Next(500);
                                y = Command.rand.Next(500);
                                if (a.isportal(a.mapid, x, y))
                                    continue;
                                if (!a.passable(x, y - 1, 8))
                                {
                                    continue;
                                }
                                if (a.passable(x, y, 2) || a.passable(x, y, 4) || a.passable(x, y, 6) || a.passable(x, y, 8))
                                {
                                    break;
                                }

                            }
                            UserData.Users[clientData].character.fieldData.leave(UserData.Users[clientData].character.no);
                            Map.Maps[1].Fields[roomno].join(UserData.Users[clientData].character, x, y);
                            u = null;
                        }
                        break;

                    case FINISH_MAKE_OWN:
                        c = UserData.Users[clientData].character;
                        Dictionary<NRP_Server.Item, UserItem> cinventory = new Dictionary<Item, UserItem>(c.Inventory);
                        foreach (UserItem ui in cinventory.Values)
                        {
                            c.loseItem(ui.itemData, ui.number);
                        }
                        c.hp = c.maxhp;
                        Map.Maps[1].Fields[0].join(c, 11, 9);
                        foreach (UserItem ui in c.ownitems.Values)
                        {
                            c.gainItem(ui.itemData, ui.number);
                        }
                        c.ownitems = new Dictionary<Item, UserItem>();
                        c.userData.clientData.SendPacket(Packet.UserChat("\\C[250,50,50]당신은 죽었습니다!"));
                        break;

                    //씨앗
                    case FarmSEED:
                        string cropname = recv["cropname"].ToString();
                        int num = ToInt(recv["num"]);
                        int seedid = 75;
                        if (UserData.Users[clientData].character != null)
                        {
                            if (cropname == "벼") { seedid = 0; }
                            UserData.Users[clientData].character.seedfarm(UserData.Users[clientData].character, cropname, seedid, num);
                        }
                        break;





                    #endregion

                    #region Own Packet
                    case COMBINATION:
                        string rname = recv["name"].ToString();
                        c = UserData.Users[clientData].character;
                        if (c.combinate(rname) == true) { clientData.SendPacket(Notice(0, 255, 0, "조합 성공!")); }
                        else { clientData.SendPacket(Notice(255, 0, 0, "조합 실패..")); }
                        break;

                    case ITEM_ASK:
                        int typea = ToInt(recv["type"]);
                        int noa = ToInt(recv["no"]);
                        int numa = ToInt(recv["num"]);
                        try {
                            clientData.SendPacket(ItemLoad(new UserItem(NRP_Server.Item.Items[ToInt(recv["no"])], ToInt(recv["num"]))));
                        }
                        catch (Exception e) {
                            clientData.SendPacket(ItemLoad(new UserItem(NRP_Server.Item.Equipments[ToInt(recv["no"])], ToInt(recv["num"]))));
                        }
                        break;

                    case FARM_INFO:
                        clientData.SendPacket(FarmInfoReply(UserData.Users[clientData].character));
                        break;

                    case FARM_SEED:
                        c = UserData.Users[clientData].character;
                        int cropid = -1;
                        foreach (Item i in NRP_Server.Item.Items.Values) {
                            try
                            {
                                if (i.name.Split(' ')[0] == "벼") { cropid = 75; }
                            } catch (Exception e) { continue; }

                        }
                        c.seedfarm(c, recv["cropname"].ToString(), cropid, ToInt(recv["num"]));
                        break;

                    case FARM_WATER:
                        c = UserData.Users[clientData].character;
                        c.water(c);
                        break;

                    case FARM_GET:
                        c = UserData.Users[clientData].character;
                        c.getfarm(c);
                        break;

                    case PARTY_CREATE:
                        c = UserData.Users[clientData].character;
                        if (c.partyno != -1) { clientData.SendPacket(Notice(255, 0, 0, "이미 파티가 있습니다.")); return true; }
                        new Party(c);
                        clientData.SendPacket(Notice(0, 255, 0, "파티 생성 완료!"));
                        break;

                    case PARTY_EXIT:
                        c = UserData.Users[clientData].character;
                        if (c.partyno == -1) { clientData.SendPacket(Notice(255, 0, 0, "들어가 있는 파티가 없습니다.")); return true; }
                        Party.parties[c.partyno].ExitMember(c);
                        clientData.SendPacket(Notice(0, 255, 0, "파티에서 나갔습니다."));
                        break;

                    case PARTY_DESTORY:
                        c = UserData.Users[clientData].character;
                        if (c.partyno == -1) { clientData.SendPacket(Notice(255, 0, 0, "들어가 있는 파티가 없습니다.")); return true; }
                        if (Party.parties[c.partyno].leader != c) { clientData.SendPacket(Notice(255, 0, 0, "당신은 파티장이 아닙니다.")); return true; }
                        Party.parties[c.partyno].RemoveParty(c);
                        clientData.SendPacket(Notice(0, 255, 0, "파티 해산 완료"));
                        break;

                    case PARTY_INVITE:
                        c = UserData.Users[clientData].character;
                        if (c.partyno == -1) { clientData.SendPacket(Notice(255, 0, 0, "들어가 있는 파티가 없습니다.")); return true; }
                        String nickname = recv["no"].ToString();
                        UserCharacter uc = null;
                        foreach (UserData ud in UserData.Users.Values) {
                            if (ud.character.name == nickname) { uc = ud.character; }
                        }
                        if (uc.partyno != -1) { clientData.SendPacket(Notice(255, 0, 0, "그 대상은 이미 들어가있는 파티가 있습니다.")); return true; }
                        //Party.parties[c.partyno].EnterMember(uc);
                        uc.userData.clientData.SendPacket(Dialog(1, "파티 초대", c.name + "님의 파티에 초대되었습니다. 참가하시겠습니까?", "PacketCreater.PartyAccept()"));
                        uc.partyinviteno = c.partyno;
                        break;

                    case PARTY_INFO:
                        c = UserData.Users[clientData].character;
                        if (c.partyno == -1) { clientData.SendPacket(Notice(255, 0, 0, "들어가 있는 파티가 없습니다.")); return true; }
                        String userlist = "";
                        foreach (UserCharacter ucd in Party.parties[c.partyno].member) {
                            if (userlist == "") { userlist = ucd.name; }
                            else { userlist += "," + ucd.name; }
                        }
                        clientData.SendPacket(PartyInfoReply(userlist));
                        break;

                    case PARTY_ACCEPT:
                        c = UserData.Users[clientData].character;
                        if (c.partyno != -1) { clientData.SendPacket(Notice(255, 0, 0, "이미 들어가있는 파티가 있습니다.")); return true; }
                        if (c.partyinviteno == -1) { clientData.SendPacket(Notice(255, 0, 0, "초대 받은 파티가 없습니다.")); return true; }
                        Party.parties[c.partyinviteno].EnterMember(c);
                        clientData.SendPacket(Notice(0, 255, 0, "파티에 들어왔습니다."));
                        foreach (UserCharacter ui in Party.parties[c.partyinviteno].member) { 
                            ui.userData.clientData.SendPacket(Notice(0, 255, 0, c.name+"님께서 파티에 들어왔습니다."));
                        }
                        c.partyinviteno = -1;
                        break;

 
                    case STATUS_STR:
                        c = UserData.Users[clientData].character;
                        if (c.point < ToInt(recv["num"])) { c.userData.clientData.SendPacket(Notice(255, 0, 0, "충분한 스텟 포인트 없습니다.")); return true; }
                        c.UpStr(ToInt(recv["num"]));
                        c.DownPoint(ToInt(recv["num"]));
                        c.ReloadStatus();
                        break;

                    case STATUS_DEX:
                        c = UserData.Users[clientData].character;
                        if (c.point < ToInt(recv["num"])) { c.userData.clientData.SendPacket(Notice(255, 0, 0, "충분한 스텟 포인트 없습니다.")); return true; }
                        c.UpDex(ToInt(recv["num"]));
                        c.DownPoint(ToInt(recv["num"]));
                        c.ReloadStatus();
                        break;

                    case STATUS_INT:
                        c = UserData.Users[clientData].character;
                        if (c.point < ToInt(recv["num"])) { c.userData.clientData.SendPacket(Notice(255, 0, 0, "충분한 스텟 포인트 없습니다.")); return true; }
                        c.UpInt(ToInt(recv["num"]));
                        c.DownPoint(ToInt(recv["num"]));
                        c.ReloadStatus();
                        break;

                    case STATUS_LUK:
                        c = UserData.Users[clientData].character;
                        if (c.point < ToInt(recv["num"])) { c.userData.clientData.SendPacket(Notice(255, 0, 0, "충분한 스텟 포인트 없습니다.")); return true; }
                        c.UpLuk(ToInt(recv["num"]));
                        c.DownPoint(ToInt(recv["num"]));
                        c.ReloadStatus();
                        break;

                    #endregion


                    #region Custom Packet
                    // 커스텀 패킷 (올 리턴)
                    case CUSTOM_PACKET:
                        BroadCast(recv);
                        break;
                        #endregion
                }
                return true;
            }
            catch (Exception e)
            {
                Msg.Error(e.ToString());
                return false;
            }
        }

        //=================================================================
        // * 패킷 전송 간편화
        //=================================================================

        public static Hashtable Version(int ver)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", VERSION_CHECK);
            msg.Add("data", ver);

            return msg;
        }

        public static Hashtable Dialog(int type, string title, string message, string script = "")
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", DIALOG);
            msg.Add("type", type);
            msg.Add("title", title);
            msg.Add("message", message);
            msg.Add("script", script);

            return msg;
        }

        public static Hashtable IdCheck(int var)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", USER_IDCHECK);
            msg.Add("data", var);

            return msg;
        }

        public static Hashtable Login(UserData u)
        {
            int count = 0;
            Hashtable msg = new Hashtable();
            msg.Add("part", USER_LOGIN);
            foreach (UserCharacter c in u.characters.Values)
            {
                count++;
                msg.Add(count.ToString() + "char_name", c.name);
                msg.Add(count.ToString() + "char_level", c.level);
                msg.Add(count.ToString() + "char_job", c.job);
                msg.Add(count.ToString() + "char_image", c.image);
            }
            msg.Add("count", count);
            //msg.Add("name", u.name);
            return msg;
        }

        public static Hashtable Move(UserCharacter c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_MOVE);
            msg.Add("name", c.name);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            msg.Add("mapid", c.fieldData.mapid);
            return msg;
        }

        public static Hashtable SelectCharacter(UserCharacter c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", USER_SELECT_CHARACTER);
            msg.Add("name", c.name);
            msg.Add("map_id", c.mapid);
            msg.Add("map_x", c.x);
            msg.Add("map_y", c.y);
            msg.Add("direction", c.direction);
            msg.Add("image", c.image);
            msg.Add("exp", c.exp);
            msg.Add("gold", c.gold);
            msg.Add("maxexp", Exp.getLevel(c.level));
            msg.Add("level", c.level);
            msg.Add("maxhp", c.maxhp);
            msg.Add("maxmp", c.maxmp);
            msg.Add("hp", c.hp);
            msg.Add("mp", c.mp);
            msg.Add("str", c.str);
            msg.Add("dex", c.dex);
            msg.Add("int", c.Int);
            msg.Add("luk", c.luk);
            msg.Add("point", c.point);
            msg.Add("job", c.job);
            return msg;
        }

        public static Hashtable CharacterCreate(UserCharacter c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_CREATE);
            msg.Add("no", c.no);
            msg.Add("name", c.name);
            msg.Add("str", c.str);
            msg.Add("dex", c.dex);
            msg.Add("int", c.Int);
            msg.Add("luk", c.luk);
            msg.Add("maxhp", c.maxhp);
            msg.Add("hp", c.hp);
            msg.Add("maxmp", c.maxmp);
            msg.Add("mp", c.mp);
            msg.Add("move_speed", c.move_speed);
            msg.Add("level", c.level);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            msg.Add("mapid", c.mapid);

            return msg;
        }

        public static Hashtable DeleteCharacter(UserCharacter c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_DELETE);
            msg.Add("name", c.name);

            return msg;
        }

        public static Hashtable UserChat(string text, string name = "")
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", USER_CHAT);
            msg.Add("text", text);
            msg.Add("name", name);

            return msg;
        }

        public static Hashtable NPCCreate(NPC c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_CREATE);
            msg.Add("type", 0);
            msg.Add("no", c.no);
            msg.Add("name", c.name);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            return msg;
        }

        public static Hashtable NPCMove(NPC c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_MOVE);
            msg.Add("no", c.no);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            return msg;
        }

        public static Hashtable EventTrigger(Message m)
        {
            int count = 0;
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_TRIGGER);
            msg.Add("no", m.npcData.no);
            msg.Add("text", m.text);
            msg.Add("num", m.actions.Length);
            msg.Add("button", (m.button ? 1 : 0));
            if (m.button)
                foreach (int index in m.actions)
                {
                    count++;
                    msg.Add($"select{count}", m.npcData.Actions[index].text);
                }

            return msg;
        }

        public static Hashtable EventTrigger()
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_TRIGGER);
            msg.Add("reset", 1);
            return msg;
        }

        public static Hashtable Item(UserItem item, int equip = 0)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_ITEM);
            msg.Add("no", item.itemData.no);
            msg.Add("name", item.itemData.name);
            msg.Add("type", item.itemData.type);
            msg.Add("equip_type", item.itemData.equip_type);
            msg.Add("icon", item.itemData.icon);
            msg.Add("desc", item.itemData.description);
            msg.Add("price", item.itemData.price);
            msg.Add("str", item.itemData.str);
            msg.Add("dex", item.itemData.dex);
            msg.Add("int", item.itemData.Int);
            msg.Add("luk", item.itemData.luk);
            msg.Add("hp", item.itemData.hp);
            msg.Add("mp", item.itemData.mp);
            msg.Add("solid", item.itemData.solid);
            msg.Add("max_ability", item.itemData.max_ability);
            msg.Add("ability", item.itemData.ability);
            msg.Add("lv_cost", item.itemData.lv_cost);
            msg.Add("rank", item.itemData.rank);
            msg.Add("trade", item.itemData.trade ? 1 : 0);
            msg.Add("sell", item.itemData.sell ? 1 : 0);
            msg.Add("use", item.itemData.use ? 1 : 0);
            msg.Add("number", item.number);
            msg.Add("equip", equip);
            msg.Add("ECollecting", item.itemData.ECollecting);
            msg.Add("EMining", item.itemData.EMining);
            return msg;
        }


        public static Hashtable ItemCombination(Combination c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ITEM_COMBINATION);
            msg.Add("no", c.no);
            msg.Add("name", c.name);
            return msg;
        }

        public static Hashtable ItemElements(Combination c, UserItem ui)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ITEM_ELEMENTS);
            msg.Add("no", c.no);
            msg.Add("elementsNo", ui.itemData.no);
            msg.Add("elementsType", ui.itemData.type);
            msg.Add("elementsNum", ui.number);
            return msg;
        }

        public static Hashtable ItemResults(Combination c, UserItem ui)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ITEM_RESULTS);
            msg.Add("no", c.no);
            msg.Add("resultsNo", ui.itemData.no);
            msg.Add("resultsType", ui.itemData.type);
            msg.Add("resultsNum", ui.number);
            return msg;
        }

        public static Hashtable ItemClear(int option = 0, int type = -1, int no = 0)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_ITEM_CLEAR);
            msg.Add("option", option);
            if (type != -1)
            {
                msg.Add("type", type);
                msg.Add("no", no);
            }

            return msg;
        }

        public static Hashtable EnemyMove(Enemy c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_MOVE);
            msg.Add("no", c.pos_no);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            msg.Add("mapid", c.mapid);

            return msg;
        }

        public static Hashtable EnemyCreate(Enemy c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_CREATE);
            msg.Add("no", c.pos_no);
            msg.Add("name", c.name);
            msg.Add("str", c.str);
            msg.Add("dex", c.dex);
            msg.Add("int", c.Int);
            msg.Add("luk", c.luk);
            msg.Add("maxhp", c.maxhp);
            msg.Add("hp", c.hp);
            msg.Add("maxmp", c.maxmp);
            msg.Add("mp", c.mp);
            msg.Add("move_speed", c.move_speed);
            msg.Add("level", c.level);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            msg.Add("mapid", c.mapid);
            msg.Add("dead", c.IsDead ? 1 : 0);

            return msg;
        }

        public static Hashtable KeyTrigger()
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_KEY_TRIGGER);
            return msg;
        }

        public static Hashtable EnemyDamage(Enemy enemy, string damage, bool critical = false)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_DAMAGE);
            msg.Add("damage", damage);
            msg.Add("no", enemy.pos_no);
            msg.Add("hp", enemy.hp);
            msg.Add("cri", critical ? 1 : 0);
            return msg;
        }

        public static Hashtable EnemyAnimation(Enemy enemy, int id)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_ANIMATION);
            msg.Add("id", id);
            msg.Add("no", enemy.pos_no);
            return msg;
        }

        public static Hashtable EnemyDead(Enemy enemy)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_DEAD);
            msg.Add("no", enemy.pos_no);
            return msg;
        }

        public static Hashtable EnemyRebirth(Enemy enemy)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_REBIRTH);
            msg.Add("no", enemy.pos_no);
            msg.Add("hp", enemy.hp);
            return msg;
        }

        public static Hashtable UserWinEffect()
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", ENEMY_WIN_EFFECT);
            return msg;
        }

        public static Hashtable CharacterDamage(UserCharacter u, string damage, bool critical = false)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_DAMAGE);
            msg.Add("damage", damage);
            msg.Add("name", u.name);
            msg.Add("maxhp", u.maxhp);
            msg.Add("hp", u.hp);
            msg.Add("cri", critical ? 1 : 0);
            return msg;
        }

        public static Hashtable CharacterAnimation(UserCharacter u, int id)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_ANIMATION);
            msg.Add("id", id);
            msg.Add("name", u.name);
            return msg;
        }

        public static Hashtable DropItemCreate(DropItem c)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_CREATE);
            msg.Add("type", 1);
            msg.Add("no", c.no);
            msg.Add("name", c.name);
            msg.Add("x", c.x);
            msg.Add("y", c.y);
            msg.Add("dir", c.direction);
            msg.Add("image", c.image);
            msg.Add("pattern", c.pattern);
            return msg;
        }

        public static Hashtable DropItemDelete(int index)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", NETEVENT_DELETE);
            msg.Add("type", 1);
            msg.Add("no", index);
            return msg;
        }

        public static Hashtable CharacterStatusUpdate(UserCharacter u)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_STATUS);
            msg.Add("exp", u.exp);
            msg.Add("gold", u.gold);
            msg.Add("maxexp", Exp.getLevel(u.level));
            msg.Add("level", u.level);
            msg.Add("maxhp", u.maxhp);
            msg.Add("maxmp", u.maxmp);
            msg.Add("hp", u.hp);
            msg.Add("mp", u.mp);
            msg.Add("str", u.str);
            msg.Add("dex", u.dex);
            msg.Add("int", u.Int);
            msg.Add("luk", u.luk);
            msg.Add("point", u.point);
            msg.Add("job", u.job);
            return msg;
        }

        public static Hashtable StoreItem(Store storeData, StoreItem store_item)
        {
            Hashtable msg = new Hashtable();
            Item item = NRP_Server.Item.Items[store_item.item_no];
            msg.Add("part", CHARACTER_STORE_LOAD);
            msg.Add("store_no", storeData.no);
            msg.Add("store_name", storeData.name);
            msg.Add("no", store_item.no);
            msg.Add("name", item.name);
            msg.Add("type", item.type);
            msg.Add("equip_type", item.equip_type);
            msg.Add("icon", item.icon);
            msg.Add("desc", item.description);
            msg.Add("price", store_item.price);
            msg.Add("str", item.str);
            msg.Add("dex", item.dex);
            msg.Add("int", item.Int);
            msg.Add("luk", item.luk);
            msg.Add("hp", item.hp);
            msg.Add("mp", item.mp);
            msg.Add("solid", item.solid);
            msg.Add("max_ability", item.max_ability);
            msg.Add("ability", item.ability);
            msg.Add("lv_cost", item.lv_cost);
            msg.Add("rank", item.rank);
            msg.Add("trade", item.trade ? 1 : 0);
            msg.Add("sell", item.sell ? 1 : 0);
            msg.Add("use", item.use ? 1 : 0);
            msg.Add("number", store_item.number);
            msg.Add("ECollecting", item.ECollecting);
            msg.Add("EMining", item.EMining);
            return msg;
        }

        public static Hashtable LoadSkill(UserSkill skill)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_SKILL_LOAD);
            msg.Add("no", skill.skillData.no);
            msg.Add("name", skill.skillData.name);
            msg.Add("desc", skill.skillData.description);
            msg.Add("icon", skill.skillData.icon);
            msg.Add("lv", skill.level);
            msg.Add("max_lv", skill.skillData.max_level);
            msg.Add("power", skill.skillData.power);
            msg.Add("power_factor", skill.skillData.power_factor);
            msg.Add("lv_power", skill.skillData.level_power);
            msg.Add("cost", skill.skillData.cost);
            msg.Add("range_type", skill.skillData.range_type);
            msg.Add("range", skill.skillData.range);
            msg.Add("count", skill.skillData.count);
            msg.Add("delay", skill.skillData.delay);
            msg.Add("wait_time", skill.skillData.wait_time);
            return msg;
        }

        public static Hashtable SkillTime(UserSkill skill)
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", CHARACTER_SKILL_TRIGGER);
            msg.Add("no", skill.skillData.no);
            msg.Add("wait_time", skill.skillData.wait_time * 6);
            return msg;
        }

        public static Hashtable FarmUIOpen()
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", FarmUI_OPEN);
            return msg;
        }

        public static Hashtable SetMakeOwn()
        {
            Hashtable msg = new Hashtable();
            msg.Add("part", SET_MAKE_OWN);
            return msg;
        }

        public static Hashtable ItemLoad(UserItem item, int equip = 0) {
            Hashtable msg = new Hashtable();
            msg.Add("part", ITEM_LOAD);
            msg.Add("no", item.itemData.no);
            msg.Add("name", item.itemData.name);
            msg.Add("type", item.itemData.type);
            msg.Add("equip_type", item.itemData.equip_type);
            msg.Add("icon", item.itemData.icon);
            msg.Add("desc", item.itemData.description);
            msg.Add("price", item.itemData.price);
            msg.Add("str", item.itemData.str);
            msg.Add("dex", item.itemData.dex);
            msg.Add("int", item.itemData.Int);
            msg.Add("luk", item.itemData.luk);
            msg.Add("hp", item.itemData.hp);
            msg.Add("mp", item.itemData.mp);
            msg.Add("solid", item.itemData.solid);
            msg.Add("max_ability", item.itemData.max_ability);
            msg.Add("ability", item.itemData.ability);
            msg.Add("lv_cost", item.itemData.lv_cost);
            msg.Add("rank", item.itemData.rank);
            msg.Add("trade", item.itemData.trade ? 1 : 0);
            msg.Add("sell", item.itemData.sell ? 1 : 0);
            msg.Add("use", item.itemData.use ? 1 : 0);
            msg.Add("number", item.number);
            msg.Add("equip", equip);
            msg.Add("ECollecting", item.itemData.ECollecting);
            msg.Add("EMining", item.itemData.EMining);
            return msg;
        }

        public static Hashtable FarmInfoReply(UserCharacter u) {
            Hashtable msg = new Hashtable();
            msg.Add("part", FARM_INFO_REPLY);
            msg.Add("uname", u.name);
            msg.Add("farmname", u.Infofarm("farmname"));
            msg.Add("farmnum", u.Infofarm("farmnum"));
            msg.Add("waternum", u.Infofarm("waternum"));
            return msg;
        }

        public static Hashtable Notice(int color1, int color2, int color3, String text, int fontsize = 28) {
            Hashtable msg = new Hashtable();
            msg.Add("part", NOTICE);
            msg.Add("color1", color1);
            msg.Add("color2", color2);
            msg.Add("color3", color3);
            msg.Add("text", text);
            msg.Add("fontsize", fontsize);
            return msg;
        }

        public static Hashtable PartyInfoReply(String userlist) {
            Hashtable msg = new Hashtable();
            msg.Add("part", PARTY_INFO_REPLY);
            msg.Add("userlist", userlist);
            return msg;
        }
    }
}
