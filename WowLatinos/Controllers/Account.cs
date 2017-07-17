using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WowLatinos.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

namespace WowLatinos.Controllers
{
    public class Account : Controller
    {
        
        private const string SessionKeyId = "_id";
        private const string SessionKeyUser = "_username";

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
            if (id != 0) {
                HttpContext.Session.SetInt32(SessionKeyId,id);
                HttpContext.Session.SetString(SessionKeyUser, acc.user);
            }
           
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Login(string user, string pass) {

            WowLatinos.Models.BD.Account acc = new Models.BD.Account(user, pass);

            HttpContext.Session.SetInt32(SessionKeyId, acc.ExistsAccount());
            HttpContext.Session.SetString(SessionKeyUser, user);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyId); 
            HttpContext.Session.Remove(SessionKeyUser);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("_id") == null) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<string> Countries() {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
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

    }
}

