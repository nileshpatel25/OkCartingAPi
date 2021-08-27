using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class expensedetails
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string expensetypeid { get; set; }
        public double amount { get; set; }
        public string chequeno { get; set; }
        public DateTime expensedate { get; set; }
        public string remark { get; set; }
        public bool deleted { get; set; }
    }
}