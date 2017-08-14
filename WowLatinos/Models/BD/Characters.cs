using HelpersSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowLatinos.Models.BD
{
    public class Characters
    {
        //Datos escenciales
        public string name { get; set; }
        public int race { get; set; }
        public int clas { get; set; }
        public int gender { get; set; }
        public int lvl { get; set; }
        public int money { get; set; }
        public int online { get; set; }

        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "characters.characters";

        public Characters()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public List<string> Pjs(int? id_account) {
            List<string> list = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("account", id_account);

            return Startup.connection.SqlQueryList(sql.SelectSql(new string[] { "name" }, data.Select(i => i.Key).ToArray()), data);
        }

        public List<string> MainData(string name) {
            List<string> list = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", name);

            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "race,class,gender,lvl,money,online,health" }, data.Select(i => i.Key).ToArray()), data);

            foreach (KeyValuePair<string, string> pair in account)
            {
                list.Add(pair.Value);
            }

            return list;
        }

        public List<string> PvpData(string name)
        {
            List<string> list = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", name);

            var account = Startup.connection.SqlQuery(sql.SelectSql(new string[] { "arenaPoints,totalHonorPoints,totalKills" }, data.Select(i => i.Key).ToArray()), data);

            foreach (KeyValuePair<string, string> pair in account)
            {
                list.Add(pair.Value);
            }

            return list;
        }


    }
}
