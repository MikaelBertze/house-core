using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class PowerPrice 
{
    [BsonElement("TimeStamp")]
    public string TimeStamp {get;set; } 
    [BsonElement("Value")]
    public double Value {get;set;}

    [BsonElement("PriceArea")]
    public string PriceArea {get;set;}

    [BsonElement("TimeStampDay")]
    public string TimeStampDay {get;set;}

    [BsonElement("TimeStampHour")]
    public string TimeStampHour {get;set;}
}