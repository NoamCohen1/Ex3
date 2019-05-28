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
        // GET: Home
        public ActionResult Index()
        {
            Info.Instance.Ip = "127.0.0.1";
            Info.Instance.Port = 5402;
            Info.Instance.connect();
            Info.Instance.listen();
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }


        [HttpGet]
        public ActionResult pointDisplay(string ip, int port)
        {
            //Info.Instance.Ip = "127.0.0.1";
            //Info.Instance.Port = 5402; 
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            Info.Instance.connect();
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
            Info.Instance.connect();
            Info.Instance.listen();
            Session["time"] = time;
            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        [HttpPost]
        public string GetVal()
        {
            Info.Instance.listen();

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
            writer.WriteStartElement("Val");

            Info.Instance.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}