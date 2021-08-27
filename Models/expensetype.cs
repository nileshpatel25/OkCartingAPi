using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class expensetype
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public bool deleted { get; set; }
    }
}