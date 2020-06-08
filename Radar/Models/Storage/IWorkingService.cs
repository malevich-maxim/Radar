using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Radar.Models.Storage
{
    public interface IWorkingService
    {
        bool IsWorking();
    }

    public class WorkingService : IWorkingService
    {
        private readonly WorkingTime _workingTime;
        public WorkingService(IConfiguration configuration)
        {
            _workingTime = configuration.GetSection("WorkingTime").Get<WorkingTime>();
        }

        public bool IsWorking()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            return currentTime > _workingTime.Start && currentTime < _workingTime.End;
        }
    }
}
