using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WowLatinos.Models;
using Microsoft.AspNetCore.Http;
using WowLatinos.Models.BD;

namespace WowLatinos.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyName = "_id";
        const string SessionKeyUser = "_username";

        public IActionResult Index()
        {
            ViewBag.PostList = new Post().Select();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult HowToPlay()
        {
            ViewData["Message"] = "Your contact page.";


            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
