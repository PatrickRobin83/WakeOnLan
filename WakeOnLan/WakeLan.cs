using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WakeOnLanLibrary
{
    public class WakeLan
    {
        static string ApplicationPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
        public static void WakeUp(string MAC_ADDRESS, IPAddress IPBCast)
        {
            UdpClient UDP = new UdpClient();

            try
            {
                UDP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                int offset = 0;
                byte[] buffer = new byte[512];   // more than enough :-)

                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    buffer[offset++] = 0xFF;

                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        buffer[offset++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                UDP.EnableBroadcast = true;
                UDP.Send(buffer, 512, new IPEndPoint(IPBCast, 0x1));
            }
            catch (Exception ex)
            {
                UDP.Close();
                Console.WriteLine(ex.Message);
            }
        }

        public static IPAddress get_broadcast()
        {
            try
            {
                string ipadress;
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); // get a list of all local IPs
                IPAddress localIpAddress = ipHostInfo.AddressList[0]; // choose the first of the list
                ipadress = Convert.ToString(localIpAddress); // convert to string
                ipadress = ipadress.Substring(0, ipadress.LastIndexOf(".") + 1); // cuts of the last octet of the given IP 
                ipadress += "255"; // adds 255 witch represents the local broadcast
                return IPAddress.Parse(ipadress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return IPAddress.Parse("127.0.0.1");// in case of error return the local loopback
            }
        }

        public static List<string> ReadMacFromTextFile()
        {
            List<string> macAdresses = new List<string>();
            string PathToMacFile = $"{ApplicationPath}\\MacAdress.txt";

            if (!File.Exists(PathToMacFile))
            {
                Console.WriteLine("File does not Exist");
            }
            else
            {
                StreamReader sr = new StreamReader(PathToMacFile);
                
                string line = "";

                while((line = sr.ReadLine()) != null)
                {
                    if(line.Substring(0,1) != "#" || line.Length > 0)
                    {
                        macAdresses.Add(line);
                    }
                }
            }

            return macAdresses;
        }
    }
}
