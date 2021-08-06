   using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class customer
    {
        public string id { get; set; }
        public string userid { get; set; }       
        public string name { get; set; }
        public string mobileno { get; set; }
        public string othermobileno { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string landmark { get; set; }   
        public string gstin { get; set; }
        public DateTime createAt { get; set; }
        public bool deleted { get; set; }
    }
}