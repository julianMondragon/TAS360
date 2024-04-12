using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Controllers
{
    public class DocumentacionController : Controller
    {
        // GET: Documentacion
        public ActionResult Procedimientos()
        {
            return View();
        }

        public ActionResult Manuales()
        {
            return View();
        }
    }
}