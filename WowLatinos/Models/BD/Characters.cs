﻿using HelpersSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace WowLatinos.Models.BD
{
    public class Characters
    {
        //Datos escenciales
        SqlHelper sql;
        private const string TABLE_ACCOUNT_NAME = "characters.characters";
        private const string TABLE_CHARACTER_MAIN = "characters.characters_main";

        private SelectList Pj { get; set; }
        private string idPj { get; set; }

        public Characters()
        {
            sql = new SqlHelper(TABLE_ACCOUNT_NAME);
        }

        public List<SelectListItem> Pjs(int? id_account)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("account", id_account);
            List<string> list = Startup.connection.SqlQueryList(sql.SelectSql(new string[] { "guid,name" }, data.Select(i => i.Key).ToArray()), data);


            List<SelectListItem> items = new List<SelectListItem>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    items.Add(new SelectListItem { Text = list[(i + 1)], Value = list[i] });
                }
            }

            return items;
        }

        public List<string> MainData(int guid)
        {
            List<string> list = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("guid", guid);
            sql = new SqlHelper(TABLE_CHARACTER_MAIN);

            List<string> acc = Startup.connection.SqlQueryList(sql.SelectSql(new string[] { "race,class,gender,level,money,online,health" }, data.Select(i => i.Key).ToArray()), data);


            list.Add(urlImageRaceGender(Convert.ToInt16(acc[0]), Convert.ToInt16(acc[2])));
            list.Add(urlClass(Convert.ToInt16(acc[1])));

            for (int i = 3; i < acc.Count; i++)
            {
                list.Add(acc[i]);
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

        private string urlImageRaceGender(int race, int gender)
        {
            StringBuilder url = new StringBuilder("http://wow.zamimg.com/images/wow/icons/large/race_");
            switch (race)
            {
                case 1:
                    url.Append("human");
                    break;
                case 2:
                    url.Append("orc");
                    break;
                case 3:
                    url.Append("dwarf");
                    break;
                case 4:
                    url.Append("nightelf");
                    break;
                case 5:
                    url.Append("scourge");
                    break;
                case 6:
                    url.Append("tauren");
                    break;
                case 7:
                    url.Append("gnome");
                    break;
                case 8:
                    url.Append("troll");
                    break;
                case 10:
                    url.Append("bloodelf");
                    break;
                case 11:
                    url.Append("draenei");
                    break;

            }

            switch (gender)
            {

                case 0:
                    url.Append("_male");
                    break;
                case 1:
                    url.Append("_female");
                    break;
            }

            return url.Append(".jpg").ToString();
        }

        private string urlClass(int clas)
        {
            StringBuilder url = new StringBuilder("http://wow.zamimg.com/images/wow/icons/large/class_");

            switch (clas)
            {
                case 1:
                    url.Append("warrior");
                    break;
                case 2:
                    url.Append("paladin");
                    break;
                case 3:
                    url.Append("hunter");
                    break;
                case 5:
                    url.Append("priest");
                    break;
                case 6:
                    url.Append("deathknight");
                    break;
                case 7:
                    url.Append("shaman");
                    break;
                case 8:
                    url.Append("mage");
                    break;
                case 9:
                    url.Append("warlock");
                    break;
                case 11:
                    url.Append("druid");
                    break;

            }


            return url.Append(".jpg").ToString();
        }


    }
}
