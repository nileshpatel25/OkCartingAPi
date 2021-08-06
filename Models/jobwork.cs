using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class jobwork
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string customerid { get; set; }
        public string paymenttype { get; set; }
        public int invoiceno { get; set; }
        public string invoicepath { get; set; }
        public string status { get; set; }
        public DateTime createAt { get; set; }
        public bool deleted { get; set; }
        public List<jobworkdetail> jobworkdetails { get; set; }
    }
}