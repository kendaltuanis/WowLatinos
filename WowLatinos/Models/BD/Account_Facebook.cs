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
    public class AccountFacebook
    {
        public string id_facebook { get; set; }
        public string id_account { get; set; }
        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "auth.account_facebook";

        public AccountFacebook()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

         public AccountFacebook(string id_facebook)
        {
            this.id_facebook = id_facebook;
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }
           

        public AccountFacebook(string id_facebook, string id_account)
        {
            this.id_facebook = id_facebook;
            this.id_account = id_account;

            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public void Add() {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Add("id_facebook", id_facebook);
            data.Add("id_account", id_account);

            Startup.connection.SqlStatement(sql.InsertSql(data.Select(i=> i.Key).ToArray()),data);

            if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
            }
            
        }

        public int SelectAccount() {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_facebook", id_facebook);
        
            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "id_account" }, data.Select(i => i.Key).ToArray()), data);


            foreach (KeyValuePair<string, string> pair in account)
            {
                return Convert.ToInt32(pair.Value);
            }

            return 0;
        }


    }
}
