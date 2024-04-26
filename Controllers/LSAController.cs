using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;
using TAS360.StorProc;

namespace TAS360.Controllers
{
    public class LSAController : Controller
    {
        // GET: LSA
        public ActionResult Index()
        {
            List<TicketViewModel> tickets = new List<TicketViewModel>();
            using (Models.HelpDesk_Entities1 db = new Models.HelpDesk_Entities1())
            {
                var Tickets = (from s in db.Ticket where s.status != 12 orderby s.CreatedAt descending select s);
                if (Tickets != null && Tickets.Any())
                {
                    foreach (var t in Tickets)
                    {
                        TicketViewModel ticket = new TicketViewModel()
                        {
                            id = t.id,
                            titulo = t.titulo,
                            mensaje = t.mensaje,
                            usuario_name = t.Ticket_User.OrderByDescending(x => x.CreatedAt).FirstOrDefault().User.nombre,
                            categoria_name = t.Categoria.nombre,
                            terminal_name = t.Terminal.Nombre,                            
                            Subsistema_name = t.Subsistema.Nombre,
                            Status = t.status,
                            Date = t.CreatedAt,
                            Datetobedone = t.CreatedAt.HasValue ? t.CreatedAt.Value.AddDays(15) : DateTime.MinValue
                        };

                        switch (t.Ticket_Record_Status.OrderByDescending(x => x.CreatedAt).FirstOrDefault().Status.descripcion)
                        {
                            case "Pendiente ":
                                ticket.status_name = "Capturado";
                                break;
                            case "Analisis  ":
                                ticket.status_name = "Espera de info";
                                break;
                            case "Correccion":
                                ticket.status_name = "En Proceso";
                                break;
                            case "Pruebas   ":
                                ticket.status_name = "En Proceso";
                                break;
                            case "Implementa":
                                ticket.status_name = "En Proceso";
                                break;
                            case "Pend_Pmx  ":
                                ticket.status_name = "Espera de info";
                                break;
                            case "Cerrado   ":
                                ticket.status_name = "Espera de info";
                                break;
                            default:
                                ticket.status_name = "Undefineded";
                                break;
                        }

                        tickets.Add(ticket);
                    }
                }
            }
            GetSummaryTKs();
            return View(tickets);
        }
        [HttpGet]
        public JsonResult GetTicktsByStatus()
        {
            LSA_Reportes Tickets = new LSA_Reportes();
            List<G_TicketsByStatusViewModel> listTksbyStatus = Tickets.GetTicktsByStatus();

            return Json(listTksbyStatus, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTicketsByCategoria()
        {
            LSA_Reportes Tickets = new LSA_Reportes();
            List<G_TicketsByCategoriaViewModel> listTksbyCategoria = Tickets.GetTicketsByCategoria();

            return Json(listTksbyCategoria, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTicketsByTerminal()
        {
            LSA_Reportes Tickets = new LSA_Reportes();
            List<G_TicketsByTerminalViewModel> listTksbyCategoria = Tickets.GetTicketsByTerminal();

            return Json(listTksbyCategoria, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTicketsByTerminalOnLastMonth()
        {
            LSA_Reportes Tickets = new LSA_Reportes();
            List<G_TicketsByTerminalViewModel> listTksbyCategoria = Tickets.GetTicketsByTerminalOnLastMonth();

            return Json(listTksbyCategoria, JsonRequestBehavior.AllowGet);
        }
        private void GetSummaryTKs()
        {
            int tksopen = 0;
            int tksclose = 0;
            List<TicketViewModel> tickets = new List<TicketViewModel>();
            using (Models.HelpDesk_Entities1 db = new Models.HelpDesk_Entities1())
            {
                var Tickets = (from s in db.Ticket select s);
                if (Tickets != null && Tickets.Any())
                {
                    foreach (var t in Tickets)
                    {
                        if (t.status == 12)
                            tksclose++;
                        else
                            tksopen++;

                    }
                }
            }
            ViewBag.tksopen = tksopen;
            ViewBag.tksclose = tksclose;
        }
    }
}