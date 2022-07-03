namespace HouseCore.Contracts{
    
    public class TemperatureInfo
    {
        public DateTime ReportTime {get;set;}
        public string Sensor {get;set;}
        public double Temperature {get;set;}

        public double DayMax {get;set;}
        public double DayMin {get;set;}

    }
}