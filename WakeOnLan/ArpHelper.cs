﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace WakeOnLanLibrary
{
    public class ArpHelper
    {
        public List<ArpEntity> GetArpResult()
        {
            var p = Process.Start(new ProcessStartInfo("arp", "-a")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            });

            var output = p?.StandardOutput.ReadToEnd();
            p?.Close();

            return ParseArpResult(output);
        }

        private List<ArpEntity> ParseArpResult(string output)
        {

            var lines = output.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l));

            var result =
                from line in lines
                 select Regex.Split(line, @"\s+")
                    .Where(i => !string.IsNullOrWhiteSpace(i)).ToList()
                    into items
                 where items.Count == 3
                 select new ArpEntity()
                 {
                     Ip = items[0],
                     MacAddress = items[1],
                     Type = items[2]
                 };
            

            return result.ToList();
        }
    }
}
