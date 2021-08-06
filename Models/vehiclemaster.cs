using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class vehiclemaster
    {
        public string id { get; set; }
        public string vehiclename { get; set; }
        public DateTime createAt { get; set; }
        public bool deleted { get; set; }
        public bool approved { get; set; }
    }
}