using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class driveradvancesalary
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string driverid { get; set; }
        public double advancesalaryamt { get; set; }
        public string advancesalarymonth { get; set; }
        public string advancesalaryyear { get; set; }
        public DateTime advancesalarydate { get; set; }
        public bool deleted { get; set; }

    }
}