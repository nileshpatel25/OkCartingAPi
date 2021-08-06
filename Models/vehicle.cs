using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class vehicle
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string vehiclename { get; set; }
        public string vehiclenumber { get; set; }
        public double perhourrate { get; set; }
        public DateTime createAt { get; set; }
        public bool deleted { get; set; }
    }
}