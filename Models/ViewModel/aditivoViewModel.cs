using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class aditivoViewModel
    {
        public DateTime FechaVolumen { get; set; }
        public double? Volumen { get; set; }
        public DateTime FechaNivel { get; set; }
        public double? Nivel { get; set; }
        public string Tag { get; set; }
        public string NombreAditivo { get; set; }

    }
}