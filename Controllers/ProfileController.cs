using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;  
using System.Web;
using System.IO;


namespace TAS360.Controllers
{
    
    public class ProfileController : Controller
    {
        // GET: Profile/Index
        public ActionResult Index()
        {
            PerfilusrViewModel perfil = new PerfilusrViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
               
                int userId = ((User)Session["User"]).id; 

                // Buscar el perfil del usuario conectado en la base de datos usando el ID
                var usuario = db.usr_profile.FirstOrDefault(u => u.id_User == userId);

                
                if (usuario != null)
                {
                    perfil.nombre = usuario.nombre;
                    perfil.email = usuario.email;
                    perfil.Cel = usuario.Cel;
                    perfil.Género = usuario.Género;
                    perfil.Estado = usuario.Estado;
                }
                else
                {
                    ViewBag.ErrorMessage = "Usuario no encontrado.";
                }
            }

            return View(perfil);
        }

        public ActionResult Editprofile()
        {
            PerfilusrViewModel perfil = new PerfilusrViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {

                int userId = ((User)Session["User"]).id;

                // Buscar el perfil del usuario en la base de datos usando el ID del usuario
                var usuario = db.usr_profile.FirstOrDefault(u => u.id_User == userId);

                if (usuario != null)
                {
                    perfil.nombre = usuario.nombre;
                    perfil.email = usuario.email;
                    perfil.Cel = usuario.Cel;
                    perfil.Género = usuario.Género;
                    perfil.Estado = usuario.Estado;
                }
                else
                {
                    return HttpNotFound("Usuario no encontrado.");
                }
            }

            return View(perfil);
        }

        [HttpPost]
        public ActionResult Guardar(PerfilusrViewModel perfil)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        int userId = ((User)Session["User"]).id;

                        // Buscar el perfil del usuario en la base de datos usando el ID del usuario
                        var usuario = db.usr_profile.FirstOrDefault(u => u.id_User == userId);

                        if (usuario == null)
                        {
                            return HttpNotFound("Registro no encontrado.");
                        }

                        // Asignar los valores del perfil al usuario
                        usuario.nombre = perfil.nombre;
                        usuario.email = perfil.email;
                        usuario.Cel = perfil.Cel;
                        usuario.Género = perfil.Género;
                        usuario.Estado = perfil.Estado;

                        // Guardar los cambios en la base de datos
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index"); // Redirige a la acción Index
                }

                // Si el modelo no es válido, regresa la vista con el modelo para mostrar los errores
                ViewBag.ErrorMessage = "Por favor, complete todos los campos requeridos.";
                return View(perfil);
            }
            catch (Exception ex)
            {
                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al guardar el registro: " + ex.Message);

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";

                // Devolver la vista con el modelo
                return View(perfil);
            }
        }



    }
}