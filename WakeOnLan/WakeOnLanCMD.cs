using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using WakeOnLanLibrary;

namespace WakeOnLan
{
    class WakeOnLanCMD
    {
        
        static List<string> macAdresses = new List<string>();
        static List<string> HostsOffline = new List<string>();
        static string remoteHostIP;
        static string HostName = System.Net.Dns.GetHostName();
        static System.Net.IPHostEntry hostInfo = Dns.GetHostByName(HostName);
        static string IpAdresse = hostInfo.AddressList[0].ToString();
        
        static void Main(string[] args)
        {
            
            foreach (string mac in WakeLan.ReadMacFromTextFile())
            {
                WakeLan.WakeUp(mac.Replace("-",""), WakeLan.get_broadcast());
                Console.WriteLine($"Rechner mit MAC: {mac} gestartet.\r\n");    
            }
            AddMacToList();
            Thread.Sleep(30000);
            hostsNotReachable();
            
            if (HostsOffline != null && HostsOffline.Count > 0)
            {
                tryAgain(HostsOffline);
            }
            
            //Console.WriteLine("Zum Beenden eine beliebige Taste drücken ...\r\n");
            //Console.ReadKey();
            Thread.Sleep(15000);
        }

        public static List<string> AddMacToList()
        {
            foreach (string mac in WakeLan.ReadMacFromTextFile())
            {
                if (macAdresses != null)
                {
                    if (!macAdresses.Contains(mac))
                    {
                        macAdresses.Add(mac);
                    }
                }
            }
            return macAdresses;
        }

        public static List<string> hostsNotReachable()
        {
            foreach (string mac in macAdresses)
            {
                remoteHostIP = IpFinder.FindIpAddressByMacAddress(mac, IpAdresse);

                if (DeviceScanner.IsHostAccessible(remoteHostIP))
                {
                    Console.WriteLine($"Rechner {getHostNameFromIp(remoteHostIP)} mit IP:{remoteHostIP} ist nun erreichbar\r\n");
                }
                else
                {
                    HostsOffline.Add(remoteHostIP);
                }
            }
            return HostsOffline;
        }

        public static void tryAgain(List<string> hosts)
        {
            for (int j = 0; j <= 5; j++)
            {
                for (int i = 0; i < hosts.Count; i++)
                {
                    if (hosts.Count > 0 && hosts[i] != null && hosts[i].Length > 0)
                    {
                        if (DeviceScanner.IsHostAccessible(hosts[i]))
                        {
                            Console.WriteLine($"Rechner: {getHostNameFromIp(hosts[i])} mit IP:{ hosts[i]} ist nun erreichbar\r\n");
                            hosts.Remove(hosts[i]);
                        }
                    }
                    i++;
                }
                j++;
                Thread.Sleep(15000);
            }

            foreach (string notFoundIP in hosts)
            {
                Console.WriteLine("Rechner wurde nicht gefunden. Möglicherweise ist der Rechner nicht eingeschaltet \r\noder nicht mit dem Netzwerk verbunden.\r\n");
            }
        }

        public static string getHostNameFromIp(string ip)
        {
            IPHostEntry iPHost = Dns.GetHostEntry(ip);
            return iPHost.HostName;
        }

    }
}
