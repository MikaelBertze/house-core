namespace HouseCore.Contracts{
    public class WaterInfo {
        public double CurrentHour {get;set;}
        public double Today {get;set;}
        public double Month {get;set;}

        public List<WatherHour> Hours {get;set;}
    }

    public class WaterInfos
    {
        public string Date { get;set;}
        public List<WatherHour> HourConsumptions {get;set;}
    }
    public class WatherHour
    {
        public int Hour {get;set;}
        public double Consumption {get;set;}
    }
}