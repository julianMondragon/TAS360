using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class PendientesViewModel
    {
        public PendientesViewModel()
        {
            Pendientes = new List<Pendiente>();
        }
        public List<Pendiente> Pendientes { get; set; }
    }
}