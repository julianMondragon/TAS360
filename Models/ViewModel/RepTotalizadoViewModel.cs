using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class RepTotalizadoViewModel
    {
        public RepTotalizadoViewModel()
        {
            Ucls = new Ucls();
            Balance = new ReporteBalance();
        }
       public Ucls Ucls { get; set; }
       public double Total1 { get; set; }
       public double Total2 { get; set; }
       public double Total3 { get; set; }

        public ReporteBalance Balance { get; set; }      

    }

    public class Ucls
    {
        public Ucls()
        {
            Ucl1 = new List<TotIny9>();
            Ucl2 = new List<TotIny8>();
            Ucl3 = new List<TotIny7>();
            Ucl4 = new List<TotIny6>();
            Ucl5 = new List<TotIny10>();
            Ucl6 = new List<TotIny11>();
            Ucl7 = new List<TotIny12>();
            Ucl8 = new List<TotIny13>();
            Ucl9 = new List<TotIny14>();
        }

        public List<TotIny9> Ucl1 { get; set; }
        public List<TotIny8> Ucl2 { get; set; }
        public List<TotIny7> Ucl3 { get; set; }
        public List<TotIny6> Ucl4 { get; set; }
        public List<TotIny10> Ucl5 { get; set; }
        public List<TotIny11> Ucl6 { get; set; }
        public List<TotIny12> Ucl7 { get; set; }
        public List<TotIny13> Ucl8 { get; set; }
        public List<TotIny14> Ucl9 { get; set; }
    }

    public class ReporteBalance
    {
        public double? Existencia_inicial { get; set; }
        public double? Recibos { get; set; }
        public double? Salidas { get; set; }
        public double? ExistenciaFinal { get; set; }
        public double? Resta_de_volumen_Tanq { get; set; }
        public double? Sobrante_Faltante { get; set; }
        public double? Nivel_Final { get; set; }
        public double? Nivel_Inicial { get; set; }
    }
}