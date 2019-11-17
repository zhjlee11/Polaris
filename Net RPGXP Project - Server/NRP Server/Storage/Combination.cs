using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.Storage
{
    class Combination
    {
        public static List<Combination> recipes = new List<Combination>();

        public int no { get; private set; }
        public String name { get; private set; }
        public List<UserItem> results { get; private set; } = new List<UserItem>(); //조합 완료
        public List<UserItem> elements { get; private set; } = new List<UserItem>(); //조합 재료

        public Combination(int num, String n) {
            no = num;
            name = n;
        }

        private static int ToInt(object data)
        {
            return Convert.ToInt32(data);
        }

        public static void loadrecipe()
        {
            int count = 0;
            Combination c;
            try
            {
                c = new Combination(0, "정기1");
                c.elements.Add(new UserItem(Item.Items[58], 2));
                c.elements.Add(new UserItem(Item.Items[67], 1));
                c.results.Add(new UserItem(Item.Items[68], 2));
                recipes.Add(c);

                c = new Combination(1, "정기2");
                c.elements.Add(new UserItem(Item.Items[68], 1));
                c.results.Add(new UserItem(Item.Items[67], 2));
                recipes.Add(c);

                c = new Combination(2, "정기3");
                c.elements.Add(new UserItem(Item.Items[67], 1));
                c.results.Add(new UserItem(Item.Items[68], 1));
                recipes.Add(c);

            }
            catch (MySqlException e)
            {
                Msg.Error(e.ToString());
            }
            Msg.Info("[조합법] " + recipes.Count().ToString() + "개 로드 완료.");
        }

        /*public static void loadrecipe() {
            int count = 0;
            try
            {
                Combination obj;
                DataTable ds = Mysql.Query("SELECT * FROM storage_recipe");

                foreach (DataRow rs in ds.Rows)
                {
                    List<UserItem> eles = new List<UserItem>();
                    List<UserItem> ress = new List<UserItem>();
                    obj = new Combination(ToInt(rs["no"]), rs["name"].ToString());
                    String[] element1 = rs["element"].ToString().Split(',');
                    foreach (String el in element1) {
                        eles.Add(new UserItem(Item.Items[ToInt(el.Split(':')[0])], ToInt(el.Split(':')[1])));
                    }
                    String[] result1 = rs["rsult"].ToString().Split(',');
                    foreach (String rl in result1)
                    {
                        ress.Add(new UserItem(Item.Items[ToInt(rl.Split(':')[0])], ToInt(rl.Split(':')[1])));
                    }
                    obj.elements = eles;
                    obj.results = ress;
                    count++;

                    recipes.Add(obj);
                }
            }
            catch (MySqlException e)
            {
                Msg.Error(e.ToString());
            }
            Msg.Info("[조합법] " + count.ToString() + "개 로드 완료.");
        }*/




    }
}
