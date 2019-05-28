using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;

namespace Ex3.Models
{
    public class Info
    {
        private double lon, lat;
        private string ip;
        private int port, time;
        private NetworkStream ns;
        private StreamReader sr;
        //private StreamWriter sw;

        //Thread threadI;

        TcpClient _client;

        //TcpListener listener;

        // while connected keep listening to the data received from the flightGear
 //       public bool shouldStop
 //       {
 //           get;
 //           set;
 //       }

        public double Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        public double Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        private static Info m_Instance = null;

        public static Info Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Info();
                }
                return m_Instance;
            }
        }

        //private Info() {
        //shouldStop = false;
        //}

        //public void closeThread()
        //{
        //    threadI.Abort();
        //}

        //public void disConnect()
        //{
        //shouldStop = true;
        //_client.Close();
        //}

        public void connect()
        {
            // connecting as client
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            _client = new TcpClient();
            _client.Connect(ep);
        }

        // calls this function every four seconds
        public void listen()
        {
            ns = _client.GetStream();
            sr = new StreamReader(ns);
            //sw = new StreamWriter(ns);
            string lonP = "get /position/longitude-deg\r\n";
            string latP = "get /position/latitude-deg\r\n";
            string lonValue = getValue(lonP);
            string latValue = getValue(latP);
            Console.WriteLine("lon {0}, lat {1}",lonValue, latValue);
            castD(lonValue, latValue);
        }

        public void castD(string lonValue, string latValue)
        {
            Lon = double.Parse(lonValue);
            Lat = double.Parse(latValue);
        }

        public string getValue(string path)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(path);
            ns.Write(buffer, 0, buffer.Length);
            return sr.ReadLine().Split('=')[1].Split(' ')[1].Split('\'')[1];
        }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Val");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}