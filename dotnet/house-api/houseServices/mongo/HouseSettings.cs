using MongoDB.Bson.Serialization.Attributes;

namespace HouseCore.HouseService.Mongo {
    
    [BsonIgnoreExtraElements]
    public class HouseSettings {
        public double ShellyLimit {get;set;}
        
    }
}