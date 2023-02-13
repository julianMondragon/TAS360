using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;

namespace TAS360.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        // POST: Acceso
        [HttpPost]
        public ActionResult Login(string user , string pass)
        {
            try
            {
                using(HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var usuario = (from u in db.User where u.email == user && u.password == pass select u).FirstOrDefault();
                    if(usuario == null)
                    {
                        ViewBag.Error = "Usuario o contraseña no validos";
                        return View();
                    }
                    Session["User"] = usuario;
                }

                return RedirectToAction("home", "Home");
            }
            catch(Exception ex)
            {
                ViewBag.exception = ex.Message;
                return View();
            }
        }
    }
}