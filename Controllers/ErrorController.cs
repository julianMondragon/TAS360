using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HttpGet]
        public ActionResult UnauthorizedOperation(string operacion, string modulo, string message)
        {
            var array = operacion.Split('?');
            ViewBag.operacion = array[0];
            ViewBag.modulo = array[1].Replace("modulo=", "");
            ViewBag.message = array[2].Replace("message=", ""); ;
            return View();
        }
    }
}