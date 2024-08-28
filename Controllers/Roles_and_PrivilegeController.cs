using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class Roles_and_PrivilegeController : Controller
    {
        /// <summary>
        ///  Metodo principal que muestra los modulos, roles y privilegios 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RolesPrivilegesViewModel rolesPrivileges = new RolesPrivilegesViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var Users = from m in db.User select m;
                if (Users.Any())
                {
                    foreach (var item in Users)
                    {
                        rolesPrivileges.Users.Add(item);
                    }
                }
                var modules = from m in db.Modulo select m;
                if (modules.Any())
                {
                    foreach (var item in modules)
                    {
                        rolesPrivileges.Modules.Add(item);
                    }
                }
                var roles = from r in db.Roll select r;
                if (roles.Any())
                {
                    foreach (var item in roles)
                    {
                        rolesPrivileges.Rolls.Add(item);
                    }
                }
                var operacion = from r in db.Operacion select r;
                if (roles.Any())
                {
                    foreach (var item in operacion)
                    {
                        rolesPrivileges.Operacions.Add(item);
                    }
                }
                var rol_operacionA = from R in db.Roll_Operacion where R.id_Roll == 1 select R;
                if (rol_operacionA.Any())
                {
                    foreach( var item in rol_operacionA)
                    {
                        rolesPrivileges.Rol_OperacionAdmin.Add(item);
                    }
                }
                var rol_operacionR = from R in db.Roll_Operacion where R.id_Roll == 2 select R;
                if (rol_operacionR.Any())
                {
                    foreach (var item in rol_operacionR)
                    {
                        rolesPrivileges.Rol_OperacionResp.Add(item);
                    }
                }
                var rol_operacionC = from R in db.Roll_Operacion where R.id_Roll == 3 select R;
                if (rol_operacionC.Any())
                {
                    foreach (var item in rol_operacionC)
                    {
                        rolesPrivileges.Rol_OperacionContac.Add(item);
                    }
                }
                var rol_operacionV = from R in db.Roll_Operacion where R.id_Roll == 4 select R;
                if (rol_operacionV.Any())
                {
                    foreach (var item in rol_operacionV)
                    {
                        rolesPrivileges.Rol_OperacionVist.Add(item);
                    }
                }
                else
                {
                    int count = 0;
                    while (count < 3)
                    {
                        rolesPrivileges.Rol_OperacionVist.Add(new Roll_Operacion()
                        {
                            id = count,
                            id_Roll = 4,
                            id_Operacion = 4
                        });
                        count++;
                    }
                }

            }
            GetUsuarios();
            return View(rolesPrivileges);
        }

        /// <summary>
        /// Metodo GET que muestra el formulario para agregar modulos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddModule()
        {
            ModuloViewModel modulo = new ModuloViewModel();
            return View(modulo);
        }

        /// <summary>
        /// Metodo Post para agregar modulos.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddModule(ModuloViewModel model)
        {
            User user = (User)Session["User"];
            try
            {                
                if (ModelState.IsValid)
                {                    
                    if (user == null)
                    {
                        ViewBag.InfoMessage = "Inicia sesion para crear un pendiente ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Logs/RolesPriv/AddModule/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var result = db.Modulo.FirstOrDefault(m => m.nombre.Contains(model.nombre));
                        if (result != null && result.nombre != null)
                        {
                            ViewBag.ExceptionMessage = "Modulo ya existente: " + result.nombre;
                            return View(model);
                        }
                        db.Modulo.Add(new Modulo()
                        {
                            nombre = model.nombre
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();
                        //Agrega logs
                        oLog.Add("Se agrega un nuevo Modulo  por id user: " + user.id + " Con Nombre: " + user.nombre);
                        oLog.Add("Nombre: " + model.nombre);
                    }
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex) 
            {
                string path = Server.MapPath("~/Logs/RolesPriv/AddModule/");
                Log oLog = new Log(path);
                oLog.Add("------------------------");
                oLog.Add("Se detecta una excepcion al agrega un nuevo Modulo  por id user: " + user.id + " Con Nombre: " + user.nombre);
                oLog.Add("Nombre del modulo: " + model.nombre);
                oLog.Add("exeption: " + ex.Message);
                oLog.Add("------------------------");

                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        /// <summary>
        /// Metodo GET que muestra el formulario para agregar Operaciones.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOperation()
        {
            OperacionViewModel model = new OperacionViewModel();
            GetModulos();
            return View(model);
        }
        /// <summary>
        /// Metodo Post para agregar roles.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOperation(OperacionViewModel model)
        {
            User user = (User)Session["User"];
            GetModulos();
            try
            {
                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        ViewBag.InfoMessage = "Inicia sesion para crear una operacion";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Logs/RolesPriv/AddOperacion/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var result = db.Operacion.FirstOrDefault(m => m.nombre.Contains(model.nombre));
                        if (result != null && result.nombre != null)
                        {
                            ViewBag.ExceptionMessage = "Operacion ya existente: " + result.nombre;
                            return View(model);
                        }
                        db.Operacion.Add(new Operacion()
                        {
                            nombre = model.nombre,
                            id_Modulo = model.id_modulo,
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();
                        //Agrega logs
                        oLog.Add("Se agrega una nueva Operacion  por id user: " + user.id + " Con Nombre: " + user.nombre);
                        oLog.Add("Nombre: " + model.nombre);
                    }
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                GetModulos();
                string path = Server.MapPath("~/Logs/RolesPriv/AddOperacion/");
                Log oLog = new Log(path);
                oLog.Add("------------------------");
                oLog.Add("Se detecta una excepcion al agrega una operacion  por id user: " + user.id + " Con Nombre: " + user.nombre);
                oLog.Add("Nombre del operacion: " + model.nombre);
                oLog.Add("exeption: " + ex.Message);
                oLog.Add("------------------------");

                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }
        /// <summary>
        /// Metodo GET que muestra el formulario para agregar roles.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRol()
        {
            RollViewModel model = new RollViewModel();
            return View(model);
        }
        /// <summary>
        /// Metodo Post para agregar roles.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRol(RollViewModel model)
        {
            User user = (User)Session["User"];
            try
            {
                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        ViewBag.InfoMessage = "Inicia sesion para crear un pendiente ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Logs/RolesPriv/AddRol/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var result = db.Roll.FirstOrDefault(m => m.nombre.Contains(model.nombre));
                        if (result != null && result.nombre != null)
                        {
                            ViewBag.ExceptionMessage = "Rol ya existente: " + result.nombre;
                            return View(model);
                        }
                        db.Roll.Add(new Roll()
                        {
                            nombre = model.nombre
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();
                        //Agrega logs
                        oLog.Add("Se agrega un nuevo Modulo  por id user: " + user.id + " Con Nombre: " + user.nombre);
                        oLog.Add("Nombre: " + model.nombre);
                    }
                }
                else
                {
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~/Logs/RolesPriv/AddRol");
                Log oLog = new Log(path);
                oLog.Add("------------------------");
                oLog.Add("Se detecta una excepcion al agrega un nuevo rol  por id user: " + user.id + " Con Nombre: " + user.nombre);
                oLog.Add("Nombre del modulo: " + model.nombre);
                oLog.Add("exeption: " + ex.Message);
                oLog.Add("------------------------");

                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        /// <summary>
        /// Metodo GET que muestra el formulario para agregar operaciones.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRol_Operacion()
        {
            GetRoles();
            GetOperaciones();
            Rol_OperacionViewModel model = new Rol_OperacionViewModel();
            return View(model);
        }
        /// <summary>
        /// Metodo Post para agregar roles.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRol_Operacion(Rol_OperacionViewModel model)
        {
            GetRoles();
            GetOperaciones();
            User user = (User)Session["User"];
            try
            {
                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        GetRoles();
                        GetOperaciones();
                        ViewBag.InfoMessage = "Inicia sesion para crear un pendiente ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Logs/RolesPriv/AddRol_operacion/");
                    Log oLog = new Log(path);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var result = db.Roll_Operacion.Where(r => r.id_Roll == model.id_rol && r.id_Operacion == model.id_operacion);
                        if (result != null && result.Any())
                        {
                            ViewBag.ExceptionMessage = "Privilegio ya existente con id: " + result.FirstOrDefault().id;
                            return View(model);
                        }
                        db.Roll_Operacion.Add(new Roll_Operacion()
                        {
                            id_Roll = model.id_rol,
                            id_Operacion = model.id_operacion
                        });
                        //Guarda los cambios en la BD
                        db.SaveChanges();
                        //Agrega logs
                        oLog.Add("Se agrega un nuevo rol_operacion  por id user: " + user.id + " Con Nombre: " + user.nombre);
                        oLog.Add("Rol: " + model.id_rol + " Operacion: " + model.id_operacion);
                    }
                }
                else
                {
                    GetRoles();
                    GetOperaciones();
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                GetRoles();
                GetOperaciones();
                string path = Server.MapPath("~/Logs/RolesPriv/AddRol_operacion");
                Log oLog = new Log(path);
                oLog.Add("------------------------");
                oLog.Add("Se detecta una excepcion al agrega un nuevo rol_operacion  por id user: " + user.id + " Con Nombre: " + user.nombre);
                oLog.Add("Rol: " + model.id_rol + " Operacion: " + model.id_operacion);
                oLog.Add("exeption: " + ex.Message);
                oLog.Add("------------------------");

                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        /// <summary>
        /// Devuelve a la vista una lista de los usuarios especialistas tecnicos 
        /// </summary>
        private void GetUsuarios()
        {

            List<SelectListItem> Usuarios = new List<SelectListItem>();
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
        /// Devuelve a la vista una lista de los Modulos del sistema 
        /// </summary>
        private void GetModulos()
        {

            List<SelectListItem> Modulos = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Modulo select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Modulos.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Modulos = Modulos;
        }

        /// <summary>
        /// Devuelve a la vista una lista de los Roles del sistema 
        /// </summary>
        private void GetRoles()
        {

            List<SelectListItem> Roles = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Roll select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Roles.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Roles = Roles;
        }

        /// <summary>
        /// Devuelve a la vista una lista de los operaciones del sistema 
        /// </summary>
        private void GetOperaciones()
        {

            List<SelectListItem> Operaciones = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Operacion select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Operaciones.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Operaciones = Operaciones;
        }
    }
}
