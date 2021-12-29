using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

    public double MonthMaxHighLoad {get;set;}
    public DateTime MonthMaxHighLoadHour {get;set;}
    
    public double CurrentAverage {get;set;}
    public double HourPrognosis {get;set;}
}

[BsonIgnoreExtraElements]
public class PowerRaw {
    [BsonElement("ts")]
    public DateTime Time {get;set;}
    
    [BsonElement("mean_effect")]
    public double MeanEffect {get;set;}
}

[BsonIgnoreExtraElements]
public class Power {

    [BsonElement("start")]
    public DateTime Start {get;set;}
    public DateTime StartLocalTime => Start.ToLocalTime();
    
    [BsonElement("stop")]
    private DateTime Stop {get;set;}
    public DateTime StopLocalTime => Stop.ToLocalTime();
    [BsonElement("duration_s")]
    public double Duration_s {get;set;}
    [BsonElement("consumption")]
    public double Consumption {get;set;}
    [BsonElement("unit")]
    public string Unit {get;set;}
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
        var powerPerHour = db.GetCollection<Power>("power_per_hour");
        var powerDocs = powerPerHour.Find(x => x.Start >= monthStart.ToUniversalTime()).ToList();
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
            CurrentHour = thisHour.Consumption,
            Today = todayDocs.Sum(x => x.Consumption),
            PreviousHour = preHour.Consumption,
            TodayMax = todayMax.Consumption,
            TodayMaxHour = todayMax.StartLocalTime,
            MonthMax = maxMonth.Consumption,
            MonthMaxHour = maxMonth.StartLocalTime,
            MonthMaxHighLoad = maxHighLoadMonth.Consumption,
            MonthMaxHighLoadHour = maxHighLoadMonth.StartLocalTime,
            CurrentAverage = currentAverage,
            HourPrognosis = thisHour.Consumption + ((1 - DateTime.Now.Minute/60.0) * (currentAverage / 1000)),
        };
    }

    private double AverageConsumption(int seconds){
        var t = DateTime.Now - TimeSpan.FromSeconds(seconds);
        IMongoDatabase db = _mongoClient.GetDatabase("house");
        var power = db.GetCollection<PowerRaw>("power");
        
        var documents = power.Find(x => x.Time >= t).ToList();
        return documents.Sum(x => x.MeanEffect) / documents.Count();
   }
}