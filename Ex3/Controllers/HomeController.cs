using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Info.Instance.Ip = "127.0.0.1";
            Info.Instance.Port = 5402;
            Info.Instance.connect();
            //Info.Instance.listen();
            //double lon = Info.Instance.Lon;
            //double lat = Info.Instance.Lat;
            return View();
        }


        [HttpGet]
        public ActionResult display()
        {
            Info.Instance.Ip = "127.0.0.1";
            Info.Instance.Port = 5400;
            //Info.Instance.time = time;

            Info.Instance.listen();

            //Session["time"] = time;

            double lon = Info.Instance.Lon;
            double lat = Info.Instance.Lat;

            return View();
        }
    }
}