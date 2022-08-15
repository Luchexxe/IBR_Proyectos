using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsUPCServices.Models
{
    public class mensajeEntrada
    {
        public string contactlistid { get; set; }
        public string Nombre { get; set; }
     
    }

    public class ListaCallHistory
    {
        public string contactlistid { get; set; }
      

    }

    public class CallHistoryIN
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string conversationID { get; set; }
        public int PageSize { get; set; }
      

    }

    public class WSUC
    {
        public string satisfactorio { get; set; }
        public string mensaje { get; set; }

    }
    public class CallHistoryOut2
    {
        public string guidRegistro { get; set; }
        public string nombreEntidad { get; set; }
        public string idGrabacionLlamada { get; set; }
        public string propietarioLlamada { get; set; }
        public string resultadoLlamada { get; set; }
        public string canalLlamada { get; set; }
        public string descripción { get; set; }
        public DateTime fechaSeguimiento { get; set; }
        public string listaContacto { get; set; }
        public string Campania { get; set; }
        public bool? Intentos { get; set; }





    }

    public class CallHistoryOut
    {
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int? Contador { get; set; }





    }
}