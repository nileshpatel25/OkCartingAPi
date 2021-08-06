using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class driverattendance
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string driverid { get; set; }  
      
       
      
        public DateTime dtattencanceDate { get; set; }
      
        public int status { get; set; } //fullday=0/halfday=1/Absance=2

        public double hour { get; set; }
        public bool deleted { get; set; }
    }
}