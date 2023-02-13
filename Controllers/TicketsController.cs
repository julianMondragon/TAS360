using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Filters;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class TicketsController : Controller
    {
        // GET: Tickets
        [AuthorizeUser(idOperacion:5)]
        public ActionResult Index()
        {
              List<TicketViewModel> tickets = new List<TicketViewModel>();
            using (Models.HelpDesk_Entities1 Ticket = new Models.HelpDesk_Entities1())
            {
                var Tickets = (from s in Ticket.Ticket select s);
                if (Tickets != null && Tickets.Any())
                {
                    foreach (var t in Tickets)
                    {
                        tickets.Add(new TicketViewModel
                        {
                            id = t.id,
                            titulo = t.titulo,
                            mensaje = t.mensaje,
                            id_Usuario = t.id_User,
                            id_Categoria = t.id_Categoria,
                            Status = t.status
                        });
                    }
                }
            }
            return View(tickets);
        }

        /// <summary>
        /// Abre la vista para crear un nuevo un ticket y obtiene los catalogos necesarios para relacionarlo.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(idOperacion: 1)]
        public ActionResult CreateTicket()
        {
            TicketViewModel NewTicket = new TicketViewModel();
            GetCategories();
            GetTerminales();
            GetUsuarios();
            GetStatus();
            return View(NewTicket);
        }

        /// <summary>
        /// Crea un nuevo registro en la tabla Ticket 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTicket(TicketViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        Ticket ticket = new Ticket();
                        ticket.titulo = model.titulo;
                        ticket.id_Terminal = model.id_Terminal;
                        ticket.id_Categoria = model.id_Categoria;
                        ticket.status = model.Status;
                        ticket.mensaje = model.mensaje;

                        //db.Ticket.Add(ticket);
                        //db.SaveChanges();
                    }

                    return Redirect("~/Tickets/Index");
                }
                else
                {
                    GetCategories();
                    GetTerminales();
                    GetUsuarios();
                    GetStatus();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                GetCategories();
                GetTerminales();
                GetUsuarios();
                GetStatus();
                ViewBag.ExceptionMessage = ex.Message;
                return View(model);

            }
        }

        /// <summary>
        /// Devuelve a la vista una lista de las categorias del ticket 
        /// </summary>
        private void GetCategories()
        {
            List<SelectListItem> Categorias = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1 ())
            {
                var aux = (from s in db.Categoria select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Categorias.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()
                            
                        });
                    }
                }
            }
            ViewBag.Categorias = Categorias;
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
                Value = "08",
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
            Usuarios.Add(new SelectListItem
            {
                Text = "Seleccione un Usuario",
                Value = "01",
                Selected = true
            });

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
        private void GetStatus()
        {

            List<SelectListItem> Status = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Status select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        if ( a.descripcion.Contains("Pendiente"))
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Selected = true

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
    }
}