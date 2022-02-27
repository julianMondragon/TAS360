using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class salidasViewModel
    {
        public salidasViewModel()
        {
            salidas = new List<Salidas>();
            Trama = new ordenCargaDescarga();
        }
       public List<Salidas> salidas { get; set; }
        public ordenCargaDescarga Trama { get; set; }
        
    }

   public class ordenCargaDescarga
    {
        public string TipoTransaccion { get; set; }
        public string NumeroOperacion { get; set; }
        public string NumeroCopartimiento { get; set; }
        public string EstadoTransaccion {  get; set; }
        public string RazonCancelacion { get; set; }
        public string ModuloOperqcion { get; set; }
        public string identificacionVehiculo { get; set; }
        public string codigoProductoAnterior { get; set; }
        public string VolumenNatural { get; set; }
        public string ClaveCliente { get; set; }
        public string CodigoProductoNuevo { get; set; }
        public string densidad { get; set; }
    }
}