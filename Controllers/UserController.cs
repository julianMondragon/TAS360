using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TAS360.Filters;
using TAS360.Models;
using TAS360.Models.ViewModel;
using DocumentFormat.OpenXml.Presentation;

namespace TAS360.Controllers
{
    public class UserController : Controller
    {
        private string contenidoHtml = @"
                                        <!DOCTYPE html>
                                        <html lang='es'>
                                        <head>
                                            <meta charset='UTF-8'>
                                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                            <title>Actualización de Ticket</title>
                                            <style>
                                                body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
                                                .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; border: 1px solid #dddddd; border-radius: 5px; overflow: hidden; }
                                                .header { background-color: #4CAF50; color: #ffffff; padding: 20px; text-align: center; }
                                                .content { padding: 20px; }
                                                .footer { background-color: #f1f1f1; color: #888888; padding: 10px; text-align: center; }
                                                .button { display: inline-block; background-color: #4CAF50; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; }
                                            </style>
                                        </head>
                                        <body>
                                            <div class='container'>
                                                <div class='header'>
                                                    <h1>Bienvenido al Help Desk de PTS</h1>
                                                </div>
                                                <div class='content'>
                                                    <p>Estimado/a <strong>{usuarioName}</strong>,</p>
                                                    <p>Ahora cuentas con acceso al <strong>Help Desk </strong> de PTS . A continuación se muestran los detalles:</p>
                                                    <ul>
                                                        <li><strong>Nombre:</strong> {usuarioName}</li>
                                                        <li><strong>Rol:</strong> {Rol}</li>
                                                        <li><strong>Email:</strong> {email}</li>
                                                        <li><strong>Contraseña:</strong> {password}</li>
                                                    </ul>
                                                    <p>Para más informacion, por favor accede a tu cuenta.</p>
                                                    <p><a href='{enlaceTicket}' class='button'>Iniciar sesion</a></p>
                                                </div>
                                                <div class='footer'>
                                                    <p>Este es un mensaje automático, por favor no responda a este correo.</p>
                                                    <p>&copy; 2024 HelpDesk PTS, Developed by Julian Mondragon</p>
                                                </div>
                                            </div>
                                        </body>
                                        </html>";
        // GET: User/Index
        [AuthorizeUser(idOperacion: 17)]
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
        [AuthorizeUser(idOperacion: 18)]
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
        [AuthorizeUser(idOperacion: 18)]
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
        [AuthorizeUser(idOperacion: 20)]
        public ActionResult Create()
        {
            ListUsuarioViewModel usuario = new ListUsuarioViewModel();
            return View(usuario);
        }

        // POST: User/Create
        [HttpPost]
        [AuthorizeUser(idOperacion: 20)]
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

                    string passencripted = ComputeSha256Hash(model.password);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var usr = db.User.FirstOrDefault(x => x.email == model.email);
                        if (usr != null)
                        {
                            ModelState.AddModelError("email", "Este correo ya exite !!!");
                            return View(model);
                        }
                        User newUser = new User
                        {
                            createdAt = DateTime.Now,
                            nombre = model.nombre,
                            email = model.email,
                            password = passencripted,
                            id_Roll = 3
                        };

                        db.User.Add(newUser);
                        db.SaveChanges();

                        User newUserAdded = db.User.FirstOrDefault(u => u.email == model.email);
                        if(newUserAdded != null)
                        {
                            sendEmailNewUser( newUserAdded.id , user.id);
                        }

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
        [AuthorizeUser(idOperacion: 19)]
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
        
        /// <summary>
        /// Metodo para cifrar la contraseña
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Metodo que envia correo al crear un nuevo usuario. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_subjet"></param>
        public void sendEmailNewUser(int id_subjet, int id_from)
        {
            try
            {
                //logs
                string path = Server.MapPath("~/Logs/Emails/");
                Log oLog = new Log(path);
                string destinatario , origen;
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var subjet = (from u in db.User where u.id == id_subjet select u).FirstOrDefault();
                    destinatario = subjet.email;
                    var from = (from u in db.User where u.id == id_from select u).FirstOrDefault();
                    origen = from.email;
                    //Remplaza el contenido del mensaje. 
                    contenidoHtml = contenidoHtml.Replace("{usuarioName}", subjet.nombre)
                             .Replace("{Rol}", GetRoles((int)subjet.id_Roll))
                             .Replace("{email}", subjet.email.ToString())
                             .Replace("{password}", "Alfer3z")
                             .Replace("{enlaceTicket}", "https://pts-tools.com.mx/Acceso/Login/");
                }

                // Configuración del cliente SMTP
                SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("soporte.tas360@pts.mx", "03Jun#2024"),
                    EnableSsl = true
                };

                // Crear el mensaje de correo
                MailMessage mensaje = new MailMessage
                {
                    From = new MailAddress("soporte.tas360@pts.mx"),
                    Subject = "Bienvenido al Help Desk de PTS",
                    Body = contenidoHtml,
                    IsBodyHtml = true // Si el cuerpo del correo es HTML
                };

                // Añadir destinatario
                mensaje.To.Add(destinatario);
                // Añadir en copia (CC)
                mensaje.CC.Add(origen);
                
                if (origen != "julian.mondragon@pts.mx")
                {
                    mensaje.CC.Add("julian.mondragon@pts.mx");
                }
                
                // Enviar el correo
                clienteSmtp.Send(mensaje);
                oLog.Add("---------------------------");
                oLog.Add($"Correo enviado exitosamente a {destinatario} sobre la creacion de su nuevo usuario.");
                //Devuelve un mensaje exitoso a la vista 
                ViewBag.InfoMessage = $"Correo enviado exitosamente a {destinatario} sobre la creacion de su nuevo usuario.";
            }
            catch (SmtpException smtpEx)
            {
                //logs
                string path = Server.MapPath("~/Logs/Emails/");
                Log oLog = new Log(path);
                oLog.Add($"SMTP Error al enviar el correo: {smtpEx.Message}  Status Code: {smtpEx.StatusCode}");
                ViewBag.ExceptionMessage = "SMTP Error al enviar el correo: " + smtpEx.Message + " Status Code: " + smtpEx.StatusCode;
                if (smtpEx.InnerException != null)
                {
                    oLog.Add(" Inner Exception: " + smtpEx.InnerException.Message);
                    ViewBag.ExceptionMessage += " Inner Exception: " + smtpEx.InnerException.Message;
                }
            }
            catch (Exception ex)
            {
                //logs
                string path = Server.MapPath("~/Logs/Emails/");
                Log oLog = new Log(path);
                oLog.Add($"Exception al enviar el correo: {ex.Message}");
                ViewBag.ExceptionMessage = "Exception al enviar el correo: " + ex.Message;
            }
        }

        /// <summary>
        /// Devuelve a la vista una lista de los Roles del sistema 
        /// </summary>
        private string GetRoles(int id)
        {
            
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Roll select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        if (a.id == id)
                            return a.nombre;
                       
                    }
                }
            }
            return "Sin Roll";
        }
}
}
