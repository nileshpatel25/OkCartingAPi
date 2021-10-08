using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class totaladvancesalary
    {
        public string id { get; set; }
        public string advancesalaryid { get; set; }
        public double advancesalaryamt { get; set; }
        public bool deleted { get; set; }
    }
}