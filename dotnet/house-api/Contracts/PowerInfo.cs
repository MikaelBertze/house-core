namespace HouseCore.Contracts{
    public class PowerInfo {
        public AccumulatedPowerInfo CurrentHour {get;set;}
        public AccumulatedPowerInfo CurrentDay {get;set;}
        public AccumulatedPowerInfo CurrentMonth { get; set; }
        
        public MaxPowerConsumptionInfo CurrentDayMax {get;set;}
        public MaxPowerConsumptionInfo CurrentMonthMax { get; set; }
        public MaxPowerConsumptionInfo CurrentMonthHighLoadMax { get; set; }

        public double CurrentAverage {get;set;}
        public double HourPrognosis {get;set;}
        public double CurrentHourPrice {get;set;}
    }

    public class AccumulatedPowerInfo
    {
        public double Consumption { get; set; }
        public double Cost { get; set; }
    }

    public class MaxPowerConsumptionInfo
    {
        public double Consumption { get; set; }
        public DateTime PointInTime { get; set; }
    }

}