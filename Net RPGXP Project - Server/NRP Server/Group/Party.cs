using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.Group
{
    class Party
    {
        public static Dictionary<int, Party> parties = new Dictionary<int, Party>();
        

        public int no { get; set; }
        public UserCharacter leader { get; set; }
        public List<UserCharacter> member { get; set; }

        public int maxnum { get; set; } = 7;

        public Party(UserCharacter u) {
            this.no = parties.Count();
            u.partyno = this.no;
            this.leader = u;
            this.member = new List<UserCharacter>();
            this.member.Add(u);
            parties[this.no] = this;
        }

        public void EnterMember(UserCharacter u) {
            this.member.Add(u);
            u.partyno = this.no;
        }

        public void ExitMember(UserCharacter u) {
            if (this.leader == u)
            {
                if (this.member.Count() <= 1)
                {
                    //자신 말고 다른 파티원이 없을 때
                    parties.Remove(this.no);
                    u.partyno = -1;
                }
                else
                {
                    //자신 말고 다른 파티원이 있어서, 방장을 넘겨줄 때
                    UserCharacter ui;
                    while (true)
                    {
                        Random r = new Random();
                        int rnum = r.Next(0, this.member.Count() - 1);
                        if (this.leader == this.member[rnum]) { continue; }
                        else { ui = this.member[rnum]; break; }
                    }
                    this.leader = ui;
                    this.member.Remove(u);
                    u.partyno = -1;
                }
            }
            //파티장이 아닐 때
            else { this.member.Remove(u); u.partyno = -1; }
        }

        public bool RemoveParty(UserCharacter u) {
            if (u == this.leader)
            {
                foreach (UserCharacter ui in this.member) {
                    u.partyno = -1;
                }
                parties.Remove(this.no);
                return true;
            }
            else {
                return false;
            }
        }


    }
}
