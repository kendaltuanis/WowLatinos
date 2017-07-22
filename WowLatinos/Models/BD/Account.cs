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
    public class Account
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public DateTime birth { get; set; }
        public string faction { get; set; }


        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "account";
        private const string TABLE_DETAILS_NAME = "account_details";

        const string SessionKeyName = "_id";

        public Account()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }
        

        public Account(string user, string pass)
        {
            this.user = user;
            this.pass = pass;

            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public int Add() {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Add("username", user);
            data.Add("sha_pass_hash", GetHashData());
            data.Add("email", email);

            Startup.connection.BeginTransaction();
            Int32 s = Convert.ToInt32(Startup.connection.SqlScalar(sql.InsertSql(data.Select(i => i.Key).ToArray()), data));

            data = new Dictionary<string, object>();
            data.Add("id_account", s);
            data.Add("first_name", first_name);
            data.Add("last_name", last_name);

            sql = new SqlHelper(TABLE_DETAILS_NAME);
            Startup.connection.SqlStatement(sql.InsertSql(data.Select(i=> i.Key).ToArray()),data);

            if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
                return 0;
            }
            Startup.connection.CommitTransaction();

            return s;
        }

        public int ExistsAccount() {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", user);
            data.Add("sha_pass_hash", GetHashData());

            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "id" }, data.Select(i => i.Key).ToArray()), data);


            foreach (KeyValuePair<string, string> pair in account)
            {
                return Convert.ToInt32(pair.Value);
            }

            return 0;
        }

        private string GetHashData() {
            byte[] bytes = Encoding.UTF8.GetBytes(pass);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        private int Faction() {
            switch (faction) {
                case "Alianza":
                    return 1;
                case "Horda":
                    return 2;
            }

            return 0;              
        }

    }
}
