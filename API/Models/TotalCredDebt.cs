namespace API.Models
{
    public class TotalCredDebt
    {
        public float? Cred {get; set;}
        public float? Debt {get; set;}
        public float? Total {get; set;}
        public List<AmountData>? ShowData {get; set;}
        public bool success {get; set;}
        public string? error {get; set;}
    }
}