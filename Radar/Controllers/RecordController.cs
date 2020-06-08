using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Radar.Models.Storage;
using RadarStorage;

namespace Radar.Controllers
{
    public class RecordController : Controller
    {
        private readonly IDataProvider _dataProvider;
        private readonly IWorkingService _workingService;
        public RecordController(IWorkingService workingService, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _workingService = workingService;
        }


        public async Task<IActionResult> GetMaxMinSpeed(DateTime date)
        {
            if (!_workingService.IsWorking())
                return View("Error");
            var data = await _dataProvider.GetDataByDateAsync<RadarRecord>(date);
            if (!data.Any())
                return View("Empty");
            

            var max = data.Max(s => s.Speed);
            var min = data.Min(s => s.Speed);
            var group = data.GroupBy(s => s.Speed);

            var maxSpeed = group.Single(g=> g.Key == max);
            var minSpeed = group.Single(g => g.Key == min);

            ViewBag.RecordsMax = maxSpeed;
            ViewBag.RecordsMin = minSpeed;
            return this.View("MaxMin");
        }

        public async Task<IActionResult> GetSpeeding(DateTime date, float speed)
        {
            if (!_workingService.IsWorking())
                return View("Error");

            var data = await _dataProvider.GetDataByDateAsync<RadarRecord>(date);
            var result = data.Where(r => r.Speed > speed);

            ViewBag.Control = result.OrderBy(r=>r.Speed).ToList();
            return View("SpeedControl");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}