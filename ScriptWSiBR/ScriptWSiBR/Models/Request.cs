using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScriptWSiBR.Models
{
   
    public class RequestInsertLead
    {
        public string contactId { get; set; }
        public string campaignId { get; set; }
        public string aux1 { get; set; }
        public string aux2 { get; set; }
        public string aux3 { get; set; }
    }

         public class RequestTicket
    {
        public string conversationId { get; set; }
        public string codAgente { get; set; }
         public string nomAgente { get; set; }
         public string rucAgente { get; set; }
         public string telAgente { get; set; }
         public string fecContacto { get; set; }
        public string Direccion { get; set; }
        public string Producto { get; set; }
         public string plazo { get; set; }
         public string userAtencion { get; set; }
          public string Observacion { get; set; }


    }

    public class RequestCrearCliente
    {
        public string CENTRAL { get; set; }
        public string TERRI { get; set; }
        public string OFICINA { get; set; }
        public string OFICINA_N { get; set; }
        public string GESTOR { get; set; }
        public string NDOC_RUC { get; set; }
        public string NOMBRE { get; set; }
        public string NETCASH { get; set; }
        public string VERSION { get; set; }
        public string NOMBRE1 { get; set; }
        public string CARGO1 { get; set; }
        public string CORREO1 { get; set; }
        public string TELEFONO1 { get; set; }
        public string CELULAR1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string CARGO2 { get; set; }
        public string CORREO2 { get; set; }
        public string TELEFONO2 { get; set; }
        public string CELULAR2 { get; set; }
        public string NOMBRE3 { get; set; }
        public string CARGO3 { get; set; }
        public string CORREO3 { get; set; }
        public string TELEFONO3 { get; set; }
        public string CELULAR3 { get; set; }


    }

    public class RequestCiabeiEPA
    {
        public string ConversationID { get; set; }
        public string Origen { get; set; }
        public string Telefono { get; set; }
        public string Agente { get; set; }
        public string SEGMENTO { get; set; }
        public string PREGUNTA_CATEGORIA { get; set; }
        public string PREGUNTA_DESCRIPCION { get; set; }
        public string PREGUNTA_VALOR { get; set; }
        public string PREGUNTA_VALOR_DESCRIPCION { get; set; }
     


    }
    public class RequestRecordings
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
      


    }

}