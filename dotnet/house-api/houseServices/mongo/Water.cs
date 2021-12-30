using MongoDB.Bson.Serialization.Attributes;

namespace HouseCore.HouseService.Mongo
{
    [BsonIgnoreExtraElements]
    public class Water
    {
        [BsonElement("ts")]
        DateTime Time {get;set;}
        [BsonElement("consumption")]
        double Consumption {get;set;}
    }
}