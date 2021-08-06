using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class profileRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string compnayname { get; set; }
        public string address { get; set; }
        public string latitude { get; set; }
        public string logitude { get; set; }
        public string address2 { get; set; }
        public string landmark { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string othercontactno { get; set; }
        public string discription { get; set; }
        public string gstin { get; set; }
        public string pushTokenId { get; set; }
    }
}