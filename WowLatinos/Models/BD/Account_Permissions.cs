using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpersSQL;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace WowLatinos.Models.BD
{
    public class Account_Permissions
    {
        public int id_account { get; set; }
        public int lvl { get; set; }

        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "auth.account_permissions";

        public Account_Permissions()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public Account_Permissions(int id_account)
        {
            this.id_account = id_account;
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public Account_Permissions(int id_account, int lvl)
        {
            this.id_account = id_account;
            this.lvl = lvl;
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public void Add()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);

            if (lvl == 0)
            {

                Startup.connection.SqlStatement(sql.InsertSql(data.Select(i => i.Key).ToArray()), data);

                if (Startup.connection.isError)
                {
                    string ss = Startup.connection.errorDescription;
                    Startup.connection.RollbackTransaction();
                }
            }




        }

        public int Select()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);

            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "lvl" }, data.Select(i => i.Key).ToArray()), data);

            foreach (KeyValuePair<string, string> pair in account)
            {
                return Convert.ToInt32(pair.Value);
            }

            return 0;
        }

        // No funciona
        public void Edit()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);

            Startup.connection.SqlQuery(sql.UpdateSql(new string[] { "username,pass,email" }, data.Select(i => i.Key).ToArray()), data);

            if (Startup.connection.isError)
            {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
            }


        }

        public void Delete()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);
            Startup.connection.SqlStatement(sql.DeleteSql(data.Select(i => i.Key).ToArray()), data);

            if (Startup.connection.isError)
            {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
            }
        }


    }
}
