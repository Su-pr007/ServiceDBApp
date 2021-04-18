using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Service
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection(string login, string password)
        {
            string host = Variables.DBConnSettings.IP;
            int port = Convert.ToInt32(Variables.DBConnSettings.Port);
            string database = Variables.DBConnSettings.Name;

            return DBMySQLUtils.GetDBConnection(host, port, database, login, password);
        }

    }
}
