using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radar.Models.Storage
{
    public interface IDataProvider 
    {
        Task<List<T>> GetDataByDateAsync<T>(DateTime date);

        void SaveDataAsync<T>(T[] entities, DateTime date = default(DateTime));
    }
}
