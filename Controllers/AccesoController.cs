using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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

        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    // Cifrar la contraseña ingresada por el usuario
                    string hashedPass = ComputeSha256Hash(pass);

                    // Buscar al usuario con el email y la contraseña cifrada
                    var usuario = (from u in db.User where u.email == user && u.password == hashedPass select u).FirstOrDefault();

                    if (usuario == null)
                    {
                        // Si el usuario no existe, mostrar un mensaje de error
                        ViewBag.exception = "Usuario o contraseña no válidos";
                        return View();
                    }

                    // Crear sesión del usuario
                    Session["User"] = usuario;

                    // Registrar log de ingreso
                    string path = Server.MapPath("~/Logs/Login/");
                    Log oLog = new Log(path);
                    oLog.Add("Ingreso " + usuario.nombre);
                    oLog = null;
                }

                // Indicar que el login fue exitoso y se debe limpiar sessionStorage
                TempData["ClearSessionStorage"] = true;

                // Redirigir a la acción que maneja la vista anterior
                return RedirectToAction("RetornarVistaAnterior", "Acceso");
            }
            catch (Exception ex)
            {
                // Registrar log de excepción
                string path = Server.MapPath("~/Logs/Login");
                Log oLog = new Log(path);
                oLog.Add("Excepción en el controlador de acceso");
                oLog.Add(ex.Message);
                oLog = null;
                ViewBag.exception = ex.Message;
                return View();
            }
        }


        public ActionResult RetornarVistaAnterior()
        {
            // Regresa a la vista anterior utilizando JavaScript
            return View("RetornarVistaAnterior");
        }

        /// <summary>
        /// Metodo para recuperar la contraseña.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecuperarContraseña()
        {
            return View();
        }
        
        // Método de recuperación de contraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContraseña(string email)
        {
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var user = db.User.FirstOrDefault(u => u.email == email);
                    if (user == null)
                    {
                        ViewBag.Message = "No se encontró ningún usuario con ese correo electrónico.";
                        return View();
                    }

                    // Generar nueva contraseña
                    string newPassword = GenerateRandomPassword(10);
                    string hashedPassword = ComputeSha256Hash(newPassword);

                    // Actualizar la contraseña en la base de datos
                    user.password = hashedPassword;
                    db.SaveChanges();

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
                        Subject = "Recuperación de Contraseña",
                        Body = $"Hola {user.nombre},<br/><br/>Tu nueva contraseña es: {newPassword}<br/><br/>Por favor, cámbiala una vez que inicies sesión.",
                        IsBodyHtml = true
                    };

                    // Añadir destinatario
                    mensaje.To.Add(user.email);

                    // Enviar el correo
                    clienteSmtp.Send(mensaje);

                    ViewBag.Message = "Se ha enviado un correo electrónico con tu nueva contraseña.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = "Hubo un error al intentar enviar el correo electrónico: " + ex.Message;
            }

            return View();
        }

        // Método para generar una contraseña aleatoria
        public static string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return res.ToString();
        }

        // Método para cifrar con SHA-256
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
    }
}