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
            aditivo.Tag = "TH-13";
            aditivo.NombreAditivo = "TESORO";
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
            var volumen = rnd.Next(1000, 23000);
            var Nivel = rnd.Next(10);
            string connectionString = "Data source=DESKTOP-RBH8FQ1;initial catalog=IMPDG15;integrated security=True;";
            string query = $"INSERT INTO VolTanq (Fecha_y_Hora , Nivel_Volumetrico) VALUES ('{Fecha_y_Hora}','{volumen}')";
            string query1 = $"INSERT INTO NivTanq (Fecha_y_Hora , Nivel_Tanque_Nivel_Value) VALUES ('{Fecha_y_Hora}','{Nivel}')";
            CreateCommand(query,connectionString);
            CreateCommand(query1, connectionString);
            #endregion

            return Redirect(Url.Content("~/Aditivo"));
        }

        public ActionResult VolTotalizado()
        {
            RepTotalizadoViewModel model = new RepTotalizadoViewModel();

            using (Aditivo_Entities db = new Aditivo_Entities())
            {
                var ucl1 = db.TotIny9.OrderByDescending(x => x.Fecha_y_Hora).Take(50);
                var ucl2 = db.TotIny8.OrderByDescending(x => x.Fecha_y_Hora).Take(50);
                var ucl3 = db.TotIny7.OrderByDescending(x => x.Fecha_y_Hora).Take(50);

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
            }

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
    }
}