using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsUPCServices.Models
{
    public class mensajeSalida
    {
        public string mensaje { get; set; }
        public DateTime fechaEnvio { get; set; }
        public string ContactListID { get; set; }
      //  public string Numero { get; set; }
        public string estado { get; set; }
       // public string CodigoConclusion { get; set; }
        
    }

}