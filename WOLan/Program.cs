using System;
using System.Net;
using System.Threading;
using WakeOnLanLibrary;

namespace WOLan
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string mac in WakeLan.ReadMacFromTextFile())
            {
                WakeLan.WakeUp(mac, WakeLan.get_broadcast());
                Console.WriteLine($"Rechner mit MAC: {mac} gestartet.");
            }
            Thread.Sleep(3000);

        }
    }
}
