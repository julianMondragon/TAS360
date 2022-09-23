using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class AditivoController : Controller
    {
        // GET: Aditivo
        public ActionResult Index()
        {
            
            aditivoViewModel aditivo = new aditivoViewModel();
            aditivo.Tag = "TH-01";
            aditivo.NombreAditivo = "IMPDG15";
            using(Aditivo_Entities db = new Aditivo_Entities())
            {
                var aux = db.VolTanq.OrderByDescending(a => a.Fecha_y_Hora).FirstOrDefault();
                if(aux != null)
                {
                    aditivo.Volumen = aux.Nivel_Volumetrico;
                }
                else
                {
                    aditivo.Volumen = 0;
                }
                var aux1 = db.NivTanq.OrderByDescending(a => a.Fecha_y_Hora).FirstOrDefault();
                if (aux1 != null)
                {
                    aditivo.Nivel = aux1.Nivel_Tanque_Nivel_Value;
                }
                else
                {
                    aditivo.Nivel = 0;
                }
            }
            return View(aditivo);
        }

        public ActionResult ActualizarDatos()
        {
            #region EntityFramework
            //using(Aditivo_Entities db = new Aditivo_Entities())
            //{
            //    //volumen
            //    Random rnd = new Random();
            //    VolTanq volumen = new VolTanq()
            //    {
            //        Fecha_y_Hora = DateTime.Now,
            //        Nivel_Volumetrico = rnd.Next(1000,23000)
            //    };
            //    db.VolTanq.Add(volumen);
            //    //nivel
            //    NivTanq nivel = new NivTanq()
            //    {
            //        Fecha_y_Hora = DateTime.Now,
            //        Nivel_Tanque_Nivel_Value = rnd.Next(10)
            //    };
            //    db.NivTanq.Add(nivel);
            //    db.SaveChanges();
            //}
            #endregion

            #region ADO.NET
            var Fecha_y_Hora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Random rnd = new Random();
            double volumen = rnd.Next(1000, 23000);
            if(volumen > 11.4609375)
            {
                volumen = volumen - 11.4609375;
            }
            else
            {
                volumen = volumen + 11.4609375;
            }
            double  Nivel = rnd.Next(10);
            string connectionString = "Data source=DESKTOP-RBH8FQ1;initial catalog=IMPDG15;integrated security=True;";
            string query = $"INSERT INTO VolTanq (Fecha_y_Hora , Nivel_Volumetrico) VALUES ('{Fecha_y_Hora}','{volumen}')";
            string query1 = $"INSERT INTO NivTanq (Fecha_y_Hora , Nivel_Tanque_Nivel_Value) VALUES ('{Fecha_y_Hora}','{Nivel}')";
            CreateCommand(query,connectionString);
            CreateCommand(query1, connectionString);
            #endregion

            return Redirect(Url.Content("~/Aditivo/Index"));
        }

        public ActionResult VolTotalizado()
        {
            RepTotalizadoViewModel model = new RepTotalizadoViewModel();
            DateTime fecha_inicio = new DateTime(2020, 10, 20, 05, 00, 00);
            DateTime fecha_Fin = new DateTime(2020, 10, 21, 05, 00, 00);


            using (Aditivo_Entities db = new Aditivo_Entities())
            {
                
                var ucl9 = db.TotIny14.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl8 = db.TotIny13.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl7 = db.TotIny12.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl6 = db.TotIny11.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl5 = db.TotIny10.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl1 = db.TotIny9.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin );
                var ucl2 = db.TotIny8.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl3 = db.TotIny7.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);
                var ucl4 = db.TotIny6.Where(x => x.Fecha_y_Hora >= fecha_inicio && x.Fecha_y_Hora <= fecha_Fin);

                foreach (var ucl in ucl1)
                {
                    model.Ucls.Ucl1.Add(ucl);
                }
                foreach (var ucl in ucl2)
                {
                    model.Ucls.Ucl2.Add(ucl);
                }
                foreach (var ucl in ucl3)
                {
                    model.Ucls.Ucl3.Add(ucl);
                }
                foreach (var ucl in ucl4)
                {
                    model.Ucls.Ucl4.Add(ucl);
                }
                foreach (var ucl in ucl5)
                {
                    model.Ucls.Ucl5.Add(ucl);
                }
                foreach (var ucl in ucl6)
                {
                    model.Ucls.Ucl6.Add(ucl);
                }
                foreach (var ucl in ucl7)
                {
                    model.Ucls.Ucl7.Add(ucl);
                }
                foreach (var ucl in ucl8)
                {
                    model.Ucls.Ucl8.Add(ucl);
                }
                foreach (var ucl in ucl9)
                {
                    model.Ucls.Ucl9.Add(ucl);
                }


                #region Salidas totales

                double? Sumatoria = 0;
                var Total_Ucl6  = db.TotIny6.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl7  = db.TotIny7.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl8  = db.TotIny8.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_Ucl9  = db.TotIny9.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl10 = db.TotIny10.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl11 = db.TotIny11.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_Ucl12 = db.TotIny12.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl13 = db.TotIny13.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                var Total_ucl14 = db.TotIny14.Where(x => x.Fecha_y_Hora == fecha_inicio || x.Fecha_y_Hora == fecha_Fin).ToList();
                if(Total_Ucl6 != null && Total_Ucl6.Any())
                {
                    Sumatoria += Total_Ucl6[1].Isla_6_Totalizador_de_Aditivo - Total_Ucl6[0].Isla_6_Totalizador_de_Aditivo;                 
                }
                if (Total_ucl7 != null && Total_ucl7.Any())
                {
                    Sumatoria += Total_ucl7[1].Isla_7_Totalizador_de_Aditivo - Total_ucl7[0].Isla_7_Totalizador_de_Aditivo;
                }
                if (Total_ucl8 != null && Total_ucl8.Any())
                {
                    Sumatoria += Total_ucl8[1].Isla_8_Totalizador_de_Aditivo - Total_ucl8[0].Isla_8_Totalizador_de_Aditivo;
                }
                if (Total_Ucl9 != null && Total_Ucl9.Any())
                {
                    Sumatoria += Total_Ucl9[1].Isla_9_Totalizador_de_Aditivo - Total_Ucl9[0].Isla_9_Totalizador_de_Aditivo;
                }
                if (Total_ucl10 != null && Total_ucl10.Any())
                {
                    Sumatoria += Total_ucl10[1].Isla_10_Totalizador_de_Aditivo - Total_ucl10[0].Isla_10_Totalizador_de_Aditivo;
                }
                if (Total_ucl11 != null && Total_ucl11.Any())
                {
                    Sumatoria += Total_ucl11[1].Isla_11_Totalizador_de_Aditivo - Total_ucl11[0].Isla_11_Totalizador_de_Aditivo;
                }
                if (Total_Ucl12 != null && Total_Ucl12.Any())
                {
                    Sumatoria += Total_Ucl12[1].Isla_12_Totalizador_de_Aditivo - Total_Ucl12[0].Isla_12_Totalizador_de_Aditivo;
                }
                if (Total_ucl13 != null && Total_ucl13.Any())
                {
                    Sumatoria += Total_ucl13[1].Isla_13_Totalizador_de_Aditivo - Total_ucl13[0].Isla_13_Totalizador_de_Aditivo;
                }
                if (Total_ucl14 != null && Total_ucl14.Any())
                {
                    Sumatoria += Total_ucl14[1].Isla_14_Totalizador_de_Aditivo - Total_ucl14[0].Isla_14_Totalizador_de_Aditivo;
                }
                #endregion
                
                model.Balance.Existencia_inicial = db.VolTanq.FirstOrDefault(x => x.Fecha_y_Hora == fecha_inicio).Nivel_Volumetrico;
                model.Balance.ExistenciaFinal = db.VolTanq.FirstOrDefault(x => x.Fecha_y_Hora == fecha_Fin).Nivel_Volumetrico;
                model.Balance.Nivel_Inicial = db.NivTanq.FirstOrDefault(x => x.Fecha_y_Hora == fecha_inicio).Nivel_Tanque_Nivel_Value;
                model.Balance.Nivel_Final = db.NivTanq.FirstOrDefault(x => x.Fecha_y_Hora == fecha_Fin).Nivel_Tanque_Nivel_Value;
                model.Balance.Resta_de_volumen_Tanq = model.Balance.Existencia_inicial - model.Balance.ExistenciaFinal;
                model.Balance.Salidas = Sumatoria;
                model.Balance.Sobrante_Faltante = model.Balance.Resta_de_volumen_Tanq - Sumatoria;
            }
             SalidasTotalesxUCLs(model);

            return View(model); 
        }

        public ActionResult ListarRecetas()
        {
            List<RecetasViewModel> recetas = new List<RecetasViewModel>();
            using(bdSimcot_Entities db = new bdSimcot_Entities())
            {
                var aux = db.Recetas;
                if(aux != null && aux.Any())
                {
                    foreach (var u in aux)
                    {
                        RecetasViewModel receta = new RecetasViewModel()
                        {
                            Id = u.Id,
                            Name = u.Nombre,
                            Producto = u.Producto,
                            Porc_producto = u.Porcentaje_producto,
                            Brazo = u.Brazo,
                            Cantidad_Aditivo = u.Cantidad_aditivo,
                            Razon_flujo = u.Razon_flujo,
                            Prod_Usando_iny = u.Prod_usando_iny
                        };
                        recetas.Add(receta);
                    }
                }
                else
                {
                    recetas.Add(new RecetasViewModel()
                    {
                        Name = "No exiten ",
                        Producto = "recetas actualmente",
                        Porc_producto = 0,
                        Brazo = 0,
                        Cantidad_Aditivo = 0,
                        Razon_flujo = 0,
                        Prod_Usando_iny = 0
                     });
                }
            }

            return View(recetas);
        }
        
        [HttpGet]
        public ActionResult AgregarReceta()
        {
            GetCatalogos();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarReceta(RecetasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //if (model.Prod_Usando_iny)
            //{
            //    ViewBag.Errormessage = "El campo debe ser numerico";
            //    return View(model);
            //}
            using (bdSimcot_Entities db = new bdSimcot_Entities())
            {
                var cont = db.Recetas.OrderByDescending(x=> x.Id).FirstOrDefault().Id;
                cont++;
                var receta = new Recetas();
                receta.Id = cont;
                receta.Nombre = model.Name;
                receta.Producto = model.Producto;
                receta.Brazo = model.Brazo;
                receta.Porcentaje_producto = model.Porc_producto;
                receta.Cantidad_aditivo = model.Cantidad_Aditivo;
                receta.Razon_flujo = model.Razon_flujo;
                receta.Prod_usando_iny = model.Prod_Usando_iny;

                db.Recetas.Add(receta);
                db.SaveChanges();
            }
            return Redirect(Url.Content("~/Aditivo/ListarRecetas"));
        }

        public ActionResult EditReceta(int Id)
        {
            RecetasViewModel model = new RecetasViewModel();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            GetCatalogos();
            using (bdSimcot_Entities db = new bdSimcot_Entities())
            {
                var receta = db.Recetas.Find(Id);

                model.Id = receta.Id;
                model.Name = receta.Nombre;
                model.Producto = receta.Producto;
                model.Brazo = receta.Brazo;
                model.Porc_producto = receta.Porcentaje_producto;
                model.Cantidad_Aditivo = receta.Cantidad_aditivo;
                model.Razon_flujo = receta.Razon_flujo;
                model.Prod_Usando_iny = receta.Prod_usando_iny; 
               

            }

            return View(model);
        }
        [HttpPost]
        public ActionResult EditReceta(RecetasViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (bdSimcot_Entities db = new bdSimcot_Entities())
            {
                var receta = db.Recetas.Find(model.Id);

                receta.Nombre = model.Name;
                receta.Producto = model.Producto;
                receta.Porcentaje_producto =model.Porc_producto;
                receta.Brazo = model.Brazo;
                receta.Cantidad_aditivo = model.Cantidad_Aditivo;
                receta.Razon_flujo = model.Razon_flujo;
                receta.Prod_usando_iny = model.Prod_Usando_iny;
               
                db.Entry(receta).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect(Url.Content("~/Aditivo/ListarRecetas"));
        }

        private void GetCatalogos()
        {

            List<SelectListItem> ListProductos = new List<SelectListItem>();
            ListProductos.Add(new SelectListItem
            {
                Text = "Seleccione producto",
                Value = "00"
            });
            ListProductos.Add(new SelectListItem
            {
                Text = "Regular",
                Value = "Regular"
            });
            ListProductos.Add(new SelectListItem
            {
                Text = "Premium",
                Value = "Premium"
            });
            ListProductos.Add(new SelectListItem
            {
                Text = "Turbosina",
                Value = "Turbosina"
            });
            ViewBag.ListProductos = ListProductos;

        }
        private void CreateCommand(string queryString,string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                                       connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SalidasTotalesxUCLs(RepTotalizadoViewModel model)
        {

            int count = model.Ucls.Ucl1.Count() - 1;
            double? premiun = 0;
            double? regular = 0;
            double? valor_inical1 = model.Ucls.Ucl1[0].Isla_9_Totalizador_de_Aditivo;
            double? valor_final1 = model.Ucls.Ucl1[count].Isla_9_Totalizador_de_Aditivo;
            double? valor_inical2 = model.Ucls.Ucl2[0].Isla_8_Totalizador_de_Aditivo;
            double? valor_final2 = model.Ucls.Ucl2[count].Isla_8_Totalizador_de_Aditivo;
            double? valor_inical3 = model.Ucls.Ucl3[0].Isla_7_Totalizador_de_Aditivo;
            double? valor_final3 = model.Ucls.Ucl3[count].Isla_7_Totalizador_de_Aditivo;
            double? valor_inical4 = model.Ucls.Ucl4[0].Isla_6_Totalizador_de_Aditivo;
            double? valor_final4 = model.Ucls.Ucl4[count].Isla_6_Totalizador_de_Aditivo;
            double? valor_inical5 = model.Ucls.Ucl5[0].Isla_10_Totalizador_de_Aditivo;
            double? valor_final5 = model.Ucls.Ucl5[count].Isla_10_Totalizador_de_Aditivo;
            double? valor_inical6 = model.Ucls.Ucl6[0].Isla_11_Totalizador_de_Aditivo;
            double? valor_final6 = model.Ucls.Ucl6[count].Isla_11_Totalizador_de_Aditivo;
            double? valor_inical7 = model.Ucls.Ucl7[0].Isla_12_Totalizador_de_Aditivo;
            double? valor_final7 = model.Ucls.Ucl7[count].Isla_12_Totalizador_de_Aditivo;
            double? valor_inical8 = model.Ucls.Ucl8[0].Isla_13_Totalizador_de_Aditivo;
            double? valor_final8 = model.Ucls.Ucl8[count].Isla_13_Totalizador_de_Aditivo;
            double? valor_inical9 = model.Ucls.Ucl9[0].Isla_14_Totalizador_de_Aditivo;
            double? valor_final9 = model.Ucls.Ucl9[count].Isla_14_Totalizador_de_Aditivo;

            if (valor_inical1.HasValue && valor_final1.HasValue)
            {
                ViewBag.Total1 = valor_final1 - valor_inical1;
                regular += valor_final1 - valor_inical1;
            }
            if (valor_inical2.HasValue && valor_final2.HasValue)
            {
                ViewBag.Total2 = valor_final2 - valor_inical2;
                regular += valor_final2 - valor_inical2;
            }
            if (valor_inical3.HasValue && valor_final3.HasValue)
            {
                double? aux = valor_final3 - valor_inical3;
                if (aux != 0)
                {
                    ViewBag.Total3 = aux;
                    regular += aux;
                }                   
                else
                    ViewBag.Total3 = 0;
            }
            if (valor_inical4.HasValue && valor_final4.HasValue)
            {
                ViewBag.Total4 = valor_final4 - valor_inical4;
                regular += valor_final4 - valor_inical4;
            }
            if (valor_inical5.HasValue && valor_final5.HasValue)
            {
                ViewBag.Total5 = valor_final5 - valor_inical5;
                regular += valor_final5 - valor_inical5;
            }
            if (valor_inical6.HasValue && valor_final6.HasValue)
            {
                double? aux = valor_final6 - valor_inical6;
                if (aux != 0)
                {
                    ViewBag.Total6 = aux;
                    regular += aux;
                }                    
                else
                    ViewBag.Total6 = 0;
            }
            if (valor_inical7.HasValue && valor_final7.HasValue)
            {
                ViewBag.Total7 = valor_final7 - valor_inical7;
                regular += valor_final7 - valor_inical7;
            }
            if (valor_inical8.HasValue && valor_final8.HasValue)
            {
                ViewBag.Total8 = valor_final8 - valor_inical8;
                premiun += valor_final8 - valor_inical8;
            }
            if (valor_inical9.HasValue && valor_final9.HasValue)
            {
                double? aux = valor_final9 - valor_inical9;
                if (aux != 0)
                {
                    ViewBag.Total9 = aux;
                    premiun += aux;
                }                    
                else
                    ViewBag.Total9 = 0;
            }

            ViewBag.Regular = regular;
            ViewBag.Premium = premiun;
        }

        // GET: Aditivo
        public ActionResult ConfiguracionAditivo()
        {
            return View();
        }
    }
}