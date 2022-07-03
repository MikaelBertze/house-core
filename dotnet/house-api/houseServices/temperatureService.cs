using System;
using System.Linq;
using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class TemperatureService
    {
        private IMongoClient _mongoClient;
        public TemperatureService(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public IEnumerable<TemperatureInfo> GetInfo()
        {
            var today = DateTime.Today.ToUniversalTime();
            //var now = DateTime.Now;
            
            //var utcnow = now.ToUniversalTime();
            
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var temperatures = db.GetCollection<Temperature>("temperature");
            var data = temperatures.Find(x=> x.TimeStamp >= today - TimeSpan.FromDays(1)).ToList();
            var sensors = data.Select(d => d.SensorId).Distinct();

            foreach(var sensor in sensors){
                var temps = data.Where(x => x.SensorId == sensor).OrderByDescending(x => x.TimeStamp).ToList();
                var doc = temps.FirstOrDefault();
                var minValue = temps.Where(x => x.TimeStamp >= today).Min(x => x.Value);
                var maxValue = temps.Where(x => x.TimeStamp >= today).Max(x => x.Value);
                yield return new TemperatureInfo { Sensor = sensor, Temperature = doc.Value, ReportTime = doc.TimeStamp.ToLocalTime(), DayMax = maxValue, DayMin = minValue };
            }
        }
    }
}