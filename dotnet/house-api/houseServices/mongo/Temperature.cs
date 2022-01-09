using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Temperature {

    [BsonElement("ts")]
    public DateTime TimeStamp {get;set;}
    
    [BsonElement("sensor_id")]
    public string SensorId {get;set;}
    [BsonElement("value")]
    public double Value {get;set;}
}