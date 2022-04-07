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
        }
       public Ucls Ucls { get; set; }

    }

    public class Ucls
    {
        public Ucls()
        {
            Ucl1 = new List<TotIny9>();
            Ucl2 = new List<TotIny8>();
            Ucl3 = new List<TotIny7>();
        }

        public List<TotIny9> Ucl1 { get; set; }
        public List<TotIny8> Ucl2 { get; set; }
        public List<TotIny7> Ucl3 { get; set; }
    }
}