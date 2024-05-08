using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class UserController : Controller
    {
        // GET: User/Index
        public ActionResult Index()
        {
            List<ListUsuarioViewModel> lst;
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                lst = (from d in db.User
                       select new ListUsuarioViewModel
                       {
                           id = d.id,
                           nombre = d.nombre,
                           email = d.email,
                       }).ToList();
            }

            return View(lst);
        }

        // GET: User/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ListUsuarioViewModel model = new ListUsuarioViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var userToEdit = db.User.Find(id);
                if (userToEdit != null)
                {
                    model.id = userToEdit.id;
                    model.nombre = userToEdit.nombre;
                    model.email = userToEdit.email;
                }
            }
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(ListUsuarioViewModel model, string confirmPassword)
        {
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    string path = Server.MapPath("~/Logs/Usuarios/");
                    Log oLog = new Log(path);
                    

                    var userToEdit = db.User.Find(model.id);
                    if (userToEdit != null)
                    {
                        oLog.Add($"Usuario con ID {userToEdit.id} editado por {((User)Session["User"]).nombre}");
                        oLog.Add($"Nombre anterior: {userToEdit.nombre}");
                        oLog.Add($"Email anterior: {userToEdit.email}");
                        userToEdit.nombre = model.nombre;
                        userToEdit.email = model.email;


                        if (model.password != confirmPassword)
                        {
                            ModelState.AddModelError("confirmPassword", "Las contraseñas no coinciden");
                            ModelState.AddModelError("Password", "Las contraseñas no coinciden");
                            return View(model);
                        }
                        userToEdit.password = model.password;

                        // Guardar log de la edición del usuario                       
                        oLog.Add($"Nuevo nombre: {userToEdit.nombre}");
                        oLog.Add($"Nuevo email: {userToEdit.email}");
                        oLog.Add($"--------------------------------");
                    }
                    else
                    {
                        ViewBag.ExceptionMessage = "ID no encontrado";
                        return View(model);
                    }

                    db.Entry(userToEdit).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ListUsuarioViewModel usuario = new ListUsuarioViewModel();
            return View(usuario);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(ListUsuarioViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Obtener el usuario actual
                    User user = (User)Session["User"];
                    if (user == null)
                    {
                        ViewBag.InfoMessage = "Inicia sesión para crear un usuario";
                        return View(model);
                    }

                    if (model.password != model.confirmPassword)
                    {
                        ModelState.AddModelError("confirmPassword", "Las contraseñas no coinciden");
                        ModelState.AddModelError("Password", "Las contraseñas no coinciden");
                        return View(model);
                    }

                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        User newUser = new User
                        {
                            createdAt = DateTime.Now,
                            nombre = model.nombre,
                            email = model.email,
                            password = model.password
                        };

                        db.User.Add(newUser);
                        db.SaveChanges();

                        string path = Server.MapPath("~/Logs/Usuarios/");
                        Log oLog = new Log(path);
                        oLog.Add($"Se agrega nuevo usuario por id user: {user.id} con nombre: {user.nombre}");
                        oLog.Add($"Nombre: {model.nombre}");
                        oLog.Add($"Email: {model.email}");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.Message;
                return View(model);
            }
        }

        // GET: User/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var UserToDelete = db.User.Find(id);
                    if (UserToDelete != null)
                    {
                        db.User.Remove(UserToDelete);
                        db.SaveChanges();

                        // Guardar log de la eliminación del usuario
                        string path = Server.MapPath("~/Logs/Usuarios/");
                        Log oLog = new Log(path);
                        oLog.Add($"Usuario con ID {id} eliminado por {((User)Session["User"]).nombre}");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.Message;
                return View();
            }
        }
    }
}
