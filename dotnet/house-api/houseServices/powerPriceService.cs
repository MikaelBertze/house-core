using System.Runtime.Serialization;
using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class PowerPriceService
    {
        private IMongoClient _mongoClient;
        public PowerPriceService(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public PowerPriceInfo GetInfo(string date)
        {
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var powerPrice = db.GetCollection<PowerPrice>("power_price_hour");
            
            Console.WriteLine(date);
            var priceDocs = powerPrice.Find(x=> x.TimeStampDay == date).ToList();
            
            return new PowerPriceInfo() {
                PowerPriceHours = priceDocs.Select(pd => new PowerPriceHour { 
                    Date = pd.TimeStampDay,
                    Hour = DateTime.Parse(pd.TimeStamp).Hour,
                    Price = pd.Value,
                    }).ToArray(),
            };
        }

        private double AverageConsumption(int seconds)
        {
            var t = DateTime.Now - TimeSpan.FromSeconds(seconds);
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var power = db.GetCollection<Power>("power");
            
            var documents = power.Find(x => x.Time >= t).ToList();
            if (documents.Any())
                return documents.Sum(x => x.MeanEffect) / documents.Count();
            
            return 0;
        }
    }
}