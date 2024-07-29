namespace API.Models
{
    public class Users
    {
        public required string XPCode { get; set;}
        public required string UserName { get; set;}
        public required string XWD { get; set;}
        public string? ConfirmXWD { get; set;}
        public string? Email { get; set;}
        public bool Premium { get; set;}
    }
}