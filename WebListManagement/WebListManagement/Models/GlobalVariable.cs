using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebListManagement.Models
{
    public class GlobalVariable
    {
        public static string idCliente = ConfigurationManager.AppSettings["PCClientID"];
        public static string clientSecret = ConfigurationManager.AppSettings["PCClientSecret"];

    }
}