using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartingManagmentApi.Models
{
    public class RegisterRequest
    {
        public string password { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string source { get; set; }
        public string name { get; set; }
    }
}