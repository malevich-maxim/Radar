using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ProtoBuf;
using RadarStorage;

namespace DataSeedConsole
{
    class Program
    {
        private string _path = "D:\\";
        static void Main(string[] args)
        {
            var year = 2020;
            var month = 6;

            var records = new List<RadarRecord>();

            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var record = new RadarRecord();
                record.Date = new DateTime(year, month, random.Next(1, 7), random.Next(0, 23), random.Next(0, 59), random.Next(0, 59), random.Next(0, 1000));
                record.Number = $"{random.Next(1435, 1437)} РР-7";
                record.Speed = (float)random.Next(60, 100);
                records.Add(record);
            }

            var recsByDate = records.GroupBy(r => r.Date.Date);

            foreach (var recByDate in recsByDate)
            {
                var path = $"D:\\{recByDate.Key.Date.ToString("dd/MM/yyyy")}.dat";
                if (!File.Exists(path))
                {
                    using (File.Create(path)) ;
                }
                using (var file = new StreamWriter(path, true))
                {
                    Serializer.Serialize(file.BaseStream, recByDate);
                }
            }
        }
    }
}
