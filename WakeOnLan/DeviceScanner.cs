using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WakeOnLanLibrary
{
    public class DeviceScanner
    {
        public static bool IsHostAccessible(string hostNameOrAddress)
        {
            bool isHostAccessible = false;
            if (hostNameOrAddress != null)
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(hostNameOrAddress, 1000);
                if(reply != null && reply.Status == IPStatus.Success)
                {
                    isHostAccessible = true;
                }
            }

            return isHostAccessible; 
        }
    }
}
