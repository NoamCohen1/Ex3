using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Text;

using System.Net;
using System.Net.Sockets;


namespace Ex3.Models
{
    public class Info
    {
        private double lon, lat;
        private string ip;
        private int port, time;

        //Thread threadI;

        TcpClient _client;

        TcpListener listener;

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

        //public void connect()
        //{
        //    // listen to the client
        //    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
        //    listener = new TcpListener(ep);
        //    listener.Start();
        //    //Console.WriteLine("gggg");
        //    _client = listener.AcceptTcpClient();
        //    //Console.WriteLine("Info channel: Client connected");
        //    //threadI = new Thread(() => listen());
        //    //threadI.Start();
        //}

        public void connect()
        {
            // connecting as client
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            _client = new TcpClient();
            _client.Connect(ep);
            //listener = new TcpListener(ep);
            //listener.Start();
            //_client = listener.AcceptTcpClient();
            //Console.WriteLine("You are connected");
        }

        // calls this function every four seconds
        public void listen()
        {
            Byte[] bytes;
            NetworkStream ns = _client.GetStream();
            // if there is a message - listen and read it
            if (_client.ReceiveBufferSize > 0)
            {
                bytes = new byte[_client.ReceiveBufferSize];
                ns.Read(bytes, 0, _client.ReceiveBufferSize);
                string msg = Encoding.ASCII.GetString(bytes); //the message incoming
                splitMsg(msg);
                //Console.WriteLine("info");
                //Console.WriteLine(Lon);
                //Console.WriteLine(Lat);
                //Console.WriteLine(msg);
            }
            ns.Close();
            //_client.Close();
            //listener.Stop();
        }

        public void splitMsg(string msg)
        {
            string[] splitMs = msg.Split(',');
            Lon = double.Parse(splitMs[0]);
            Lat = double.Parse(splitMs[1]);
        }

        public void sendMessage(string[] splited, TcpClient tcpClient)
        {
            splited = Parse("get /position/longitude-deg");
            NetworkStream ns = tcpClient.GetStream();
            foreach (string split in splited)
            {
                // Send data to server
                string command = split;
                command += "\r\n";
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                ns.Write(buffer, 0, buffer.Length);
            }
        }

        private string[] Parse(string line)
        {
            string[] newLine = { "\r\n" };
            string[] input = line.Split(newLine, StringSplitOptions.None);
            return input;
        }
    }
}