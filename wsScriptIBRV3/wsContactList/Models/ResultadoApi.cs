using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsTeleavenceService.Models
{
    public class ResultadoApi
    {
        public string contactListID { get; set; }
        public string estado { get; set; }
        public string mensaje { get; set; }
    }

    public class ResultadoApliIndividual
    {
        public string contactID { get; set; }
        public string estado { get; set; }
        public string mensaje { get; set; }
    }

    public class ResultadoDNC
    {
        
        public string estado { get; set; }
        public string mensaje { get; set; }
    }
}