using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class WaterService
    {
        private IMongoDatabase _db;
        public WaterService(IMongoDatabase db)
        {
            _db = db;
        }

        public WaterInfo GetInfo()
        {
            var dayStart = DateTime.Today;
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var waterPerHour = _db.GetCollection<PerHourAggregate>("water_per_hour");
            var waterDocs = waterPerHour.Find(x => x.Start >= monthStart.ToUniversalTime()).ToList();
            var todayDocs = waterDocs.Where(x => x.StartLocalTime >= dayStart).ToList();
            var thisHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == now.Hour);
            var hourConsumptions = new List<WatherHour>();
            foreach (var d in todayDocs.OrderBy(x => x.Start))
            {
                var hour = d.StartLocalTime.Hour;
                var consumption = d.Consumption;
                hourConsumptions.Add(new WatherHour { Hour = hour, Consumption = consumption });
            }
            
            return new WaterInfo() { 
                CurrentHour = thisHour?.Consumption ?? -1,
                Today = todayDocs.Sum(x => x.Consumption),
                Month = waterDocs.Sum(x => x.Consumption),
                Hours = hourConsumptions
            };
        }

        public WaterInfos GetInfos(DateOnly date)
        {
            var waterPerHour = _db.GetCollection<PerHourAggregate>("water_per_hour");
            var waterDocs = waterPerHour.Find(x => 
                x.Start >= date.ToDateTime(new TimeOnly(0, 0)).ToUniversalTime()
                && x.Start <= date.ToDateTime(new TimeOnly(0, 0)).ToUniversalTime() + TimeSpan.FromDays(1)
                ).ToList();
            var info = new WaterInfos() { Date = date.ToString() };
            info.HourConsumptions = new List<WatherHour>();
            foreach(var d in waterDocs)
            {
                var hour = d.StartLocalTime.Hour;
                var consumption = d.Consumption;
                info.HourConsumptions.Add(new WatherHour { Hour = hour, Consumption = consumption });
            }
            return info;
        }
    }
}