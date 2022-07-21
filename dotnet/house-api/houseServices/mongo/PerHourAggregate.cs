using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class PerHourAggregate {

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
    [BsonElement("price")]
    public double Price {get;set;}

}
