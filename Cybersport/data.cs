using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybersport
{
    internal class data
    {
        static public string host = Properties.Settings.Default.host;
        static public string user = Properties.Settings.Default.user;
        static public string db = Properties.Settings.Default.db;
        static public string pwd = Properties.Settings.Default.pwd;
        static public string conStr = $@"host=localhost;uid=root;pwd=;database=cybersport;";
        static public string conStr2 = $@"host={host};uid={user};pwd={pwd};";
        static public string role;
        static public string usrName;
        static public string Login;
    }
}
