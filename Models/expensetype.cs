using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class expensetype
    {
        public string id { get; set; }
        public string name { get; set; }
        public string rimark { get; set; }
        public bool deleted { get; set; }
    }
}