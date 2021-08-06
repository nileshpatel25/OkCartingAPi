using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class LoginStatus
    {
        public string id { get; set; }
        public string userid { get; set; }
        public DateTime registrationdate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime subscriptionstartdate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime subscriptionenddate { get; set; }
        public bool deleted { get; set; }
    }
}