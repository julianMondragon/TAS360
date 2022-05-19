using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class TablaCubViewModel
    {
        public string TAD { get ; set; }
        public string Tag { get; set; }
        public string Capacidad { get; set; }
        public double Altura { get; set; }
        public string Producto { get; set; }
        public double Fondo_Rango1 { get; set; }
        public double Fondo_Rango2 { get; set; }
        public double ZonaCritica_Rango1 { get; set; }
        public double ZonaCritica_Rango2 { get; set; }
        public double VolumenXmil { get; set; }


    }
}