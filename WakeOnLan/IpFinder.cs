using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLanLibrary
{
    public class IpFinder
    {
        public static string FindIpAddressByMacAddress(string macAddress, string currentIpAddress)
        {
            var arpEntities = new ArpHelper().GetArpResult();
            var ip = arpEntities.FirstOrDefault(
                a => string.Equals(
                    a.MacAddress,
                    macAddress,
                    StringComparison.CurrentCultureIgnoreCase))?.Ip;
            return ip;
        }

        private static List<string> GetListOfSubnetIps(string currentIp)
        {
            var a = currentIp.Split('.');
            var lst = new List<string>();

            for (int i = 1; i <= 255; i++)
            {
                lst.Add($"{a[0]}.{a[1]}.{a[2]}.{i}");
            }

            lst.Remove(currentIp);

            return lst;
        }

        public static string GetOwnIPV4()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        } 
    }
}
