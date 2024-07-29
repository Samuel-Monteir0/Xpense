using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class AmountData
    {
        public string Account {get; set;}
        public float Amount { get; set; }
        public string? Type {get; set;}
        public string Date{ get; set; }
    }
}