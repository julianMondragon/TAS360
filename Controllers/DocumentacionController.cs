using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Filters;

namespace TAS360.Controllers
{
    public class DocumentacionController : Controller
    {
        /// <summary>
        /// Muestra la pagina de los procedimientos para la instalacion y comisionamiento del software TAS360 y VTSCADA
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser(idOperacion: 16)]
        public ActionResult Procedimientos()
        {
            return View();
        }
        /// <summary>
        /// Muestra la pagina de los manuales del TAS360.
        /// </summary>
        /// <returns></returns>
        public ActionResult Manuales()
        {
            return View();
        }
    }
}