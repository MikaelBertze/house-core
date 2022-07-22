using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class PowerService
    {
        private IMongoDatabase _db;
        public PowerService(IMongoDatabase db)
        {
            _db = db;
        }

        // public ChargeLimit GetShargeLimit()
        // {
        //     IMongoDatabase db = _mongoClient.GetDatabase("house");
        //     db.GetCollection<
        // }
        public PowerInfo GetInfo()
        {
            var dayStart = DateTime.Today;
            var tomorrow = DateTime.Today + TimeSpan.FromDays(1);
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            
            var powerPerHour = _db.GetCollection<PerHourAggregate>("power_per_hour");
            //var powerPrice = _db.GetCollection<PowerPrice>("power_price_hour");
            
            var powerDocs = powerPerHour.Find(x => x.Start >= monthStart.ToUniversalTime()).ToList();

            //var priceDocs = powerPrice.Find(x=> x.TimeStampDay == now.ToString("yyyy-MM-dd") && x.TimeStampHour == now.ToString("HH:00")).ToList();
            var todayDocs = powerDocs.Where(x => x.StartLocalTime >= dayStart && x.StartLocalTime < tomorrow).ToList();
            var thisHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == now.Hour);
            var preHour = todayDocs.SingleOrDefault(x => x.StartLocalTime.Hour == (now - TimeSpan.FromHours(1)).Hour);
            var maxMonth = powerDocs.MaxBy(x => x.Consumption);
            var todayMax = todayDocs.MaxBy(x => x.Consumption);
            var monthConsumption = powerDocs.Sum(x => x.Consumption);

            // high load
            // mon - fri 0600-1800 CET
            var highLoadDocs = powerDocs.Where(x => (int)x.StartLocalTime.DayOfWeek >= 1 && (int)x.StartLocalTime.DayOfWeek <= 6 && x.StartLocalTime.TimeOfDay.Hours >= 6 && x.StartLocalTime.TimeOfDay.Hours < 18);
            var maxHighLoadMonth = highLoadDocs.MaxBy(x => x.Consumption);
            
            //Prognosis
            var currentAverage = AverageConsumption(60);

            return new PowerInfo() { 
                CurrentHour = new AccumulatedPowerInfo
                {
                    Consumption = thisHour != null ? thisHour.Consumption : -1,
                    Cost = thisHour != null ? thisHour.Consumption * thisHour.Price_SEK : 0
                },
                CurrentDay = new AccumulatedPowerInfo
                {
                    Consumption = todayDocs.Sum(x => x.Consumption),
                    Cost = todayDocs.Sum(x => x.Consumption * x.Price_SEK)
                },
                CurrentMonth = new AccumulatedPowerInfo
                {
                    Consumption = powerDocs.Sum(x => x.Consumption),
                    Cost = powerDocs.Sum(x => x.Consumption * x.Price_SEK)
                },
                CurrentDayMax = new MaxPowerConsumptionInfo
                {
                    Consumption = todayMax.Consumption,
                    PointInTime = todayMax.StartLocalTime,
                },
                CurrentMonthMax = new MaxPowerConsumptionInfo
                {
                    Consumption = maxMonth.Consumption,
                    PointInTime = maxMonth.StartLocalTime,
                },
                CurrentMonthHighLoadMax = new MaxPowerConsumptionInfo
                {
                    Consumption = maxHighLoadMonth.Consumption,
                    PointInTime = maxHighLoadMonth.StartLocalTime,
                },
                CurrentAverage = currentAverage,
                HourPrognosis = (thisHour?.Consumption ?? 0) + ((1 - DateTime.Now.Minute/60.0) * (currentAverage / 1000)),
                CurrentHourPrice = thisHour?.Price??-9999
            };
        }

        private double AverageConsumption(int seconds)
        {
            var t = DateTime.Now - TimeSpan.FromSeconds(seconds);
            
            var power = _db.GetCollection<Power>("power");
            
            var documents = power.Find(x => x.Time >= t).ToList();
            if (documents.Any())
                return documents.Sum(x => x.MeanEffect) / documents.Count();
            
            return 0;
        }
    }
}