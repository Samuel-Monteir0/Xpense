namespace API.Models
{
    public class DailyTrend
    {
        public TotalCredDebt? Monday {get; set;} 
        public TotalCredDebt? Tuesday {get; set;} 
        public TotalCredDebt? Wednesday {get; set;} 
        public TotalCredDebt? Thursday {get; set;} 
        public TotalCredDebt? Friday {get; set;} 
        public TotalCredDebt? Saturday {get; set;} 
        public TotalCredDebt? Sunday {get; set;}   
        public bool success {get; set;}
        public string? error {get; set;}
    }
}