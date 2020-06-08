using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using ProtoBuf;
using RadarStorage;

namespace Radar.Models.Storage
{
    public class ProtoDataProvider : IDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _storagePath;
        private const string DataExtName = ".dat";
        private const string DataFormat = "dd/MM/yyyy";

        public ProtoDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _storagePath = _configuration["AppSettings:StoragePath"];
            if (string.IsNullOrEmpty(_storagePath))
                _storagePath = Path.Combine(Directory.GetCurrentDirectory());
            InitPath();
        }

        private void InitPath()
        {
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public Task<List<T>> GetDataByDateAsync<T>(DateTime date)
        {
            if (!File.Exists(Path.Combine(_storagePath, date.ToString(DataFormat)+DataExtName)))
                return Task.FromResult(new List<T>());

            return Task.Run(() =>
            {
                var path = Path.Combine(_storagePath, date.ToString(DataFormat) + DataExtName);
                using (var file = new StreamReader(path))
                {
                    var result = Serializer.DeserializeItems<T>(file.BaseStream, PrefixStyle.Base128, 1).ToList();
                    return result;
                }
            });
        }

        public void SaveDataAsync<T>(T[] entities, DateTime date = default(DateTime))
        {
            if(date == default(DateTime))
               date = DateTime.Now;

            Task.Run(() =>
            {
                using (var file = new StreamWriter(Path.Combine(_storagePath, date.Date + DataExtName), true))
                {
                    Serializer.Serialize(file.BaseStream, entities);
                }
            });
        }
    }
}
