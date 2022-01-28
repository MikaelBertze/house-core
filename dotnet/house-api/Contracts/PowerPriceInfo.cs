namespace HouseCore.Contracts{
    
    public class PowerPriceHour
    {
        public string Date {get;set;}
        public int Hour {get;set;}
        public double Price {get;set;}
        public double PriceIndex {get;set;}
    }
    public class PowerPriceInfo {
        
        public PowerPriceHour[] PowerPriceHours {get;set;}
        //public PowerPriceHour CurrentHour {get;set;}
    }
}