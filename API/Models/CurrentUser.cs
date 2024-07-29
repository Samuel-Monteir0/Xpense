using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CurrentUser
    {
        public string xpCode {get; set;}
        public string username {get; set;}
        public string email {get; set;}
        public string token {get; set;}
        public bool result {get; set;} 
        public string? error {get; set;}
    }
}