using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server
{
    class Config
    {
        // Server
        public static string TITLE       = "Net RMXP Prject Server - by. Allfea (allfea_@naver.com)";
        // Socket
        public static int    PORT        = 50002;
        public static int    MAX_NUM     = 100;
        // Mysql
        public static string SERVER_IP   = "127.0.0.1";
        public static string HOST_NAME   = "localhost";
        public static int    SQLPORT     = 3306;
        public static string USER_NAME   = "root";
        public static string PASSWORD    = "apmsetup";
        public static string DATABASE    = "net_rmxp_project";
        // Thread
        public static int    WAIT_TIME   = 100;
        // Costom Variable
        public static string line = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";
        // Console Message
        public static string MAIN_MESSAGE = line + "\n\n\tNet RMXP Prject Server (2018.06.12)\n\n" + line;
        public static string SERVER_MESSAGE = "서버를 시작합니다.";
        // Game Version
        public static string VERSION     = "1.00";
    }
}
