using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;
using TAS360.Models;
using DocumentFormat.OpenXml.EMMA;

namespace TAS360.Controllers
{
    public class TerminalController : Controller
    {
        /// <summary>
        /// Muestra un listado de las terminales actuales en el sistema
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<TerminalViewModel> model = new List<TerminalViewModel>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                //db.Configuration.ProxyCreationEnabled = false;
                var ListTerminales = from t in db.Terminal select t;
                if (ListTerminales.Any())
                {
                    foreach (var p in ListTerminales)
                    {
                        model.Add(new TerminalViewModel()
                        {
                            id = p.id,
                            Nombre = p.Nombre,
                            clave = p.clave,
                            
                        });
                    }

                }

            }
            return View(model);
        }

        // GET: Terminal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Terminal/Create
        public ActionResult Create()
        {
            TerminalViewModel terminal = new TerminalViewModel();
            return View(terminal);
        }

        // POST: Terminal/Create
        [HttpPost]
        public ActionResult Create(TerminalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = (User)Session["User"];
                    if (user == null)
                    {
                        ViewBag.InfoMessage = "Inicia sesion para crear un pendiente ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Logs/Terminal/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {

                        db.Terminal.Add(new Terminal()
                        {
                            Nombre = model._Nombre,
                            clave = model._Clave
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();

                        //Agrega logs
                        oLog.Add("Se agrega nueva Terminal  por id user: " + user.id + " Con Nombre: " + user.nombre);
                        oLog.Add("Nombre: " + model.Nombre);
                        oLog.Add("_Nombre: " + model._Nombre);
                        oLog.Add("Clave: " + model.clave);
                        oLog.Add("_Clave: " + model._Clave);
                    }
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Terminal/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            TerminalViewModel model = new TerminalViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var TerminalToEdit = db.Terminal.Find(id);
                if(TerminalToEdit != null)
                {
                    model._id = TerminalToEdit.id;
                    model._Nombre = TerminalToEdit.Nombre;
                    model._Clave = TerminalToEdit.clave;
                }
            }
            return View(model);
        }

        // POST: Terminal/Edit/5
        [HttpPost]
        public ActionResult Edit(TerminalViewModel model)
        {
            
            try
            {
                string path = Server.MapPath("~/Logs/Terminal/");
                Log oLog = new Log(path);
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var TerminalToEdit = db.Terminal.Find(model._id);
                    if (TerminalToEdit != null)
                    {
                        TerminalToEdit.Nombre = model._Nombre;
                        TerminalToEdit.clave = model._Clave;
                    }
                    else
                    {
                        ViewBag.ExceptionMessage = "Not Fount ID";
                        return View(model);
                    }
                    
                    //Guarda cambios
                    db.Entry(TerminalToEdit).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //agrega logs
                    oLog.Add("Se guardan cambios en el Terminal");
                    oLog.Add("Nuevo Nombre: " + model._Nombre);
                    oLog.Add("Nuevo clave: " + model._Clave);
                    if((User)Session["User"] != null)
                            oLog.Add("Usuario que modificó: " + ((User)Session["User"]).nombre);

                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~/Logs/Terminal/");
                Log oLog = new Log(path);
                oLog.Add("Catched Excepcion: Get Metod Edit : " + ex.Message);
                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        // GET: Terminal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Terminal/Delete/5
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
    }
}
