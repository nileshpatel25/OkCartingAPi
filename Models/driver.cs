using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class driver
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string vehicleid { get; set; }
        public string name { get; set; }
        public string mobileno { get; set; }
        public string othermobileno { get; set; }
        public string address { get; set; }        
        public string address2 { get; set; }
        public string landmark { get; set; }
        public string adharcardno { get; set; }
        public string licenseno { get; set; }
       
        public string driverimage { get; set; }
        public string licenseimage { get; set; }
        public string adharcardimage { get; set; }
        public string hireon { get; set; }
        public double perdaysalary { get; set; }
        public double fulldayhour { get; set; }
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime dateofjoining { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime licensevalidupto { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime dateofresinging { get; set; }
        public bool active { get; set; }
        public bool deleted { get; set; }
    }
}