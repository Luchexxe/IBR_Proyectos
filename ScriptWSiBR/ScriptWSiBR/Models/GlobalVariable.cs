using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ScriptWSiBR.Models
{
    public class GlobalVariable
    {
        public static string idCliente = ConfigurationManager.AppSettings["PCClientID"];
        public static string clientSecret = ConfigurationManager.AppSettings["PCClientSecret"];
        public static SqlConnection connexionBD()
        {
            var ConnectionStringBD = ConfigurationManager.AppSettings["ConnectionBD"];
            SqlConnection conn = new SqlConnection(ConnectionStringBD);
            return conn;
        }
    }
}