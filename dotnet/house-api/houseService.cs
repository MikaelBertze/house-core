using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
//using MoreLinq.Extensions

public class PowerInfo {
    public double CurrentHour {get;set;}
    public double PreviousHour {get;set;}

    public double Today {get;set;}
    public double TodayMax {get;set;}
    public DateTime TodayMaxHour {get;set;}
    public double MonthMax {get;set;}
    public DateTime MonthMaxHour {get;set;}

}

public class Power {
    public DateTime Dt {get;set;}
    public double Consumption {get;set;}
}

public class HouseService
{
    private IMongoClient _mongoClient;
    public HouseService(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public PowerInfo GetInfo()
    {
        var dayStart = DateTime.Today;
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        IMongoDatabase db = _mongoClient.GetDatabase("house");
        var powerPerHour = db.GetCollection<BsonDocument>("power_per_hour");
        var filter = Builders<BsonDocument>.Filter.Gte("start", monthStart.ToUniversalTime());
        var docs = powerPerHour.Find(filter).ToList();
        var powerDocs = docs.Select(x => new Power { Dt = x["start"].ToLocalTime(), Consumption = x["consumption"].AsDouble}).ToList();
        var todayDocs = powerDocs.Where(x => x.Dt > dayStart);
        var thisHour = todayDocs.SingleOrDefault(x => x.Dt.Hour == now.Hour);
        var preHour = todayDocs.SingleOrDefault(x => x.Dt.Hour == (now - TimeSpan.FromHours(1)).Hour);
        var maxMonth = powerDocs.MaxBy(x => x.Consumption);
        var todayMax = todayDocs.MaxBy(x => x.Consumption);
        
        return new PowerInfo() { 
            CurrentHour = thisHour.Consumption,
            Today = todayDocs.Sum(x => x.Consumption),
            PreviousHour = preHour.Consumption,
            TodayMax = todayMax.Consumption,
            TodayMaxHour = todayMax.Dt,
            MonthMax = maxMonth.Consumption,
            MonthMaxHour = maxMonth.Dt
        };
    }

    
}