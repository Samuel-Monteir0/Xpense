namespace API.Models
{
    public class Transaction
    {
        public string XpCode {get; set;}
        public string? Account {get; set;} 
        public float? Amount {get; set;}
        public string? Date {get; set;}
        public string? TransType {get; set;}
    }
}