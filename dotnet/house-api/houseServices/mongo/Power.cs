using MongoDB.Bson.Serialization.Attributes;

namespace HouseCore.HouseService.Mongo {
    
    [BsonIgnoreExtraElements]
    public class Power {
        [BsonElement("ts")]
        public DateTime Time {get;set;}
        
        [BsonElement("mean_effect")]
        public double MeanEffect {get;set;}
    }
}