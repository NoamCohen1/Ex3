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

        public string FileName
        {
            get;
            set;
        }

        public double Throttle
        {
            get;
            set;
        }

        public double Rudder
        {
            get;
            set;
        }

        public int EndOfFile
        {
            get;
            set;
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
            string throttle = "get /controls/engines/current-engine/throttle\r\n";
            string rudder = "get /controls/flight/rudder\r\n";
            string lonValue = getValue(lonP);
            string latValue = getValue(latP);
            string throttleValue = getValue(throttle);
            string rudderValue = getValue(rudder);
            Console.WriteLine("lon {0}, lat {1}",lonValue, latValue);
            castD(lonValue, latValue, throttleValue, rudderValue);
        }

        public void castD(string lonValue, string latValue, string throttleValue, string rudderValue)
        {
            // TODO - delete
            //Random r = new Random();
            //
            Lon = double.Parse(lonValue);
            Lat = double.Parse(latValue);
            Throttle = double.Parse(throttleValue);
            Rudder = double.Parse(rudderValue);

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
            writer.WriteElementString("EndOfFile", this.EndOfFile.ToString());
            writer.WriteEndElement();
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt"; // The Path of the Secnario

        public void writeToFile()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            listen();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine(Lon.ToString());
                    file.WriteLine(Lat.ToString());
                    file.WriteLine(Throttle.ToString());
                    file.WriteLine(Rudder.ToString());
                }
        }

        public static int currLine = 0;

        public void readFromFile()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            string[] lines = System.IO.File.ReadAllLines(path); // reading all the lines of the file
            if (currLine >= lines.Length)
            {
                EndOfFile = 1;
                currLine = 0;
            }
            else
            {
                Lon = double.Parse(lines[currLine]);
                Lat = double.Parse(lines[++currLine]);
                currLine += 3;
            }
        }
    }
}