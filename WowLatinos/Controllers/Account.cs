﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WowLatinos.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using WowLatinos.Models.BD;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace WowLatinos.Controllers
{
    public class Account : Controller
    {

        private const string SessionKeyId = "_id";
        private const string SessionKeyUser = "_username";
        private const string SessionKeyPassChange = "_changeP";
        private const string SessionKeyUserChange = "_changeU";
        private const string SessionKeyEmailChange = "_changeE";
        private const string SessionKeyPermissions = "_permissions";

        public string CountryCode { get; set; }
        public SelectList CountryList { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.CountryList = Countries();
            ViewBag.FactionList = Factions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(WowLatinos.Models.BD.Account acc)
        {
            int id = acc.Add();
            if (id != 0)
            {
                HttpContext.Session.SetInt32(SessionKeyId, id);
                HttpContext.Session.SetString(SessionKeyUser, acc.user);
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Login(string user, string pass)
        {

            WowLatinos.Models.BD.Account acc = new Models.BD.Account(user, pass);

            int idA = acc.ExistsAccount();
            HttpContext.Session.SetInt32(SessionKeyId, idA);
            HttpContext.Session.SetString(SessionKeyUser, user);

            IsPermissions(idA);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyId);
            HttpContext.Session.Remove(SessionKeyUser);
            HttpContext.Session.Remove(SessionKeyPassChange);
            HttpContext.Session.Remove(SessionKeyUserChange);
            HttpContext.Session.Remove(SessionKeyPermissions);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            WowLatinos.Models.BD.Account acc = new WowLatinos.Models.BD.Account();
            WowLatinos.Models.BD.Characters pj = new WowLatinos.Models.BD.Characters();
            int? i = HttpContext.Session.GetInt32("_id");
            ViewBag.Main = acc.SelectAccountData(i);
            ViewBag.Details = acc.SelectPersonalData(i);
            ViewBag.Characters= pj.Pjs(i);
             ViewBag.CharactersMain =null;

            return View();
        }

        [HttpPost]
        public IActionResult CharactersMainData(int id)
        {
            WowLatinos.Models.BD.Characters pj = new WowLatinos.Models.BD.Characters();

            return Json(pj.MainData(id));
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult FirstTime(string id, string first_name, string last_name, string email)
        {
            AccountFacebook accF = new AccountFacebook(id);
            int id_u = accF.SelectAccount();

            if (id_u == 0)
            {
                string[] sd = email.Split('@');
                WowLatinos.Models.BD.Account acc = new WowLatinos.Models.BD.Account(string.Format("{0}{1}", sd[0], id.Substring(1, 3)), string.Format("{0}{1}", first_name.Substring(0, 2), id.Substring(3, 8)), email, first_name, last_name);
                id_u = acc.Add();
                accF = new AccountFacebook(id, id_u.ToString());

                accF.Add();
                new Account_Is_Change(id_u, true, true).Add();

                HttpContext.Session.SetInt32(SessionKeyPassChange, 1);
                HttpContext.Session.SetInt32(SessionKeyUserChange, 1);
            }

            HttpContext.Session.SetInt32(SessionKeyId, id_u);
            HttpContext.Session.SetString(SessionKeyUser, first_name);
            IsChanges(id_u);

            IsPermissions(id_u);


            return RedirectToAction("Index", "Home");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<string> Countries()
        {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo("en-US");
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }

            CountryList.Sort();
            return CountryList;
        }

        private List<string> Factions()
        {
            List<string> faction = new List<string>();
            faction.Add("Sin favorita xd");
            faction.Add("Alianza");
            faction.Add("Horda");
            faction.Sort();

            return faction;
        }

        private void IsChanges(int id_u)
        {
            List<bool> list = new Account_Is_Change(id_u).Select();
            if (list[0])
            {
                HttpContext.Session.SetInt32(SessionKeyUserChange, 1);
            }
            if (list[1])
            {
                HttpContext.Session.SetInt32(SessionKeyPassChange, 1);
            }
            if (list[2])
            {

            }
        }

        private void IsPermissions(int id_u)
        {
            Account_Permissions p = new Account_Permissions(id_u);
            int permissions = p.Select();

            if (permissions != 0)
            {
                HttpContext.Session.SetInt32(SessionKeyPermissions, permissions);
            }
        }

    }
}

