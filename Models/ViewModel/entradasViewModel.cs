using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class entradasViewModel
    {
        public entradasViewModel()
        {
            entradas = new List<Entradas>();
            Trama = new OrdenCargaDescarga();
        }
        public List<Entradas> entradas { get; set; }
        public OrdenCargaDescarga Trama { get; set; }
    }

    public class OrdenCargaDescarga
    {
        public string TipoTransaccion { get; set; }
        public string NumeroOperacion { get; set; }
        public string NumeroCopartimiento { get; set; }
        public string EstadoTransaccion { get; set; }
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