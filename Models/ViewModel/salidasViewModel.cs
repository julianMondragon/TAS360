using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Models.ViewModel
{
    public class salidasViewModel
    {
        public salidasViewModel()
        {
            salidas = new List<Salidas>();
            Trama = new ordenCargaDescargaSalidas();
            Trama2 = new volumenTanques();
        }
        public List<Salidas> salidas { get; set; }
        public ordenCargaDescargaSalidas Trama { get; set; }
        public volumenTanques Trama2 { get; set; }
    }
   public class ordenCargaDescargaSalidas
    {
        public string TipoTransaccion { get; set; }
        public string NumeroOperacion { get; set; }
        public string NumeroCopartimiento { get; set; }
        public string EstadoTransaccion {  get; set; }
        public string Razon { get; set; }
        public string ModuloOperacion { get; set; }
        public string VolumenNatural { get; set; }
        public string VolumenNeto { get; set; }
        public string Temperatura { get; set; }
        public string CodigoProductoAnterior { get; set; }
        public string FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public string FechaTermino { get; set; }
        public string HoraTermino { get; set; }
        public string PosicionCarga { get; set; }
        public string FlujoPromedio { get; set; }
        public string FactorMedicion { get; set; }
        public string CodigoProductoNuevo { get; set; }
    }
    public class volumenTanques
    {
        public string TipoTransaccion { get; set; }
        public string NumeroTanque { get; set; }
        public string CodigoProductoAnterior { get; set; }
        public string VolumenTotalNeto { get; set; }
        public string VolumenNeto { get; set; }
        public string VolumenNatural { get; set; }
        public string TemperaturaPromedio { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string CodigoProductoNuevo { get; set; }
    }
}