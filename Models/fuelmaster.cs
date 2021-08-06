using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class fuelmaster
    {
        public string id { get; set; }
        public string userid { get; set; }

        public string petrolpumpid { get; set; }
        public string vehicleid { get; set; }
        public string driverid { get; set; }
        public string paymenttype { get; set; }
        public double rate { get; set; }
        public double liter { get; set; }
        public double totalamount { get; set; }
        public DateTime fueldate { get; set; }
        public string receipt { get; set; }
        public bool deleted { get; set; }

        
    }
}