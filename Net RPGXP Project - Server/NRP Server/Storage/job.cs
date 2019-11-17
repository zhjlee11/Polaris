using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.Storage
{
    class Job
    {
        public static Dictionary<int, Job> JobList = new Dictionary<int, Job>();

        public int no { get; private set; }
        public string name { get; private set; }
        public string info { get; private set; }
        public Dictionary<int, int> JobSkillList { get; private set; } = new Dictionary<int, int>();  //레벨, Skill ID

        public Job(DataRow rs)
        {
            no = Convert.ToInt32(rs["no"]);
            name = rs["name"].ToString();
            info = rs["info"].ToString();
        }

        public void AddSkill(DataRow ps)
        {
            JobSkillList[Convert.ToInt32(ps["level"])] = Convert.ToInt32(ps["skillno"]);
        }

        public static void loadData()
        {
            int count = 0;
            Job obj = null;
            Dictionary<int, Job> Jobs = new Dictionary<int, Job>();
            DataTable pos = Mysql.Query($"SELECT * FROM storage_job");
            DataTable ds;
            foreach (DataRow ps in pos.Rows)
            {
                JobList[Convert.ToInt32(ps["no"])] = new Job(ps);
                ds = Mysql.Query($"SELECT * FROM storage_job_skill WHERE jobno = '{ps["no"]}'");
                foreach (DataRow ts in ds.Rows)
                {
                    JobList[Convert.ToInt32(ps["no"])].AddSkill(ts);
                }
                count += 1;
            }
            Msg.Info($"[직업] {count} 개의 직업 로드 완료");
        }
    }
}