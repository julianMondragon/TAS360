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
            entradasViewModel entradasVM = new entradasViewModel();
            using (bd_Entities db = new bd_Entities())
            {
                var aux = (from s in db.Entradas select s);
                foreach (var a in aux)
                {
                    entradasVM.entradas.Add(a);
                }
            }
            return View(entradasVM);
        }
        public ActionResult About()
        {
            salidasViewModel salidasVM = new salidasViewModel();
            using (bd_Entities db = new bd_Entities())
            {
                var aux = (from s in db.Salidas select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        salidasVM.salidas.Add(a);
                    }
                }
            }
            return View(salidasVM);
        }
        public ActionResult Contact()
        {
            auditoriaViewModel auditoriaVM = new auditoriaViewModel();
            using (bd_Entities db = new bd_Entities())
            {
                var aux = (from s in db.Auditoria select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        auditoriaVM.auditoria.Add(a);
                    }
                }
            }
            return View(auditoriaVM);
        }
        public ActionResult InterpretarTrama(string id)
        {
            entradasViewModel entradasVM = new entradasViewModel();
            string trama = null;
            using (bd_Entities db = new bd_Entities())
            {
                int Id = int.Parse(id);
                var aux = from s in db.Entradas where s.Id == Id select s;
                foreach (var a in aux)
                {
                    entradasVM.entradas.Add(a);
                }
                trama = db.Entradas.Find(int.Parse(id)).Transaccion;
            }

            entradasVM.Trama = new OrdenCargaDescarga();
            entradasVM.Trama = leertrama(trama);


            return View(entradasVM);
        }


        public OrdenCargaDescarga leertrama(string trama)
        {
            OrdenCargaDescarga ordenCarga = new OrdenCargaDescarga();

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
                    if (estadoTransaccion == "0")
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
                    string vehiculo = trama.Substring(13, 11);
                    ordenCarga.identificacionVehiculo = vehiculo;
                    //Clave antigua del producto
                    string productoV = trama.Substring(24, 4);
                    switch (productoV)
                    {
                        case "0470":
                            ordenCarga.codigoProductoAnterior = "Diesel automotriz";
                            break;
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
            return View();
        }
        public ActionResult TramaConfirmacionO()
        {
            GetCatalogos();
            GetComportamientos();
            GetEstadoTran();
            GetModulo();
            GetProducto();
            return View();
        }
        public ActionResult TramaVolumenTanques()
        {
            GetCatalogos2();
            GetProducto();
            return View();
        }
        public ActionResult TramaCancelacionO()
        {
            GetCatalogos3();
            GetComportamientos();
            GetEstadoTran();
            GetModulo();
            GetProducto();
            return View();
        }
        
         
        private void GetCatalogos()
        {

            List<SelectListItem> ListTipoTrancc = new List<SelectListItem>();
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Seleccione una transacción",
                Value = "00"
            });
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Confirmacion de orden de Carga / Descarga",
                Value = "02",
                Selected = true
            });
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Volumen en tanques de almacenamiento",
                Value = "04"
            });
            ListTipoTrancc.Add(new SelectListItem
            {
                Text = "Cancelacion de orden de Carga / Descarga",
                Value = "02"
            });
            ViewBag.ListTipoTrancc = ListTipoTrancc;

        }
        private void GetCatalogos2()
        {

            List<SelectListItem> ListTipoTrancc2 = new List<SelectListItem>();
            ListTipoTrancc2.Add(new SelectListItem
            {
                Text = "Seleccione una transacción",
                Value = "00"
            });
            ListTipoTrancc2.Add(new SelectListItem
            {
                Text = "Confirmacion de orden de Carga / Descarga",
                Value = "02"
            });
            ListTipoTrancc2.Add(new SelectListItem
            {
                Text = "Volumen en tanques de almacenamiento",
                Value = "04",
                Selected = true
            });
            ListTipoTrancc2.Add(new SelectListItem
            {
                Text = "Cancelacion de orden de Carga / Descarga",
                Value = "02"
            });
            ViewBag.ListTipoTrancc2 = ListTipoTrancc2;

        }
        private void GetCatalogos3()
        {

            List<SelectListItem> ListTipoTrancc3 = new List<SelectListItem>();
            ListTipoTrancc3.Add(new SelectListItem
            {
                Text = "Seleccione una transacción",
                Value = "00"
            });
            ListTipoTrancc3.Add(new SelectListItem
            {
                Text = "Confirmacion de orden de Carga / Descarga",
                Value = "02"
            });
            ListTipoTrancc3.Add(new SelectListItem
            {
                Text = "Volumen en tanques de almacenamiento",
                Value = "04"
            });
            ListTipoTrancc3.Add(new SelectListItem
            {
                Text = "Cancelacion de orden de Carga / Descarga",
                Value = "02",
                Selected = true
            });
            ViewBag.ListTipoTrancc3 = ListTipoTrancc3;

        }
        private void GetComportamientos()
        {

            List<SelectListItem> ListComport = new List<SelectListItem>();
            ListComport.Add(new SelectListItem
            {
                Text = "Seleccione un comportamiento",
                Value = "00",
                Selected = true
            });
            ListComport.Add(new SelectListItem
            {
                Text = "Cargado",
                Value = "1"
            });
            ListComport.Add(new SelectListItem
            {
                Text = "Descargado",
                Value = "2"
            });
            ViewBag.ListComport = ListComport;

        }
        private void GetEstadoTran()
        {

            List<SelectListItem> ListEstadoTran = new List<SelectListItem>();
            ListEstadoTran.Add(new SelectListItem
            {
                Text = "Seleccione un estado de transacción",
                Value = "",
                Selected = true
            });
            ListEstadoTran.Add(new SelectListItem
            {
                Text = "Terminado",
                Value = "0"
            });
            ListEstadoTran.Add(new SelectListItem
            {
                Text = "Cancelado",
                Value = "1"
            });
            ViewBag.ListEstadoTran = ListEstadoTran;

        }
        private void GetModulo()
        {

            List<SelectListItem> ListModulos = new List<SelectListItem>();
            ListModulos.Add(new SelectListItem
            {
                Text = "Seleccione un modulo de operación",
                Value = "",
                Selected = true
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Descarga de auto-tanques",
                Value = "11"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Descarga de carro-tanques",
                Value = "12"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Descarga de barcos",
                Value = "13"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Carga de autotanques",
                Value = "21"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Carga de carro-tanques",
                Value = "22"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Carga de barcos",
                Value = "23"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Movimiento en poliducto",
                Value = "24"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Carga de tambores",
                Value = "26"
            });
            ListModulos.Add(new SelectListItem
            {
                Text = "Autoconsumo",
                Value = "27"
            });
            ViewBag.ListModulos = ListModulos;

        }
        private void GetProducto()
        {

            List<SelectListItem> ListProducto = new List<SelectListItem>();
            ListProducto.Add(new SelectListItem
            {
                Text = "Seleccione un estado de transacción",
                Value = "",
                Selected = true
            });
            ListProducto.Add(new SelectListItem
            {
                Text = "Diesel Automotriz",
                Value = "0470"
            });
            ListProducto.Add(new SelectListItem
            {
                Text = "Premium",
                Value = "0270"
            });
            ListProducto.Add(new SelectListItem
            {
                Text = "Regular",
                Value = "0266"
            });
            ViewBag.ListProducto = ListProducto;

        }
        
        
        [HttpPost]  
        public ActionResult TramaConfirmacionO(TramaConfOrdenCargaDescViewModel model)
        {
            try 
            {
                if (ModelState.IsValid)
                { 
                    using (bd_Entities db = new bd_Entities())
                    {
                        var tabla = new Salidas();
                        tabla.Id =  2;
                        tabla.Transaccion =
                            model.Tipo_Transaccion +
                            model.Numero_Operacion +
                            model.Numero_Compartimiento +
                            model.Estado_Transaccion +
                            model.Razones +
                            model.Modulo_Operacion +
                            model.volumen_natural +
                            model.volumen_neto +
                            model.temperatura +
                            model.Codigo_Anterior_producto +
                            model.Fecha_inicio +
                            model.hora_inicio +
                            model.Fecha_fin +
                            model.hora_fin +
                            model.posicion_carga +
                            model.flujo_promedio +
                            model.factor_medicion +
                            model.Codigo_nuevo_producto;

                        db.Salidas.Add(tabla);
                        db.SaveChanges();
                    }

                    return Redirect("about/");
                }
                else 
                {
                    GetCatalogos();
                    GetComportamientos();
                    GetModulo();
                    GetEstadoTran();
                    GetProducto();
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        [HttpPost]
        public ActionResult TramaVolumenTanques(TramaVolumenesTanquesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (bd_Entities db = new bd_Entities())
                    {
                        var tabla = new Salidas();
                        tabla.Id = 3;
                        tabla.Transaccion =
                            model.Tipo_Transaccion +
                            model.numero_tanque +
                            model.codigo_Anterior_producto +
                            model.volumen_total_neto +
                            model.volumen_neto +
                            model.volumen_natural +
                            model.temperatura_promedio +
                            model.fecha +
                            model.Hora +
                            model.codigo_nuevo_producto;

                        db.Salidas.Add(tabla);
                        db.SaveChanges();
                    }

                    return Redirect("about/");
                }
                else
                {
                    GetCatalogos2();
                    GetProducto();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult TramaCancelacionO(TramaCancelacionOrdenesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (bd_Entities db = new bd_Entities())
                    {
                        var tabla = new Salidas();
                        tabla.Id = 4;
                        tabla.Transaccion =
                            model.Tipo_Transaccion +
                            model.Numero_Operacion +
                            model.Numero_Compartimiento +
                            model.Estado_Transaccion +
                            model.Razones +
                            model.Modulo_Operacion +
                            model.Identificador_Vehiculo +
                            model.codigo_Anterior_producto +
                            model.volumen_natural +
                            model.codigo_nuevo_producto;

                        db.Salidas.Add(tabla);
                        db.SaveChanges();
                    }

                    return Redirect("about/");
                }
                else
                {
                    GetCatalogos3();
                    GetComportamientos();
                    GetEstadoTran();
                    GetModulo();
                    GetProducto();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ActionResult InterpretarTramaS(string id)
        {
            salidasViewModel salidasVM = new salidasViewModel();
            string trama = null;
            using (bd_Entities db = new bd_Entities())
            {
                int Id = int.Parse(id);
                var aux = from s in db.Salidas where s.Id == Id select s;
                foreach (var a in aux)
                {
                    salidasVM.salidas.Add(a);
                }
                trama = db.Salidas.Find(int.Parse(id)).Transaccion;
            }

            if (trama.Substring(0,2) == "02" && trama.Substring(8, 1) == "0")
            {
                salidasVM.Trama = new ordenCargaDescargaSalidas();
                salidasVM.Trama = leertramaConfirmada(trama);
            }
            else
            if (trama.Substring(0, 2) == "02" && trama.Substring(8 , 1) == "1")
            {
                salidasVM.Trama3 = new cancelacionOrdenes();
                salidasVM.Trama3 = leerTramaCancelada(trama);
            }
            else
            if (trama.Substring(0, 2) == "04")
            {
                salidasVM.Trama2 = new volumenTanques();
                salidasVM.Trama2= leerTramaVolumenes(trama);
            }

            return View(salidasVM);
        }
        public ordenCargaDescargaSalidas leertramaConfirmada(string Trama)
        {
            ordenCargaDescargaSalidas confirmacion = new ordenCargaDescargaSalidas();

            #region Orden de Carga
            //Tipo de transaccion
            confirmacion.TipoTransaccion = "Confirmacion de carga / descarga";

            //Numero de operacion
            string numeroOperacion = Trama.Substring(2, 5);
            confirmacion.NumeroOperacion = numeroOperacion;

            //Numero de compartimiento 
            string numeroComparimiento = Trama.Substring(7, 1);
            confirmacion.NumeroCopartimiento = numeroComparimiento;

            //Estado de transaccion
            string estadoTransaccion = Trama.Substring(8, 1);
            if (estadoTransaccion == "0")
                confirmacion.EstadoTransaccion = "Terminada";
            else
            if (estadoTransaccion == "1")
                confirmacion.EstadoTransaccion = "Cancelada";

            //Razones de cancelacion 
            string Razon = Trama.Substring(9, 2);
            confirmacion.Razon = Razon;

            //Modulo de operacion
            string moduloOperacion = Trama.Substring(11, 2);
            switch (moduloOperacion)
            {
                case "11":
                    confirmacion.ModuloOperacion = "Descarga de auto-tanques";
                    break;
                case "12":
                    confirmacion.ModuloOperacion = "Descarga de carro-tanques";
                    break;
                case "13":
                    confirmacion.ModuloOperacion = "Descarga de barcos";
                    break;
                case "21":
                    confirmacion.ModuloOperacion = "Carga de auto-tanques";
                    break;
                case "22":
                    confirmacion.ModuloOperacion = "Carga de carro-tanques";
                    break;
                case "23":
                    confirmacion.ModuloOperacion = "Carga de carro-tanques";
                    break;
                case "24":
                    confirmacion.ModuloOperacion = "Movimiento en poliducto";
                    break;
                case "26":
                    confirmacion.ModuloOperacion = "Carga de tambores";
                    break;
                case "27":
                    confirmacion.ModuloOperacion = "Autoconsumo";
                    break;
            }

            //Volumen natural
            string volumenNatural = Trama.Substring(13, 11);
            confirmacion.VolumenNatural = volumenNatural;

            //Volumen neto
            string volumenNeto = Trama.Substring(24, 11);
            confirmacion.VolumenNeto = volumenNeto;

            //Temperatura promedio
            string temperaturaPro = Trama.Substring(35, 5);
            confirmacion.Temperatura = temperaturaPro;

            //Producto anterior
            string productoA = Trama.Substring(40, 4);
            switch (productoA)
            {
                case "0470":
                    confirmacion.CodigoProductoAnterior = "Diesel automotriz";
                    break;
                case "0270":
                    confirmacion.CodigoProductoAnterior = "Premium";
                    break;
                case "0266":
                    confirmacion.CodigoProductoAnterior = "Regular";
                    break;
                default:
                    confirmacion.CodigoProductoAnterior = productoA;
                    break;
            }

            //Fecha Inicio
            string fechaInicio = Trama.Substring(44, 6);
            confirmacion.FechaInicio = fechaInicio;

            //Hora Inicio
            string horaInicio = Trama.Substring(50, 4);
            confirmacion.HoraInicio = horaInicio;

            //Fecha Termino
            string fechaTermino = Trama.Substring(54, 6);
            confirmacion.FechaTermino = fechaTermino;

            //Hora Termino
            string horaTermino = Trama.Substring(60, 4);
            confirmacion.HoraTermino = horaTermino;

            //Posicion carga
            string posicionCarga = Trama.Substring(64, 2);
            confirmacion.PosicionCarga = posicionCarga;

            //Flujo promedio
            string flujoPromedio = Trama.Substring(66, 4);
            confirmacion.FlujoPromedio = flujoPromedio;

            //Factor medicion
            string factorMedicion = Trama.Substring(70, 6);
            confirmacion.FactorMedicion = factorMedicion;

            //Producto nuevo
            string productoNuevo = Trama.Substring(76, 4);
            confirmacion.CodigoProductoNuevo = productoNuevo;
            #endregion

            return confirmacion;
        }        
        public volumenTanques leerTramaVolumenes(string Trama2)
        {
            volumenTanques confirmacion = new volumenTanques();

            #region Orden de Carga
            //Tipo de transaccion
            confirmacion.TipoTransaccion = "Volumen de Tanques";

            //Numero de tanque
            string numeroTanque = Trama2.Substring(2, 3);
            confirmacion.NumeroTanque = numeroTanque;

            //Producto anterior
            string productoA = Trama2.Substring(5, 4);
            switch (productoA)
            {
                case "0470":
                    confirmacion.CodigoProductoAnterior = "Diesel automotriz";
                    break;
                case "0270":
                    confirmacion.CodigoProductoAnterior = "Premium";
                    break;
                case "0266":
                    confirmacion.CodigoProductoAnterior = "Regular";
                    break;
                default:
                    confirmacion.CodigoProductoAnterior = productoA;
                    break;
            }

            //Volumen total neto
            string volumenTotalNeto = Trama2.Substring(9, 11);
            confirmacion.VolumenTotalNeto = volumenTotalNeto;

            //Volumen neto
            string volumenNeto = Trama2.Substring(20, 11);
            confirmacion.VolumenNeto = volumenNeto; 

            //Volumen natural
            string volumenNatural = Trama2.Substring(31, 11);
            confirmacion.VolumenNatural = volumenNatural;

            //Temperatura promedio
            string temperaturaPro = Trama2.Substring(42, 5);
            confirmacion.TemperaturaPromedio = temperaturaPro;

            //Fecha 
            string fecha = Trama2.Substring(47, 6);
            confirmacion.Fecha = fecha;

            //Hora 
            string hora = Trama2.Substring(53, 4);
            confirmacion.Hora = hora;

            //Producto nuevo
            string productoNuevo = Trama2.Substring(57, 4);
            confirmacion.CodigoProductoNuevo = productoNuevo;
            #endregion

            return confirmacion;
        }
        public cancelacionOrdenes leerTramaCancelada(string Trama3)
        {
            cancelacionOrdenes confirmacion = new cancelacionOrdenes();

            #region Orden de Carga
            //Tipo Transaccion
            confirmacion.TipoTransaccion = "Cancelacion de orden de Carga / Descarga";

            //Numero de operacion
            string numeroOperacion = Trama3.Substring(2, 5);
            confirmacion.NumeroOperacion = numeroOperacion;

            //Numero de comportamiento
            string numeroComportamiento = Trama3.Substring(7, 1);
            confirmacion.NumeroCopartimiento = numeroComportamiento;

            //Estado de transaccion
            string estadoTransaccion = Trama3.Substring(8, 1);
            if (estadoTransaccion == "0")
                confirmacion.EstadoTransaccion = "Terminada";
            else
            if (estadoTransaccion == "1")
                confirmacion.EstadoTransaccion = "Cancelada";

            //Razones de cancelacion 
            string Razon = Trama3.Substring(9, 2);
            confirmacion.Razon = Razon;

            //Modulo de operacion
            string moduloOperacion = Trama3.Substring(11, 2);
            switch (moduloOperacion)
            {
                case "11":
                    confirmacion.ModuloOperacion = "Descarga de auto-tanques";
                    break;
                case "12":
                    confirmacion.ModuloOperacion = "Descarga de carro-tanques";
                    break;
                case "13":
                    confirmacion.ModuloOperacion = "Descarga de barcos";
                    break;
                case "21":
                    confirmacion.ModuloOperacion = "Carga de auto-tanques";
                    break;
                case "22":
                    confirmacion.ModuloOperacion = "Carga de carro-tanques";
                    break;
                case "23":
                    confirmacion.ModuloOperacion = "Carga de carro-tanques";
                    break;
                case "24":
                    confirmacion.ModuloOperacion = "Movimiento en poliducto";
                    break;
                case "26":
                    confirmacion.ModuloOperacion = "Carga de tambores";
                    break;
                case "27":
                    confirmacion.ModuloOperacion = "Autoconsumo";
                    break;
            }

            //Identificador del vehiculo
            string identificadorVehiculo = Trama3.Substring(13, 11);
            confirmacion.IdentificacionVehiculo = identificadorVehiculo;

            //Producto anterior
            string productoA = Trama3.Substring(24, 4);
            switch (productoA)
            {
                case "0470":
                    confirmacion.CodigoProductoAnterior = "Diesel automotriz";
                    break;
                case "0270":
                    confirmacion.CodigoProductoAnterior = "Premium";
                    break;
                case "0266":
                    confirmacion.CodigoProductoAnterior = "Regular";
                    break;
                default:
                    confirmacion.CodigoProductoAnterior = productoA;
                    break;
            }

            //Volumen natural
            string volumenNatural = Trama3.Substring(28, 11);
            confirmacion.VolumenNaturalProgramado = volumenNatural;

            //Producto nuevo
            string productoNuevo = Trama3.Substring(39, 4);
            confirmacion.CodigoProductoNuevo = productoNuevo;
            #endregion

            return confirmacion;
        }
    }
} 