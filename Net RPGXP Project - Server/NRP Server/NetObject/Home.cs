using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.NetObject
{
    class Home
    {
        //필드
        public Field fieldData;
        public int id { get; private set; }
        public int Builderid { get; private set; }

        //{아이템 구분 번호,아이템 개수}
        //But, 장비 아이템의 item.no는 storage_item이 아닌 storage_equipment에 있는 id값이다.
        //Store.cs 의 Sell 함수 확인
        public Dictionary<int, int> storage { get; private set; } = new Dictionary<int, int>();

        //생성자
        public Home(Field _fieldData, UserCharacter user)
        {
            this.fieldData = _fieldData;
            this.id = fieldData.Homes.Count() + 1;
            this.Builderid = user.userData.no;
            //this.Enemyid = Home pattern을 가지고 있는 몬스터의 no
        }
    }
}
