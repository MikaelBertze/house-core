using System;
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
            var now = DateTime.Now;
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var temperatures = db.GetCollection<Temperature>("temperature");
            var sensors = temperatures.Distinct(x => x.SensorId, x=> x.TimeStamp > now - TimeSpan.FromDays(1)).ToList();

            foreach(var sensor in sensors){
                
                var doc = temperatures.Find(x => x.SensorId == sensor).SortByDescending(x => x.TimeStamp).FirstOrDefault();
                yield return new TemperatureInfo { Sensor = sensor, Temperature = doc.Value, ReportTime = doc.TimeStamp.ToLocalTime()};
            }
        }
    }
}