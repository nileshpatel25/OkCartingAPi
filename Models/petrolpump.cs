using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class petrolpump
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string ownername { get; set; }
        public string contactno { get; set; }
        public string otherconatcno { get; set; }
        public string gst { get; set; }
        public bool deleted { get; set; }
    }
}