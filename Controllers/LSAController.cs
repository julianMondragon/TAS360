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
            GetSummaryTKs();
            return View();
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