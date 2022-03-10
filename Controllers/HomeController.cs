using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           salidasViewModel salidasVM = new salidasViewModel();
           using (bd_Entities db = new bd_Entities())
            {
                var aux = (from s in db.Salidas select s);
                foreach (var a in aux)
                {
                    salidasVM.salidas.Add(a);
                }
            }
            return View(salidasVM);
        }

        public ActionResult About()
        {
            entradasViewModel salidasVM = new entradasViewModel();
            using (bd_Entities db = new bd_Entities())
            {
                var aux = (from s in db.Entradas select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        salidasVM.entradas.Add(a);
                    }
                }                
            }
            return View(salidasVM);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult InterpretarTrama(string id)
        {
            salidasViewModel salidasVM = new salidasViewModel();
            string trama = null;
            using (bd_Entities db = new bd_Entities())
            {
                int Id = int.Parse(id);
                var aux = from s in db.Salidas where  s.Id == Id select s;
                foreach (var a in aux)
                {
                    salidasVM.salidas.Add(a);
                }
                trama = db.Salidas.Find(int.Parse(id)).Transaccion;
            }

            salidasVM.Trama = new ordenCargaDescarga();
            salidasVM.Trama = leertrama(trama);


            return View(salidasVM);
        }

        public ordenCargaDescarga leertrama( string trama )
        {
            ordenCargaDescarga ordenCarga = new ordenCargaDescarga();

            //Tipo de  transaccion
            string tipotransaccion = trama.Substring(0, 2);
            switch (tipotransaccion)
            {
                case "01":
                    #region Orden de Carga
                    ordenCarga.TipoTransaccion = "Orden de Carga";
                    //Numero de operacion
                    string numeroOperacion = trama.Substring(2, 5);
                    ordenCarga.NumeroOperacion = numeroOperacion;
                    //Numero de compartimiento 
                    string numeroComparimiento = trama.Substring(7, 1);
                    ordenCarga.NumeroCopartimiento = numeroComparimiento;
                    //Estado de transaccion
                    string estadoTransaccion = trama.Substring(8, 1);
                    if(estadoTransaccion == "0" )
                        ordenCarga.EstadoTransaccion = "Normal";
                    else
                        ordenCarga.EstadoTransaccion = estadoTransaccion;
                    //Razones de cancelacion 
                    string RazonCancelacion = trama.Substring(9, 2);
                    ordenCarga.RazonCancelacion = RazonCancelacion;
                    //Modulo de operacion
                    string moduloOperacion = trama.Substring(11, 2);
                    switch (moduloOperacion)
                    {
                        case "11":
                            ordenCarga.ModuloOperqcion = "Descarga de auto-tanques";
                            break;
                        case "12":
                            ordenCarga.ModuloOperqcion = "Descarga de carro-tanques";
                            break;
                        case "13":
                            ordenCarga.ModuloOperqcion = "Descarga de barcos";
                            break;
                        case "21":
                            ordenCarga.ModuloOperqcion = "Carga de auto-tanques";
                            break;
                        case "22":
                            ordenCarga.ModuloOperqcion = "Carga de carro-tanques";
                            break;
                        case "23":
                            ordenCarga.ModuloOperqcion = "Carga de carro-tanques";
                            break;
                        case "24":
                            ordenCarga.ModuloOperqcion = "Movimiento en poliducto";
                            break;
                        case "26":
                            ordenCarga.ModuloOperqcion = "Carga de tambores";
                            break;
                        case "27":
                            ordenCarga.ModuloOperqcion = "Autoconsumo";
                            break;
                    }
                    //identificacion del vehiculo 
                    string vehiculo = trama.Substring(15, 9);
                    ordenCarga.identificacionVehiculo = vehiculo;
                    //Clave antigua del producto
                    string productoV = trama.Substring(24, 4);
                    switch (productoV)
                    {
                        case "0470":
                            ordenCarga.codigoProductoAnterior = "Diesel automotriz";
                            break ;
                        case "0270":
                            ordenCarga.codigoProductoAnterior = "Premium";
                            break;
                        case "0266":
                            ordenCarga.codigoProductoAnterior = "Regular";
                            break;
                        default:
                            ordenCarga.codigoProductoAnterior = productoV;
                            break;
                    }                    
                    //volumen natural programado 
                    string volumenN = trama.Substring(28, 10);
                    ordenCarga.VolumenNatural = volumenN;
                    //Clave del cliente
                    string cliente = trama.Substring(39, 5);
                    ordenCarga.ClaveCliente = cliente;
                    //Clave del producto
                    string productoN = trama.Substring(45, 5);
                    switch (productoN)
                    {
                        case "34006":
                            ordenCarga.CodigoProductoNuevo = "Diesel automotriz";
                            break;
                        case "32012":
                            ordenCarga.CodigoProductoNuevo = "Premium";
                            break;
                        case "32011":
                            ordenCarga.CodigoProductoNuevo = "Regular";
                            break;
                        default:
                            ordenCarga.CodigoProductoNuevo = productoN;
                            break;
                    }
                    //Densidad 
                    //ordenCarga.densidad = trama.Substring(50, 4);
                    #endregion
                    break;
                case "02":
                    ordenCarga.TipoTransaccion = "Confirmacion de orden de carga";
                    break;
            }
           
            return ordenCarga; 
        }

        public ActionResult GenerarTrama()
        {
            GetCatalogos();
            return View();
        }

        private void GetCatalogos()
        {
            
            List<SelectListItem> ListTipoTrancc = new List<SelectListItem>();
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Confirmacion de orden de Carga / Descarga",
                Value = "02",
                Selected = true
            });
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Movimiento entre tanques",
                Value = "05",
                Selected = true
            });
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Movimiento en Poliducto",
                Value = "03",
                Selected = true
            });

            ViewBag.ListTipoTrancc = ListTipoTrancc;
                
        }
    }
}