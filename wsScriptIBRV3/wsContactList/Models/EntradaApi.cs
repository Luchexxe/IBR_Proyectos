using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsTeleavenceService.Models
{
    public class entradaNuevoContactList
    {
        public string nombreContactList { get; set; }
        public string campoTelefono { get; set; }
        public string tipoTelefono { get; set; }
        public string rutaCsv { get; set; }
        public string campoIdentificador { get; set; }
        public string division { get; set; }

    }

    public class entradaContactoListExistente
    {
        //public string contactlistid { get; set; }
        public string nombreContactList { get; set; }
        public string rutaCsv { get; set; }
        public string campoIdentificador { get; set; }
    }
    public class entradaDNCList
    {
        //public string contactlistid { get; set; }
        public string FechaActual { get; set; }
        //ejemplo:20220120

    }

    public class entradaContactoIndivididualListExistente
    {
        public string contactlistid { get; set; }
        public string nombreContactList { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
    public class ingresContactoReferido
    {
        public string campaignId { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
    public class actualizarContacto
    {
        public string contactListId { get; set; }
        public string contactId { get; set; }
        public string nombreContactList { get; set; }
        public bool llamable { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

    public class reciclarContacto
    {
        public string campaignId { get; set; }
        public string contactListId { get; set; }
        public List<string> listContactID { get; set; }
        public bool reprocesarPorPrimeraVez { get; set; }
    }

}