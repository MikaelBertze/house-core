using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
//using MoreLinq.Extensions

public class PowerInfo {
    public double CurrentHour {get;set;}
    public double PreviousHour {get;set;}

    public double Today {get;set;}
    public double TodayMax {get;set;}
    public double MonthMax {get;set;}
    public DateTime MonthMaxHour {get;set;}

}

public class Power {
    public DateTime Dt {get;set;}
    public double Consumption {get;set;}
}

public class HouseService
{
    public PowerInfo GetInfo()
    {
        var dayStart = DateTime.Today;
        var now = DateTime.Now;


        var monthStart = new DateTime(now.Year, now.Month, 1);

        MongoClient dbClient = new MongoClient("mongodb://192.168.50.5:27017");

        IMongoDatabase db = dbClient.GetDatabase("house");
        var powerPerHour = db.GetCollection<BsonDocument>("power_per_hour");
        var filter = Builders<BsonDocument>.Filter.Gte("start", monthStart.ToUniversalTime());
        var docs = powerPerHour.Find(filter).ToList();

        var powerDocs = docs.Select(x => new Power { Dt = x["start"].AsLocalTime, Consumption = x["consumption"].AsDouble}).ToList();
        var todayDocs = powerDocs.Where(x => x.Dt > dayStart);
        var thisHour = todayDocs.SingleOrDefault(x => x.Dt == );
        var preHour = todayDocs.SingleOrDefault(x => x.Dt.Hour == (now - TimeSpan.FromHours(1)).Hour);
        //var consumptions = powerDocs.Select(x => x.Consumption) docs.Select(x => new Power() { dt = x["start"].ToUniversalTime(), Consumption = x["consumption"].AsDouble});

        var maxMonth = powerDocs.MaxBy(x => x.Consumption);

        //var maxHighLoad = consumptions.Where(x => (int)x.dt.DayOfWeek >= 1 && (int)x.dt.DayOfWeek <= 5 && x.dt.Hour >= 5 && x.dt.Hour <= 17);

        return new PowerInfo() { 
            CurrentHour = thisHour.Consumption,
            Today = todayConsumptions.Sum(x => x.Consumption),
            PreviousHour = preHour.Consumption,
            TodayMax = todayConsumptions.Max(x => x.Consumption),
            MonthMax = maxMonth.Consumption,
            MonthMaxHour = maxMonth.Dt
        };
    }

    private bool SameHour(DateTime x, DateTime y)
    {
        
    }

}