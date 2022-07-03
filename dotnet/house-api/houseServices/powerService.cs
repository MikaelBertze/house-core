using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class PowerService
    {
        private IMongoClient _mongoClient;
        public PowerService(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        // public ChargeLimit GetShargeLimit()
        // {
        //     IMongoDatabase db = _mongoClient.GetDatabase("house");
        //     db.GetCollection<
        // }
        public PowerInfo GetInfo()
        {
            var dayStart = DateTime.Today;
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            IMongoDatabase db = _mongoClient.GetDatabase("house");
            var powerPerHour = db.GetCollection<PerHourAggregate>("power_per_hour");
            var powerPrice = db.GetCollection<PowerPrice>("power_price_hour");
            
            var powerDocs = powerPerHour.Find(x => x.Start >= monthStart.ToUniversalTime()).ToList();

            var priceDocs = powerPrice.Find(x=> x.TimeStampDay == now.ToString("yyyy-MM-dd") && x.TimeStampHour == now.ToString("HH:00")).ToList();
            var todayDocs = powerDocs.Where(x => x.StartLocalTime >= dayStart).ToList();
            var thisHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == now.Hour);
            var preHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == (now - TimeSpan.FromHours(1)).Hour);
            var maxMonth = powerDocs.MaxBy(x => x.Consumption);
            var todayMax = todayDocs.MaxBy(x => x.Consumption);
            
            // high load
            // mon - fri 0600-1800 CET
            var highLoadDocs = powerDocs.Where(x => (int)x.StartLocalTime.DayOfWeek >= 1 && (int)x.StartLocalTime.DayOfWeek <= 6 && x.StartLocalTime.TimeOfDay.Hours >= 6 && x.StartLocalTime.TimeOfDay.Hours < 18);
            var maxHighLoadMonth = highLoadDocs.MaxBy(x => x.Consumption);
            
            //Prognosis
            var currentAverage = AverageConsumption(60);

            return new PowerInfo() { 
                CurrentHour = thisHour?.Consumption ?? -1,
                Today = todayDocs.Sum(x => x.Consumption),
                PreviousHour = preHour?.Consumption ?? -1,
                TodayMax = todayMax?.Consumption ?? -1,
                TodayMaxHour = todayMax?.StartLocalTime ?? DateTime.MinValue,
                MonthMax = maxMonth?.Consumption ?? -1,
                MonthMaxHour = maxMonth?.StartLocalTime ?? DateTime.MinValue,
                MonthMaxHighLoad = maxHighLoadMonth?.Consumption ?? -1,
                MonthMaxHighLoadHour = maxHighLoadMonth?.StartLocalTime ?? DateTime.MinValue,
                CurrentAverage = currentAverage,
                HourPrognosis = (thisHour?.Consumption ?? 0) + ((1 - DateTime.Now.Minute/60.0) * (currentAverage / 1000)),
                CurrentHourPrice = priceDocs.SingleOrDefault()?.Value??-9999
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