using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class Appversion
    {
        public string id { get; set; }
        public string version { get; set; }
        public bool forceUpdate { get; set; }
        public bool deleted { get; set; }
        public DateTime publishdate { get; set; }
    }
}