namespace HouseCore.Contracts{
    
    public class TemperatureInfo
    {
        public DateTime ReportTime {get;set;}
        public string Sensor {get;set;}
        public double Temperature {get;set;}
    }
}