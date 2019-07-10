using System;
using System.Net;
using System.Threading;
using WakeOnLanLibrary;

namespace WOLan
{
    class WakeOnLanCMD
    {
        static void Main(string[] args)
        {
            string HostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry hostInfo = Dns.GetHostByName(HostName);
            string IpAdresse = hostInfo.AddressList[0].ToString();
            string remoteHostIP;
            foreach (string mac in WakeLan.ReadMacFromTextFile())
            {
                WakeLan.WakeUp(mac.Replace("-",""), WakeLan.get_broadcast());
                //Console.WriteLine($"Rechner mit MAC: {mac} gestartet.");    
            }
            Thread.Sleep(1000);

            foreach (string mac in WakeLan.ReadMacFromTextFile())
            {
                remoteHostIP = IpFinder.FindIpAddressByMacAddress(mac, IpAdresse);
                Console.WriteLine(DeviceScanner.IsHostAccessible(remoteHostIP));
            }
            Console.ReadLine();
        }
    }
}
