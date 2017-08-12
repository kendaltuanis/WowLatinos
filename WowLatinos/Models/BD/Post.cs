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
    public class Post
    {
        public int id_user { get; set; }
        public int id_post { get; set; }


        SqlHelper sql;
        private const string TABLE_NAME = "blog.facebook_post";

        const string SessionKeyName = "_id";

        public Post()
        {
            sql = new SqlHelper(TABLE_NAME);
        }
        

        public Post(int id_post, int id_user)
        {
            this.id_post = id_post;
            this.id_user = id_user;

            sql = new SqlHelper(TABLE_NAME);
        }

        public void Add() {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("id_post", id_post);

            Startup.connection.BeginTransaction();
          
            Startup.connection.SqlStatement(sql.InsertSql(data.Select(i=> i.Key).ToArray()),data);

            if (Startup.connection.isError) {
                string ss = Startup.connection.errorDescription;
                Startup.connection.RollbackTransaction();
            }

            Startup.connection.CommitTransaction();

        }

        public List<List<string>> Select(int limit=5,int? offset=0) {

            Dictionary<string, object> dataWhere = new Dictionary<string, object>();
            dataWhere.Add("show_post", 1);
           
            List<string> p = Startup.connection.SqlQueryList(string.Format("{0} LIMIT {1} OFFSET {2}",sql.SelectSql(new string[] { "id_post, publish_date" }, dataWhere.Select(i => i.Key).ToArray()),limit,offset), dataWhere);
            List<string> ids = new List<string>();
            List<string> dates = new List<string>();
            for (int i = 0; i < p.Count; i++)
            {
                if ((i % 2) == 0)
                {
                    ids.Add(p[i]);
                }
                else {
                    string[] split =p[i].Split(' ');
                    dates.Add(split[0].Replace('/','-'));
                }
            }
            List<List<string>> pru = new List<List<string>>();
            pru.Add(ids);
            pru.Add(dates);
            return pru;
        }



    }
}
