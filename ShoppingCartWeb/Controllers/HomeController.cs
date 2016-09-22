using ShoppingCartWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["API_URL"] = Common.API_URL;
            ViewData["Token"] = Common.Token;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}