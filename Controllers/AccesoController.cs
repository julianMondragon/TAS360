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
                        ViewBag.exception = "Usuario o contraseña no validos";
                        return View();
                    }
                    Session["User"] = usuario;
                    //Add Log
                    string path = Server.MapPath("~/Logs/");
                    Log oLog = new Log(path);
                    oLog.Add("Ingreso " + usuario.nombre);
                    oLog = null;
                }
                
                return RedirectToAction("home", "Home");
            }
            catch(Exception ex)
            {
                //Add Log
                string path = Server.MapPath("~/Logs/");
                Log oLog = new Log(path);
                oLog.Add("Excepcion en el controllador de acceso");
                oLog.Add(ex.Message);
                oLog = null;
                ViewBag.exception = ex.Message;
                return View();
            }
        }
    }
}