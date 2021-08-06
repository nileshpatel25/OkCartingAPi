using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class jobworkdetail
    {
        public string id { get; set; } 
        public string jobworkid { get; set; }
        public string vehicleid { get; set; }
        public double hour { get; set; }
        public double perhourrate { get; set; }
        public double totalamount { get; set; }
        public DateTime workdate { get; set; }       
        public string discrition { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime createAt { get; set; }
        public bool deleted { get; set; }
    }
}