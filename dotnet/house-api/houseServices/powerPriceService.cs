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

        public PowerPriceInfo GetInfo()
        {
            var start = DateTime.Today.ToUniversalTime() - TimeSpan.FromDays(2);
            var now = DateTime.UtcNow;
            
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var powerPrice = db.GetCollection<PowerPrice>("power_price_hour");
            var priceDocs = powerPrice.Find(x=> x.Dt >= start).SortBy(x => x.Dt).ToList();
            
            var currentHourDoc = priceDocs.SingleOrDefault(pd => pd.Dt.Date == now.Date && pd.Dt.Hour == now.Hour);

            return new PowerPriceInfo() {
                PowerPriceHours = priceDocs.Select(pd => new PowerPriceHour { 
                    Date = pd.TimeStampDay,
                    Hour = pd.Dt.ToLocalTime().Hour,
                    Price = pd.Value,
                    }).ToArray(),

                CurrentHour = new PowerPriceHour {
                    Date = currentHourDoc.TimeStampDay,
                    Hour = currentHourDoc.Dt.ToLocalTime().Hour,
                    Price = currentHourDoc.Value
                }
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