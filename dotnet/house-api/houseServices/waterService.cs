using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class WaterService
    {
        private IMongoClient _mongoClient;
        public WaterService(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public WaterInfo GetInfo()
        {
            var dayStart = DateTime.Today;
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var waterPerHour = db.GetCollection<PerHourAggregate>("water_per_hour");
            var waterDocs = waterPerHour.Find(x => x.Start >= monthStart.ToUniversalTime()).ToList();
            var todayDocs = waterDocs.Where(x => x.StartLocalTime >= dayStart).ToList();
            var thisHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == now.Hour);
            
            return new WaterInfo() { 
                CurrentHour = thisHour?.Consumption ?? -1,
                Today = todayDocs.Sum(x => x.Consumption),
                Month = waterDocs.Sum(x => x.Consumption),
            };
        }
    }
}