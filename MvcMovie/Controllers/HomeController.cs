using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Machine Shop System ver. beta-0.1 | 2015";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact programmer if you have any questions or ideas.";

            return View();
        }
    }
}