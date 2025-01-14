using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace PortKnock
{
    internal class Program
    {
        static string info = "https://github.com/kamilmroczkowski/PortKnock\n" +
            "Kamil Mroczkowski kamil@adminkm.pl\n";
        static string help = "Usage: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " /h:domain.ltd /p:u7123;t4421;u10092;t46331 [/d:500] [/w]\n" +
            "/h - hostname (many times declare)\n/p - ports tcp/udp\n/d - delay ms\n/w - wait for any key";
        static void Main(string[] args)
        {
            Console.WriteLine(info);
            int delay = 500;
            string ports = "";
            string tmp = "";
            int tmpPort = 0;
            string tmpTcp = "";
            bool wait = false;
            List<PortUT> portsList = new List<PortUT>();
            List<string> hosts = new List<string>();
            TcpClient TCPclient = new TcpClient();
            UdpClient UDPclient = new UdpClient();
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            if (args.Length == 0)
            {
                Help();
            }
            else
            {
                foreach (string arg in args)
                {
                    if (arg.Substring(0, 1) == "/")
                    {
                        tmp = arg.Substring(0, 2);
                        if (tmp == "/h")
                        {
                            hosts.Add(arg.Split(':')[1]);
                        }
                        else if (tmp == "/p")
                        {
                            ports = arg.Split(':')[1];
                            foreach (string port in ports.Split(','))
                            {
                                tmpTcp = port.Substring(0, 1);
                                if (int.TryParse(port.Substring(1), out tmpPort))
                                {
                                    if (tmpPort <= 0)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                                if (tmpTcp == "t")
                                {
                                    portsList.Add(new PortUT(tmpPort, true));
                                }
                                else
                                {
                                    portsList.Add(new PortUT(tmpPort));
                                }
                            }
                        }
                        else if (tmp == "/d")
                        {
                            if (int.TryParse(arg.Split(':')[1], out delay))
                            {
                                if (delay <= 0)
                                {
                                    delay = 500;
                                }
                            }
                        }
                        else if (tmp == "/w")
                        {
                            wait = true;
                        }
                    }
                }
                if (hosts.Count == 0 || portsList.Count == 0)
                {
                    Help();
                }
            }
            foreach (string host in hosts)
            {
                Console.WriteLine("Connect to: " + host + " Delay: " + delay + "ms");
                foreach (PortUT portUT in portsList)
                {
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " Send: " + portUT.Port + " (" + ((portUT.Tcp) ? "TCP" : "UDP") + ")");
                    if (portUT.Tcp)
                    {
                        try
                        {
                            TCPclient = new TcpClient();
                            TCPclient.ConnectAsync(host, portUT.Port).Wait(delay);
                            Thread.Sleep(delay);
                            TCPclient.Close();
                        }
                        catch
                        {
                            Thread.Sleep(delay);
                        }
                    }
                    else
                    {
                        UdpClient udpclient = new UdpClient();
                        UDPclient.Connect(host, portUT.Port);
                        UDPclient.Send(sendBytes, sendBytes.Length);
                        Thread.Sleep(delay);
                        //UDPclient.Close();
                    }
                }
                Console.WriteLine();
            }
            if (wait)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        static void Help()
        {
            Console.WriteLine(help);
            System.Environment.Exit(1);
        }
    }
}
