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

        public PartialViewResult _LoadMore()
        {
            int offset=5;
            int? count=5;
            int? i =0;
             if (HttpContext.Session.GetInt32("_load") != null)
            {
                i=HttpContext.Session.GetInt32("_load");
               
            }
             if (HttpContext.Session.GetInt32("_count") != null)
            {
                count=HttpContext.Session.GetInt32("_count");
               
            }
             i+=offset;
             List<List<string>> list = new Post().Select(5,i);
            ViewBag.PostList = list;
            ViewBag.LastCount=count;
            HttpContext.Session.SetInt32("_load",Convert.ToInt32(i));
            HttpContext.Session.SetInt32("_count",list[0].Count);
             return PartialView();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
