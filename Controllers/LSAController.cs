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
                        tickets.Add(new TicketViewModel
                        {
                            id = t.id,
                            titulo = t.titulo,
                            mensaje = t.mensaje,
                            usuario_name = t.Ticket_User.OrderByDescending(x => x.CreatedAt).FirstOrDefault().User.nombre,
                            categoria_name = t.Categoria.nombre,
                            terminal_name = t.Terminal.Nombre,                            
                            status_name = t.Ticket_Record_Status.OrderByDescending(x => x.CreatedAt).FirstOrDefault().Status.descripcion,
                            Subsistema_name = t.Subsistema.Nombre,
                            Status = t.status,
                            Date = t.CreatedAt,
                            Datetobedone = t.CreatedAt.HasValue ? t.CreatedAt.Value.AddDays(15) : DateTime.MinValue
                        });

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