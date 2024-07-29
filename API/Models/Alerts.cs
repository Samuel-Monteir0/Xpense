namespace API.Models
{
    public class Alerts
    {
        public string? AlertName {get; set;}
        public string? AlertType {get; set;}
        public float? AlertValue {get; set;}
        public DateTime? AlertDate {get; set;}
        public bool? Active {get; set;} 
    }
}