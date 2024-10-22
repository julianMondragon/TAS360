﻿using DocumentFormat.OpenXml.Office2010.Excel;
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
using DocumentFormat.OpenXml.EMMA;

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
                    perfil.id_User = usuario.id;
                    perfil.nombre = usuario.nombre;
                    perfil.email = usuario.email;
                    perfil.Cel = usuario.Cel;
                    perfil.Género = usuario.Género;
                    perfil.Estado = usuario.Estado;
                    perfil.Foto_usuario = usuario.Foto_usuario;

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
                    perfil.Foto_usuario = usuario.Foto_usuario;

                }
                else
                {
                    return HttpNotFound("Usuario no encontrado.");
                }
            }

            // Llenar el ViewBag con las opciones para los campos desplegables de género y estado
            ViewBag.GeneroOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Masculino", Value = "Masculino" },
                new SelectListItem { Text = "Femenino", Value = "Femenino" },
                new SelectListItem { Text = "Otro", Value = "Otro" }
            };

            ViewBag.EstadoOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Aguascalientes", Value = "Aguascalientes" },
                new SelectListItem { Text = "Baja California", Value = "Baja California" },
                // ... (otros estados) ...
                new SelectListItem { Text = "Zacatecas", Value = "Zacatecas" }
            };

            return View(perfil);
        }

        [HttpPost]
        public ActionResult Guardar(PerfilusrViewModel perfil, HttpPostedFileBase Foto_usuario)
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
                        perfil.Foto_usuario = usuario.Foto_usuario;



                        // Guardar los cambios en la base de datos
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index"); // Redirige a la acción Index
                }

                // Si el modelo no es válido, regresa la vista con el modelo para mostrar los errores
                ViewBag.ErrorMessage = "Por favor, complete todos los campos requeridos.";

                // Rellenar el ViewBag nuevamente con las opciones para evitar errores en la vista
                ViewBag.GeneroOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Masculino", Value = "Masculino" },
                    new SelectListItem { Text = "Femenino", Value = "Femenino" },
                    new SelectListItem { Text = "Otro", Value = "Otro" }
                };

                ViewBag.EstadoOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Aguascalientes", Value = "Aguascalientes" },
                    new SelectListItem { Text = "Baja California", Value = "Baja California" },
                    // ... (otros estados) ...
                    new SelectListItem { Text = "Zacatecas", Value = "Zacatecas" }
                };

                return View(perfil);
            }
            catch (Exception ex)
            {
                // Registrar el error
                System.Diagnostics.Debug.WriteLine("Error al guardar el registro: " + ex.Message);

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";

                // Rellenar el ViewBag nuevamente con las opciones para evitar errores en la vista
                ViewBag.GeneroOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Masculino", Value = "Masculino" },
                    new SelectListItem { Text = "Femenino", Value = "Femenino" },
                    new SelectListItem { Text = "Otro", Value = "Otro" }
                };

                ViewBag.EstadoOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Aguascalientes", Value = "Aguascalientes" },
                    new SelectListItem { Text = "Baja California", Value = "Baja California" },
                    // ... (otros estados) ...
                    new SelectListItem { Text = "Zacatecas", Value = "Zacatecas" }
                };

                // Devolver la vista con el modelo
                return View(perfil);
            }
        }

       
      




    }
}
