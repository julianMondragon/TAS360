using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using TAS360.Filters;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class PendientesController : Controller
    {
        /// <summary>
        ///  Metodo que devuelve a la lista todos los Pendientes que no fueron elimindos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<PendientesViewModel> model = new List<PendientesViewModel>();
            List<CurrentList> currentList = new List<CurrentList>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                //db.Configuration.ProxyCreationEnabled = false;
                var ListPendientes = from p in db.Pendiente where p.is_deleted == false select p;
                if(ListPendientes.Any())
                {
                    foreach(var  p in ListPendientes)
                    {
                        currentList.Add(new CurrentList() { id = p.id , is_selected = false});
                    }
                    foreach (var p in ListPendientes)
                    {
                        model.Add(new PendientesViewModel()
                        {
                            id = p.id,
                            Descripcion = p.Descripcion,
                            Clasificacion_Pendiente = db.Clasificacion_Pendiente.FirstOrDefault(cp => cp.id == p.id_Clasificacion),
                            Responsable = p.Responsable,
                            id_Terminal = (int) p.id_Terminal,
                            Avance = (double)p.Avance,
                            Subsistema = db.Subsistema.FirstOrDefault(cp => cp.id == p.id_Subsistema),
                            currentList = currentList
                        });                        
                    }

                }
                
            }
            return View(model);
        }

        

        /// <summary>
        /// Devuelve a la vista una lista de pendientes con un pendiente seleccionado para mostrar detalles
        /// </summary>
        /// <param name="Current_List"></param>
        /// <returns></returns>
        public ActionResult Details(string encodedCurrentList, int id_pendiente)
        {
            PendientesViewModel model = new PendientesViewModel();
            List<CurrentList> Current_List = new List<CurrentList>();

            var decodedObject = HttpUtility.UrlDecode(encodedCurrentList);
            Current_List = JsonConvert.DeserializeObject<List<CurrentList>>(decodedObject);
            foreach (var item in Current_List)
            {
                item.is_selected = false;
            } 
            Current_List.FirstOrDefault(z => z.id == id_pendiente).is_selected = true;
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var Pendiente = db.Pendiente.Find(Current_List.FirstOrDefault(c => c.is_selected == true).id);
                if (Pendiente != null)
                {
                    model.id = Pendiente.id;
                    model.Descripcion = Pendiente.Descripcion;
                    model.Actividades_Pend_Susess = Pendiente.Actividades_Pend_Susess;
                    model.Observacion = Pendiente.Observacion;
                    model.id_Clasificacion = (int)Pendiente.id_Clasificacion;
                    model.id_Prioridad = (int)Pendiente.id_Prioridad;
                    model.CreatedAt = (DateTime)Pendiente.CreatedAt;
                    model.Fecha_Compromiso = (DateTime)Pendiente.Fecha_Compromiso;
                    if (Pendiente.id_Ticket != null)
                        model.id_Ticket = (int)Pendiente.id_Ticket;
                    model.id_Terminal = (int)Pendiente.id_Terminal;
                    model.id_status = (int)Pendiente.id_status;
                    model.id_Subsistema = (int)Pendiente.id_Subsistema;
                    model.Responsable = Pendiente.Responsable;
                    model.id_User = (int)Pendiente.id_User;
                    model.Avance = (double)Pendiente.Avance;
                    model.Subsistema = Pendiente.Subsistema;
                    model.Terminal = Pendiente.Terminal;
                    model.Clasificacion_Pendiente = Pendiente.Clasificacion_Pendiente;
                    model.User = Pendiente.User;
                    model.Prioridad = Pendiente.Prioridad_de_Pendiente;
                    model.Record_Status = new List<Pendiente_Record_Status>();
                    foreach (var item in Pendiente.Pendiente_Record_Status)
                    {
                        item.Status = db.Status.Find(item.id_Status);
                        model.Record_Status.Add(item);
                    }
                    model.currentList = Current_List;
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
                    ViewBag.InfoMessage = "Pendiente no encontrado";
                    return View(model);
                }

            }

            GetTerminales();
            GetSubsistemas();
            GetPrioridad();
            GetClasificacion();
            GetUsuarios();
            GetTickets();
            GetStatus(1);
            return View(model);
        }

        /// <summary>
        /// Metodo que abre la vista para crear un pendiente
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo que guarda un Pendiente en la BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <example>
        /// Valida que exista un usuario logeado
        /// Asigna al modelo el nombre de responsable de la tabla Users
        /// Llena un nuevo objeto de tipo Pendiente para agregarlo a la BD
        /// llena un nuevo objeto de tipo Pendiente_Record_Status para agregarlo a BD
        /// registra en los logs los nuevos ojetos creados
        /// </example>
        [HttpPost]
        [AuthorizeUser(idOperacion: 11)]
        public ActionResult Create(PendientesViewModel model)
        {
            try
            {
                User user = (User) Session["User"];
                if ( user == null )
                {
                    GetTerminales();
                    GetSubsistemas();
                    GetPrioridad();
                    GetClasificacion();
                    GetUsuarios();
                    GetTickets();
                    GetStatus(1);
                    ViewBag.InfoMessage = "Inicia sesion para crear un pendiente ";
                    return View(model);
                }
                               
                if (ModelState.IsValid)
                {
                    string path = Server.MapPath("~/Logs/Pendientes/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        if (model.id_Ticket != 0)
                        {
                            db.Pendiente.Add(new Pendiente()
                            {
                                Descripcion = model.Descripcion,
                                id_Terminal = model.id_Terminal,
                                id_Clasificacion = model.id_Clasificacion,
                                id_Prioridad = model.id_Prioridad,
                                id_Subsistema = model.id_Subsistema,
                                id_Ticket = model.id_Ticket,
                                id_User = user.id,
                                Actividades_Pend_Susess = model.Actividades_Pend_Susess,
                                Avance = (int)model.Avance,
                                Observacion = model.Observacion,
                                CreatedAt = model.CreatedAt,
                                Fecha_Compromiso = model.Fecha_Compromiso,
                                Responsable = model.Responsable,
                                id_status = model.id_status,
                                is_deleted = false,
                                is_PAS = model.is_PAS,
                                is_PAF = model.is_PAF,
                                version_where_the_Pending_was_found = model.version_where_the_Pending_was_found
                            });
                        }
                        else
                        {
                            db.Pendiente.Add(new Pendiente()
                            {
                                Descripcion = model.Descripcion,
                                id_Terminal = model.id_Terminal,
                                id_Clasificacion = model.id_Clasificacion,
                                id_Prioridad = model.id_Prioridad,
                                id_Subsistema = model.id_Subsistema,
                                id_User = user.id,
                                Actividades_Pend_Susess = model.Actividades_Pend_Susess,
                                Avance = (int)model.Avance,
                                Observacion = model.Observacion,
                                CreatedAt = model.CreatedAt,
                                Fecha_Compromiso = model.Fecha_Compromiso,
                                Responsable = model.Responsable,
                                id_status = model.id_status,
                                is_deleted = false
                            });
                        }
                        
                        //Guarda los cambios en la BD
                        db.SaveChanges();
                        db.Pendiente_Record_Status.Add( new Pendiente_Record_Status()
                        {
                            CreatedAt = model.CreatedAt,
                            id_Status = model.id_status,
                            id_Pendiente = db.Pendiente.FirstOrDefault(z => z.Descripcion == model.Descripcion).id,
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();

                        //Agrega logs
                        oLog.Add("Se agrega nuevo Pendiente por id user: " + user.id + " Con Nombre: " + user.nombre );
                        oLog.Add("Descripcion: " + model.Descripcion);
                        oLog.Add("Terminal: " + model.id_Terminal);
                        oLog.Add("Clasificacion: " + model.id_Clasificacion);
                        oLog.Add("Prioridad: " + model.id_Prioridad);
                        oLog.Add("Subsistema: " + model.id_Subsistema);
                        oLog.Add("Ticket: " + model.id_Ticket);
                        oLog.Add("Responsable: " + model.Responsable);
                        oLog.Add("Actividad Pendiente Susess: " + model.Actividades_Pend_Susess);
                        oLog.Add("Avance: " + model.Avance);
                        oLog.Add("Created At: " + model.CreatedAt.ToString());
                        oLog.Add("Fecha Compromiso: " + model.Fecha_Compromiso.ToString());
                        oLog.Add("Status: " + model.id_status);
                        oLog.Add("version donde fue encontrado: " + model.version_where_the_Pending_was_found);

                        //redirige al listar pendientes
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
                    ViewBag.ExceptionMessage = "Formulario no valido para procesar la solicitud create en PendienteController ";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                GetTerminales();
                GetSubsistemas();
                GetPrioridad();
                GetClasificacion();
                GetUsuarios();
                GetTickets();
                GetStatus(1);
                string path = Server.MapPath("~/Logs/Pendientes/");
                Log oLog = new Log(path);                
                oLog.Add("Metodo POST Create Excepcion: " + ex.Message);
                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
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
