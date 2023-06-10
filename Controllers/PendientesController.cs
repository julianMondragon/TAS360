using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class PendientesController : Controller
    {
        // GET: Pendientes
        public ActionResult Index()
        {
            List<PendientesViewModel> model = new List<PendientesViewModel>();
            #region Ejemplos Temporales 
            Clasificacion_Pendiente CP = new Clasificacion_Pendiente() { id = 1, Clave = "H", Nombre = "Alta", };
            User user = new User() { id = 1, nombre = "Carter" };
            Subsistema sub = new Subsistema() { id = 1, Nombre = "Carga" };
            
            //Ejemplos
            model.Add(new Models.ViewModel.PendientesViewModel()
            {
                
                Descripcion = "Pendiente de Prueba",
                id_Clasificacion = 3,
                id_Prioridad = 1,
                id_Subsistema = 2,
                id_Terminal = 2,
                id_User = 1,
                Avance = 60,
                Actividades_Pend_Susess = "titipuchales de codigo",
                CreatedAt = DateTime.Now,
                Fecha_Compromiso = DateTime.Now.AddDays(5),
                Observacion = "Mas codigo por desarrollar",
                Responsable = "Tomas Gaytan",
                Clasificacion_Pendiente = CP,
                User = user,
                Subsistema = sub
            });
            model.Add(new Models.ViewModel.PendientesViewModel()
            {
                Descripcion = "Pendiente de Prueba",
                id_Clasificacion = 3,
                id_Prioridad = 1,
                id_Subsistema = 2,
                id_Terminal = 2,
                id_User = 1,
                Avance = 60,
                Actividades_Pend_Susess = "titipuchales de codigo",
                CreatedAt = DateTime.Now,
                Fecha_Compromiso = DateTime.Now.AddDays(5),
                Observacion = "Mas codigo por desarrollar",
                Responsable = "Tomas Gaytan",
                Clasificacion_Pendiente = CP,
                User = user,
                Subsistema = sub
            });
            model.Add(new Models.ViewModel.PendientesViewModel()
            {
                
                Descripcion = "Pendiente de Prueba",
                id_Clasificacion = 3,
                id_Prioridad = 1,
                id_Subsistema = 2,
                id_Terminal = 2,
                id_User = 1,
                Avance = 60,
                Actividades_Pend_Susess = "titipuchales de codigo",
                CreatedAt = DateTime.Now,
                Fecha_Compromiso = DateTime.Now.AddDays(5),
                Observacion = "Mas codigo por desarrollar",
                Responsable = "Tomas Gaytan",
                Clasificacion_Pendiente = CP,
                User = user,
                Subsistema = sub
            });
            #endregion Temporal

            return View(model);
        }

        // GET: Pendientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pendientes/Create
        [HttpGet]
        public ActionResult Create()
        {
            PendientesViewModel model = new PendientesViewModel();
            model.id_Clasificacion = 3;
            model.id_Prioridad = 2;
            model.CreatedAt = DateTime.Now;
            model.Fecha_Compromiso = DateTime.Now.AddDays(15);
            GetTerminales();
            GetSubsistemas();
            GetPrioridad();
            GetClasificacion();
            GetUsuarios();
            GetTickets();
            GetStatus(1);
            return View(model);
        }

        // POST: Pendientes/Create
        [HttpPost]
        public ActionResult Create(PendientesViewModel model)
        {
            try
            {
                // TODO: 
                if (ModelState.IsValid)
                {
                    string path = Server.MapPath("~/Logs/Pendientes/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    GetTerminales();
                    GetSubsistemas();
                    GetPrioridad();
                    GetClasificacion();
                    GetUsuarios();
                    GetTickets();
                    GetStatus(1);
                    return View(model);
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: Pendientes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pendientes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pendientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pendientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Devuelve a la vista una lista de las clasificaciones de los pendientes
        /// </summary>
        private void GetClasificacion()
        {
            List<SelectListItem> Clasificacion = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Clasificacion_Pendiente select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Clasificacion.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Clasificacion = Clasificacion;
        }

        /// <summary>
        /// Devuelve a la vista una lista de las Terminales del ticket 
        /// </summary>
        private void GetTerminales()
        {

            List<SelectListItem> Terminales = new List<SelectListItem>();
            Terminales.Add(new SelectListItem
            {
                Text = "Seleccione una Terminal",
                Value = "09",
                Selected = true
            });

            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Terminal select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Terminales.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Terminales = Terminales;

        }
        /// <summary>
        /// Devuelve a la vista una lista de los usuarios especialistas tecnicos 
        /// </summary>
        private void GetUsuarios()
        {

            List<SelectListItem> Usuarios = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.User select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Usuarios.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Usuarios = Usuarios;

        }

        /// <summary>
        /// Devuelve a la vista una lista de los status
        /// </summary>
        private void GetStatus(int status)
        {

            List<SelectListItem> Status = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Status select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        if (a.Status1 == status)
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Selected = true
                                //Disabled = true                                
                            });
                        }
                        else
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Disabled = true
                            }); 
                        }
                    }
                }
            }
            ViewBag.Status = Status;

        }

        /// <summary>
        /// Devuelve a la vista una lista de las prioridades
        /// </summary>
        private void GetPrioridad()
        {
            List<SelectListItem> Prioridades = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Prioridad_de_Pendiente select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Prioridades.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()
                        });

                    }
                }
            }
            ViewBag.Prioridad = Prioridades;
        }

        /// <summary>
        /// Devuelve a la vista una lista de los Subsistemas
        /// </summary>
        private void GetSubsistemas()
        {

            List<SelectListItem> Subsistemas = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Subsistema select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Subsistemas.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()
                        });

                    }
                }
            }
            ViewBag.Subsistemas = Subsistemas;

        }

        /// <summary>
        /// Devuelve a la vista una lista de los tickets
        /// </summary>
        private void GetTickets()
        {
            List<SelectListItem> Tickets = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                Tickets.Add(new SelectListItem
                {
                    Text = "Sin ticket asignado",
                    Value = "0",
                    Selected = true

                });
                var aux = (from s in db.Ticket where s.status != 12  select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Tickets.Add(new SelectListItem
                        {
                            Text = a.titulo,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Tickets = Tickets;
        }


    }
}
