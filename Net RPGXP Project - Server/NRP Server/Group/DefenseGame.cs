using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.Group
{
    class DefenseGame
    {

        public static Dictionary<int, DefenseGame> games = new Dictionary<int, DefenseGame>();

        public int no { get; set; }
        public UserCharacter member { get; set; }

        public int GameType { get; set; }
        /*
         * [게임 타입]
         * 
         * 0. 일반 도서관 Defense 게임
         * 
         * */

        public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

        public DefenseGame(UserCharacter u, int type=0) {
            no = games.Count();
            member = u;
            GameType = type;
            games[no] = this;
        }

        public void loadenemies() {
            if (GameType == 0) {

            }
        }



    }
}
