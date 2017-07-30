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
    public class Account_Is_Change
    {
        public int id_account { get; set; }
        public Boolean username { get; set; }
        public Boolean pass { get; set; }
        public Boolean email { get; set; }

        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "auth.account_is_changes";
  
        public Account_Is_Change()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

         public Account_Is_Change(int id_account)
        {
            this.id_account = id_account;
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public Account_Is_Change(int id_account,Boolean user=false, Boolean pass=false, Boolean email=false)
        {
            this.id_account = id_account;
            this.username=user;
            this.pass=pass;
            this.email=email;

            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public void Add() {
            Dictionary<string, object> data = new Dictionary<string, object>();
             data.Add("id_account", id_account);

            if(username){
                data.Add("username", username);
            }
            if(pass){
                data.Add("pass", pass);
            }
            if(email){
                data.Add("email", email);
            }
          

            Startup.connection.SqlStatement(sql.InsertSql(data.Select(i=> i.Key).ToArray()),data);

            if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
            }
            
        }

        public List<Boolean> Select() {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);

            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "username,pass,email" }, data.Select(i => i.Key).ToArray()), data);
            List<Boolean> list = new List<Boolean>();

            foreach (KeyValuePair<string, string> pair in account)
            {
                list.Add(Convert.ToBoolean(pair.Value));
            }

            return list;
        }

        public void Edit(){
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);

            Startup.connection.SqlQuery(sql.UpdateSql(new string[] { "username,pass,email" }, data.Select(i => i.Key).ToArray()), data);
           
           if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
           }


        }

        public void Delete(){
         Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_account", id_account);
            Startup.connection.SqlStatement(sql.DeleteSql(data.Select(i=> i.Key).ToArray()),data);
            
              if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
                }
        }


    }
}
