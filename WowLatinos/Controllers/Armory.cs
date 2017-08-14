using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WowLatinos.Models;
using Microsoft.AspNetCore.Http;

namespace WowLatinos.Controllers
{
    public class Armory : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Addons()
        {
            return View();
        }




        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
