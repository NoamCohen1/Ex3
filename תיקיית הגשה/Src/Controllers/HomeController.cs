using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class HomeController : Controller
    {
        public Boolean isConnected;

        public HomeController()
        {
            isConnected = false;
        }

        // GET: Home
        public ActionResult Index()
        {
            Info.Instance.Ip = "127.0.0.1";
            Info.Instance.Port = 5400;
            if(!isConnected)
            {
                Info.Instance.connect();
                isConnected = true;
            }
            Info.Instance.listen();
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        [HttpGet]
        public ActionResult pointDisplay(string ip, int port)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            if (!isConnected)
            {
                Info.Instance.connect();
                isConnected = true;
            }
            Info.Instance.listen();
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        [HttpGet]
        public ActionResult pathDisplay(string ip, int port, int time)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            Info.Instance.Time = time;
            if (!isConnected)
            {
                Info.Instance.connect();
                isConnected = true;
            }
            Info.Instance.listen();
            Session["time"] = time;
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        [HttpGet]
        public ActionResult fileDisplay(string ip, int port, int time, int seconds, string fileName)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            Info.Instance.Time = time;
            Info.Instance.FileName = fileName;
            if (!isConnected)
            {
                Info.Instance.connect();
                isConnected = true;
            }
            Session["time"] = time;
            Session["seconds"] = seconds;
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        // GET: Home
        public ActionResult loadDisplay(string fileName, int time)
        {
            Session["time"] = time;
            Info.Instance.FileName = fileName;
            Info.Instance.EndOfFile = 0;           
            return View();
        }

        [HttpPost]
        public string GetVal()
        {
            Info.Instance.listen();

            return ToXml();
        }

        [HttpPost]
        public string WriteVal()
        {
            Info.Instance.writeToFile();

            return ToXml();
        }

        [HttpPost]
        public string ReadVal()
        {
            Info.Instance.readFromFile();

            return ToXml();
        }

        // write into an XML all the values of the lon and the lat
        private string ToXml()
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();

            Info.Instance.ToXml(writer);

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public ActionResult chooseDisplay(string str, int num)
        {
            System.Net.IPAddress ip = null;
            bool isValid = System.Net.IPAddress.TryParse(str, out ip);
            if (isValid)
            {
                pointDisplay(str, num);
                return View("pointDisplay");
            }
            loadDisplay(str, num);
            return View("loadDisplay");
        }
    }
}