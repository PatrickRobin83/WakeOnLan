using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WakeOnLanLibrary
{
    public class IpFinder
    {
        public static string FindIpAddressByMacAddress(string macAddress, string currentIpAddress)
        {
            Parallel.ForEach(GetListOfSubnetIps(currentIpAddress), delegate (string s)
            {
                DeviceScanner.IsHostAccessible(s);
            });

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
    }
}
