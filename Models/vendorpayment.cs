using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class vendorpayment
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string vendorid { get; set; } //petrolpump Id
        public string purchaseid { get; set; } //id of fuel master primary key
        public string paymenttype { get; set; } //receieved/given
        public string paymentby { get; set; } //checque or cash
        public string chequeno { get; set; }
        public DateTime paymentdate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime createAt { get; set; }
        public double amount { get; set; }
        public string remark { get; set; }
        public bool deleted { get; set; }
    }
}