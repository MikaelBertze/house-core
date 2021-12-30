namespace HouseCore.Contracts{
    public class PowerInfo {
        public double CurrentHour {get;set;}
        public double PreviousHour {get;set;}

        public double Today {get;set;}
        public double TodayMax {get;set;}
        public DateTime TodayMaxHour {get;set;}
        public double MonthMax {get;set;}
        public DateTime MonthMaxHour {get;set;}

        public double MonthMaxHighLoad {get;set;}
        public DateTime MonthMaxHighLoadHour {get;set;}
        
        public double CurrentAverage {get;set;}
        public double HourPrognosis {get;set;}
    }
}