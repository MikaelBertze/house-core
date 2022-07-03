using HouseCore.Contracts;
using HouseCore.HouseService.Mongo;
using MongoDB.Driver;

namespace HouseCore.HouseService
{
    public class SettingsService
    {
        public SettingsService(object x) {}
        public HouseSettings GetSettings(){
            return new HouseSettings();
        }

        public bool SetPowerLimit(int limit){ return true;}
    }

    
}