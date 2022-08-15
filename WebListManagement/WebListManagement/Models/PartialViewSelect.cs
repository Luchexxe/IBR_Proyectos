using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebListManagement.Models
{
    public class PartialViewSelect
    {
        public string PartialNuevoMessage { get; set; }
        public string PartialExistenteMessage { get; set; }
        public int SelectedParameter { get; set; }
    }
}