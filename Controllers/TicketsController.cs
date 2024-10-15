using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TAS360.Filters;
using TAS360.Models;
using TAS360.Models.ViewModel;
using System.Net;
using System.Net.Mail;
using DocumentFormat.OpenXml.EMMA;
using System.EnterpriseServices.Internal;



namespace TAS360.Controllers
{
    public class TicketsController : Controller
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
                                                    <h1>Actualización de Ticket</h1>
                                                </div>
                                                <div class='content'>
                                                    <p>Estimado/a <strong>{usuarioName}</strong>,</p>
                                                    <p>El ticket <strong>{ticketId}</strong> ha sido actualizado. A continuación se muestran los detalles:</p>
                                                    <ul>
                                                        <li><strong>Título:</strong> {titulo}</li>
                                                        <li><strong>Estado:</strong> {estado}</li>
                                                        <li><strong>Último Comentario:</strong> {ultimoComentario}</li>
                                                    </ul>
                                                    <p>Para más detalles, por favor accede a tu cuenta.</p>
                                                    <p><a href='{enlaceTicket}' class='button'>Ver Ticket</a></p>
                                                </div>
                                                <div class='footer'>
                                                    <p>Este es un mensaje automático, por favor no responda a este correo.</p>
                                                    <p>&copy; 2024 HelpDesk PTS</p>
                                                </div>
                                            </div>
                                        </body>
                                        </html>";

        /// <summary>
        /// Devuelve a la vista un listados de los tickets 
        /// Se asigna al  modelo el ultimo registro del ticket a los campos User , RecortStatus, nombre de categoria y subsistema
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser(idOperacion:5)]
        public ActionResult Index()
        {
              List<TicketViewModel> tickets = new List<TicketViewModel>();
            using (Models.HelpDesk_Entities1 db = new Models.HelpDesk_Entities1())
            {
                var Tickets = (from s in db.Ticket where s.status != 12 orderby s.CreatedAt descending select s);
                if (Tickets != null && Tickets.Any())
                {
                    foreach (var t in Tickets)
                    {
                        try
                        {
                            TicketViewModel ticketViewModel = new TicketViewModel
                            {
                                id = t.id,
                                titulo = t.titulo,
                                mensaje = t.mensaje,
                                usuario_name = t.Ticket_User.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.User.nombre,
                                categoria_name = t.Categoria.nombre,
                                terminal_name = t.Terminal.Nombre,
                                status_name = t.Ticket_Record_Status.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Status.descripcion,
                                Subsistema_name = t.Subsistema.Nombre,
                                Status = t.status,
                                id_externo = t.id_externo
                            };

                            var lastComment = t.Ticket_Comentario.OrderByDescending(x => x.id).FirstOrDefault();
                            if (lastComment != null)
                            {
                                ticketViewModel.LastComent = lastComment.Comentario.Comentario1;
                            }
                            else
                            {
                                if (ticketViewModel.mensaje.Count() >= 220)
                                    ticketViewModel.LastComent = ticketViewModel.mensaje.Substring(0, 220) + "...";
                                else
                                    ticketViewModel.LastComent = ticketViewModel.mensaje;
                            }

                            tickets.Add(ticketViewModel);
                        }
                        catch(Exception ex) 
                        {
                            ViewBag.ExceptionMessage = ex.Message;
                        }
                    }
                }
            }
            return View(tickets);
        }
        /// <summary>
        /// Método que genera la lista de tickets filtrados para posteriormente enviarlos 
        /// a la vista de IndexWithFilter
        /// </summary>
        /// <param name="encodedCurrentList"></param>
        /// <returns></returns>
        [AuthorizeUser(idOperacion: 5)]
        public ActionResult IndexWithFilter(string encodedCurrentList)
        {
            // Inicializa la lista
            List<TicketViewModel> model = new List<TicketViewModel>();
            List<CurrentList> currentList = new List<CurrentList>();
            // Decodifica el objeto que recibe
            var decodedObject = HttpUtility.UrlDecode(encodedCurrentList);
            currentList = JsonConvert.DeserializeObject<List<CurrentList>>(decodedObject);
            // Aquí itera sobre cada elemento para crear la lista
            foreach (var item in currentList)
            {
                item.is_selected = false;
                // Se conecta a la bd
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    // Busca el ticket correspondiente en la base de datos
                    var t = db.Ticket.FirstOrDefault(tic => tic.id == item.id);
                    if (t != null)
                    {
                        // Busca el usuario asociado al ticket
                        var User = db.User.FirstOrDefault(usr => usr.id == t.id_User);
                        // Va añadiendo a la lista cada TicketViewModel de los tickets existentes
                        model.Add(new TicketViewModel()
                        {
                            id = t.id,
                            titulo = t.titulo,
                            mensaje = t.mensaje,
                            usuario_name = User != null ? User.nombre : "Usuario no encontrado",
                            categoria_name = t.Categoria != null ? t.Categoria.nombre : "Categoría no encontrada",
                            terminal_name = t.Terminal != null ? t.Terminal.Nombre : "Terminal no encontrada",
                            status_name = t.Ticket_Record_Status.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Status.descripcion ?? "Estado no encontrado",
                            Subsistema_name = t.Subsistema != null ? t.Subsistema.Nombre : "Subsistema no encontrado",
                            Status = t.status,
                            currentList = currentList
                        });
                    }                    
                }
            }
            // Devuelve la vista con la lista de modelos de tickets
            return View(model);
        }

        /// <summary>
        /// Abre la vista para crear un nuevo un ticket y obtiene los catalogos necesarios para relacionarlo.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(idOperacion: 1)]
        public ActionResult CreateTicket()
        {
            TicketViewModel NewTicket = new TicketViewModel();
            NewTicket.Status = 1;
            NewTicket.mensaje = GetTemplatemessage();
            GetCategories();
            GetTerminales();
            GetUsuarios();
            GetStatusEditTicket(1);
            GetSubsistemas();
            return View(NewTicket);
        }

        /// <summary>
        /// Crea un nuevo registro en la tabla Ticket 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser(idOperacion: 1)]
        public ActionResult CreateTicket(TicketViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string path = Server.MapPath("~/Logs/Tickets/");
                    Log oLog = new Log(path);
                    int idTicket = 0;
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        Ticket ticket = new Ticket();

                        ticket.titulo = model.titulo;
                        ticket.id_Terminal = model.id_Terminal;
                        ticket.id_Categoria = model.id_Categoria;
                        ticket.id_Subsistema = model.id_Subsistema;
                        ticket.id_User = ((User)Session["User"]).id;
                        ticket.status = model.Status;
                        ticket.mensaje = model.mensaje;
                        ticket.CreatedAt = DateTime.Now;
                        ticket.id_externo = model.id_externo;

                        //db.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
                        db.Ticket.Add(ticket);
                        db.SaveChanges();

                        idTicket = db.Ticket.FirstOrDefault(a => a.titulo == model.titulo && a.id_Terminal == model.id_Terminal && a.id_Categoria == model.id_Categoria && a.mensaje == model.mensaje).id;
                        oLog.Add("Se creo el Ticket: " + idTicket);
                        oLog.Add("Titulo del Ticket: " + model.titulo);
                        oLog.Add("Usuario creador: " + ((User)Session["User"]).id);
                        oLog.Add("Usuario creador: " + ((User)Session["User"]).nombre);
                        if (idTicket != 0)
                        {
                            Ticket_Record_Status New_ticket_Record = new Ticket_Record_Status();
                            New_ticket_Record.id_Ticket = idTicket;
                            New_ticket_Record.id_Status = ticket.status;
                            New_ticket_Record.CreatedAt = DateTime.Now;

                            db.Ticket_Record_Status.Add(New_ticket_Record);
                            db.SaveChanges();
                            oLog.Add("Se agrega record status");
                            oLog.Add("Status: " + New_ticket_Record.id_Status);

                            Ticket_User ticket_User = new Ticket_User();
                            ticket_User.id_Ticket = ticket.id;
                            ticket_User.id_User = (int)model.id_Resp;
                            ticket_User.CreatedAt = DateTime.Now;

                            db.Ticket_User.Add(ticket_User);
                            db.SaveChanges();
                            oLog.Add("Se agrega nuevo usuario asociado al ticket: " + model.id_Resp);

                        }
                    }

                    //Envia correo
                    //EnviarCorreo("emiliano.garcia@pts.mx", "Actualización de Ticket", contenidoHtml);
                    sendEmailUpdateTK(idTicket,(int)model.id_Resp, ((User)Session["User"]).id);

                    return Redirect("~/Tickets/Index");
                }
                else
                {
                    string path = Server.MapPath("~/Logs/Tickets/");
                    Log oLog = new Log(path);
                    oLog.Add("Formulaio no valido");                    
                    ViewBag.ExceptionMessage = "Formulaio no valido...";
                    GetCategories();
                    GetTerminales();
                    GetUsuarios();
                    GetStatusEditTicket(1);
                    GetSubsistemas();
                    return View(model);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string Message = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                        Message += "Property: " + validationError.PropertyName +  " Error: "+ validationError.ErrorMessage + "\n";
                    }
                }

                //Add Logs
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add(Message);

                ViewBag.ExceptionMessage = Message;
                return View(model);
            }
            catch (Exception ex)
            {
                GetCategories();
                GetTerminales();
                GetUsuarios();
                GetStatus(1);
                GetSubsistemas();
                ViewBag.ExceptionMessage = ex.Message;
                //Add Log
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add(ex.Message);
                return View(model);

            }
        }

        /// <summary>
        /// Muestra el Tickted seleccionado con sus archivos y comentarios
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(idOperacion:4)]
        public ActionResult ShowTicket(int id)
        {
            TicketViewModel myticket = new TicketViewModel();
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var ticket = db.Ticket.Find(id);
                    myticket.id = id;
                    myticket.titulo = ticket.titulo;
                    myticket.mensaje = ticket.mensaje;
                    myticket.id_externo = ticket.id_externo;
                    myticket.usuario_name = ticket.Ticket_User.OrderByDescending(u => u.CreatedAt).FirstOrDefault().User.nombre;
                    myticket.categoria_name = ticket.Categoria.nombre;
                    myticket.status_name = db.Ticket_Record_Status.Where(x => x.id_Ticket == id).OrderByDescending(x => x.CreatedAt).FirstOrDefault().Status.descripcion;
                    myticket.terminal_name = db.Terminal.Where(x => x.id == ticket.id_Terminal).FirstOrDefault().Nombre; 
                    


                    //Lista de status
                    myticket.RecordStatus = new List<string>();

                    foreach (var status in db.Ticket_Record_Status.Where(x => x.id_Ticket == id))
                    {
                        myticket.RecordStatus.Add(status.Status.descripcion);
                    }
                    myticket.Subsistema_name = ticket.Subsistema.Nombre;
                    var files = db.Tickets_Files.Where(f => f.id_Ticket == id);
                    //Lista de Files
                    myticket.Files = new List<Archivos>();
                    foreach (var file in files)

                    {


                        myticket.Files.Add(new Archivos
                        {
                            id = file.Files.id,
                            Nombre = file.Files.Nombre,
                            //Local
                            //URL = (file.Files.URL.Replace("C:\\Projects\\PTS\\TAS360", "")).Replace("\\","/")
                            //Plesk
                            URL = (file.Files.URL.Replace("C:\\Inetpub\\vhosts\\pts-tools.com.mx\\httpdocs\\softwaretool", "")).Replace("\\", "/")
                        });
                    }
                    //Comentarios
                    if (db.Ticket_Comentario.Where(c => c.id_Ticket == id).Any())
                    {
                        var coms = db.Ticket_Comentario.Where(c => c.id_Ticket == id);
                        foreach (var com in coms)
                        {

                            myticket.mensaje += ("\n" + com.Comentario.Comentario1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Add Log
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add(ex.Message);
                ViewBag.Exception = ex.Message;
            }
            
            return View(myticket);
        }

        /// <summary>
        /// Metodo que se encarga de hacer la plantilla del reporte del ticket para ser impreso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TicketReport(int id)
        {
            TicketViewModel myticket = new TicketViewModel();
            try
            {
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var ticket = db.Ticket.Find(id);
                    myticket.id = id;
                    myticket.titulo = ticket.titulo;
                    myticket.mensaje = ticket.mensaje;
                    myticket.usuario_name = ticket.Ticket_User.OrderByDescending(u => u.CreatedAt).FirstOrDefault().User.nombre;
                    myticket.categoria_name = ticket.Categoria.nombre;
                    myticket.status_name = db.Ticket_Record_Status.Where(x => x.id_Ticket == id).OrderByDescending(x => x.CreatedAt).FirstOrDefault().Status.descripcion;
                    myticket.terminal_name = db.Terminal.Where(x => x.id == ticket.id_Terminal).FirstOrDefault().Nombre;


                  
                    //Lista de status
                    myticket.RecordStatus = new List<string>();
                    foreach (var status in db.Ticket_Record_Status.Where(x => x.id_Ticket == id))
                    {
                        myticket.RecordStatus.Add(status.Status.descripcion);
                    }
                    myticket.Subsistema_name = ticket.Subsistema.Nombre;
                    var files = db.Tickets_Files.Where(f => f.id_Ticket == id);
                    //Lista de Files
                    myticket.Files = new List<Archivos>();
                    foreach (var file in files)
                    {

                        myticket.Files.Add(new Archivos
                        {
                            id = file.Files.id,
                            Nombre = file.Files.Nombre,
                           //Local
                            //URL = (file.Files.URL.Replace("C:\\Projects\\PTS\\TAS360", "")).Replace("\\","/")
                           //Plesk
                            URL = (file.Files.URL.Replace("C:\\Inetpub\\vhosts\\pts-tools.com.mx\\httpdocs\\softwaretool", "")).Replace("\\", "/")
                        }); 
                    }
                    //Comentarios

                                     
                    if (db.Ticket_Comentario.Where(c => c.id_Ticket == id).Any())
                    {
                        var coms = db.Ticket_Comentario.Where(c => c.id_Ticket == id);


                        foreach (var com in coms)
                        {
                            
                            if (com.Comentario.Comentario1.Contains("Cambio de Status a:"))
                            {
                                myticket.mensaje += (com.Comentario.Comentario1);
                            }
                            else
                            {
                                string formattedDate = "********";
                                if (com.Comentario.CreatedAt != null)
                                {
                                    var Date = (DateTime)com.Comentario.CreatedAt;
                                    formattedDate = Date.ToString("dd-MM-yyyy");
                                }
                                myticket.mensaje += ("\n*****************************************************" + "\n----------------------------------------- " + formattedDate + "\n * Sin cambio de Status." + "\n" + "---------------------------------------------------- \n" + "***************************************************** \n" + com.Comentario.Comentario1 + "\n");
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Add Log
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add(ex.Message);
                ViewBag.Exception = ex.Message;
            }

            return View(myticket);
        }

        /// <summary>
        /// Metodo encargado de imprimir el reporte de un ticket por su id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrintTicketReport(int id)
        {
            return new ActionAsPdf($"TicketReport/{id}")
            {
                FileName = $"Reporte de Ticket {id}.pdf"
            };
        }

        /// <summary>
        /// Devuelve a la vista el ticket con el comentario base, el ultimo usuario y el ultimo status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(idOperacion: 6)]
        public ActionResult AddCommentTicket(int id) 
        {
            TicketViewModel ticket = new TicketViewModel();
            using(HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var T = db.Ticket.Find(id);
                ticket.id = id;
                ticket.Status = T.status;
                ticket.titulo = T.titulo;
                ticket.mensaje = T.mensaje;
                //Asignar el usuario.
                ticket.id_Usuario = db.Ticket_User.Where(a => a.id_Ticket == id).OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User;
            }

            Comentarios comentario = new Comentarios();
            comentario.id_User = ((User)Session["User"]).id;
            comentario.Comentario1 = GetTemplateComent();
            ticket.Comentarios = new List<Comentarios>();
            ticket.Comentarios.Add(comentario);
            GetStatus((int)ticket.Status);
            GetUsuarios();

            return View(ticket);            
        }

        /// <summary>
        /// Guarda en la Base de datos un nuevo Comentario relacionado con el Ticket, si cambio de 
        /// status genera un registro de record status del ticket, si se cambio de responsable del ticket
        /// se agrega un nuevo usuario
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="Comentario"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser(idOperacion: 6)]
        public ActionResult AddCommentTicket(TicketViewModel ticket , Comentarios Comentario)
        {
            try
            {                
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add("Agrega comentario a Ticket: " + ticket.id);
                bool StatusChanged = false;
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    var t = db.Ticket.Find(ticket.id);  
                    oLog.Add("Status actual del Ticket: " + t.status );
                    oLog.Add("Nuevo Status para el Ticket: " + ticket.Status);
                    oLog.Add("Valida status");
                    if (t.status != ticket.Status)
                    {
                        Ticket_Record_Status New_ticket_Record = new Ticket_Record_Status();
                        New_ticket_Record.id_Ticket = ticket.id;
                        New_ticket_Record.id_Status = ticket.Status;
                        New_ticket_Record.CreatedAt = DateTime.Now;
                        db.Ticket_Record_Status.Add(New_ticket_Record);
                        t.status = ticket.Status;
                        StatusChanged = true;
                        db.SaveChanges();
                        oLog.Add("Se agrega nuevo record status");
                        oLog.Add("Status: " + New_ticket_Record.id_Status);
                    }
                    if (StatusChanged)
                    {                        
                        db.Comentario.Add(new Comentario()
                        {
                            Comentario1 = (GetTemplateStatus((int)ticket.Status) + Comentario.Comentario1),
                            id_User = ((User)Session["User"]).id,
                            CreatedAt = DateTime.Now
                        });
                        
                        db.SaveChanges();
                        oLog.Add("Guardo comentario: " + Comentario.Comentario1);
                        oLog.Add("Usuario asociado: " + ((User)Session["User"]).nombre);
                    }                
                    else
                    {
                        db.Comentario.Add(new Comentario()
                        {

                            Comentario1 = Comentario.Comentario1,
                            id_User = ((User)Session["User"]).id,
                            CreatedAt = DateTime.Now
                        });
                        db.SaveChanges();
                        oLog.Add("Guardo comentario: " + Comentario.Comentario1);
                        oLog.Add("Usuario asociado: " + ((User)Session["User"]).nombre);
                    }
                          
                    var idComentario = db.Comentario.FirstOrDefault(c => c.Comentario1.Contains(Comentario.Comentario1)).id;
                    if (idComentario != 0)
                    {
                        db.Ticket_Comentario.Add(new Ticket_Comentario()
                        {
                            id_Ticket = ticket.id,
                            id_Comentario = idComentario
                        });
                        db.SaveChanges();
                        oLog.Add("Guardo una relacion entre el ticket y el comentario");
                    }

                    if (ticket.id_Usuario != db.Ticket_User.Where(x => x.id_Ticket == ticket.id).OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User)
                    {
                        
                        Ticket_User ticket_User = new Ticket_User();
                        ticket_User.id_Ticket = ticket.id;
                        ticket_User.id_User = (int)ticket.id_Usuario;
                        ticket_User.CreatedAt = DateTime.Now;

                        db.Ticket_User.Add(ticket_User);
                        db.SaveChanges();
                        
                        var Ticket = db.Ticket.Find(ticket.id);
                        oLog.Add("id_User: " + ticket.id_Resp);
                        ticket.id_Resp = Ticket.id_User;
                        //Ticket.id_User = ticket.id_Resp;

                        db.Entry(Ticket).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        //Logs
                        oLog.Add("actual Responsable user id : " + db.Ticket_User.OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User);
                        oLog.Add("Se agrega una relacion entre usuario responsable y ticket");                                             
                        oLog.Add("Nuevo id User: " + ticket_User.id_User);
                    }

                    sendEmailUpdateTK(ticket.id, (int)ticket.id_Usuario, ((User)Session["User"]).id);
                }
            }
            catch(Exception ex)
            {
                Comentarios comentario = new Comentarios();
                comentario.id_User = ((User)Session["User"]).id;
                comentario.Comentario1 = GetTemplateComent();
                ticket.Comentarios = new List<Comentarios>();
                ticket.Comentarios.Add(comentario);
                ViewBag.ExceptionMessage = ex.Message;
                GetStatus((int)ticket.Status);
                GetUsuarios();
                //Add Log
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add("Excepcion agregando un comentario");
                oLog.Add(ex.Message);
                oLog.Add("Usuario: " + ((User)Session["User"]).nombre);
                oLog.Add("Ticket id: " + ticket.id);
                oLog = null;
                return View(ticket);
            }
            
            return Redirect("~/Tickets/ShowTicket/" + ticket.id);
        }

        /// <summary>
        /// Devuelve a la vista el ticket con el ultimo usuario y el ultimo status para ser editado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(idOperacion: 2)]
        public ActionResult EditTicket(int id)
        {
            var ticket = new TicketViewModel();
            using(HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var t = db.Ticket.Find(id);
                ticket.id = t.id;
                ticket.titulo = t.titulo;
                ticket.id_Terminal = t.id_Terminal;
                ticket.id_Categoria = t.id_Categoria;
                ticket.id_Subsistema = t.id_Subsistema;
                ticket.id_externo = t.id_externo;
                ticket.id_Resp = t.Ticket_User.OrderByDescending(u => u.CreatedAt).First().id_User;
                ticket.Status = t.Ticket_Record_Status.Where(s => s.id_Ticket == t.id).OrderByDescending(s => s.CreatedAt).FirstOrDefault().id_Status;
                ticket.mensaje = t.mensaje;
            }
            GetCategories();
            GetTerminales();
            GetUsuarios();
            GetStatusEditTicket((int)ticket.Status);
            GetSubsistemas();
            return View(ticket);
            
        }

        /// <summary>
        /// Guarda en la Base de datos el Ticket con los nuevos cambios, si cambio de 
        /// status genera un registro de record status del ticket, si se cambio de responsable del ticket
        /// se agrega un nuevo usuario
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUser(idOperacion: 2)]
        public ActionResult EditTicket(TicketViewModel ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string path = Server.MapPath("~/Logs/Tickets/");
                    Log oLog = new Log(path);
                    oLog.Add("Edit Ticket: " + ticket.id);
                    using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                    {
                        var Ticket = db.Ticket.Find(ticket.id);
                        oLog.Add("Titulo: " + Ticket.titulo);
                        Ticket.titulo = ticket.titulo;
                        oLog.Add("Terminal: " + Ticket.id_Terminal);
                        Ticket.id_Terminal = ticket.id_Terminal;
                        oLog.Add("Categoria: " + Ticket.id_Categoria);
                        Ticket.id_Categoria = ticket.id_Categoria;
                        oLog.Add("Subsistema: " + Ticket.id_Subsistema);
                        Ticket.id_Subsistema = ticket.id_Subsistema;
                        oLog.Add("Usuario: " + Ticket.id_User);
                        Ticket.id_User = ((User)Session["User"]).id;
                        oLog.Add("Status: " + Ticket.status);
                        Ticket.status = ticket.Status;
                        oLog.Add("Mensaje: " + Ticket.mensaje);
                        Ticket.mensaje = ticket.mensaje;
                        oLog.Add("Identificador: " + Ticket.id_externo);
                        Ticket.id_externo = ticket.id_externo;

                        db.Entry(Ticket).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        oLog.Add("Se guardan cambios en el Ticket");
                        oLog.Add("Nuevo Titulo: " + ticket.titulo);
                        oLog.Add("Nuevo Terminal: " + ticket.id_Terminal);
                        oLog.Add("Nuevo Categoria: " + ticket.id_Categoria);
                        oLog.Add("Nuevo Subsistema: " + ticket.id_Subsistema);
                        oLog.Add("Nuevo Usuario: " + Ticket.id_User);
                        oLog.Add("Nuevo Mensaje: " + ticket.mensaje);
                        oLog.Add("Nuevo Identificador: " + ticket.id_externo);
                    }
                    //Envia correo sobre la actualizacion del ticket
                    sendEmailUpdateTK(ticket.id, (int)ticket.id_Resp, ((User)Session["User"]).id);
                    return Redirect("~/Tickets/ShowTicket/" + ticket.id);
                }
                else
                {
                    GetCategories();
                    GetTerminales();
                    GetUsuarios();
                    GetStatus((int)ticket.Status);
                    GetSubsistemas();
                    return View(ticket);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                string Message = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                        Message += "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage + "\n";
                    }
                }
                ViewBag.ExceptionMessage = Message;
                //Add Logs
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add("DbEntityValidationException: " + Message);

                return View(ticket);
            }
            catch (Exception ex)
            {
                GetCategories();
                GetTerminales();
                GetUsuarios();
                GetStatus((int)ticket.Status);
                GetSubsistemas();
                ViewBag.ExceptionMessage = ex.Message;
                //Add Logs
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add("Excepcion: " + ex.Message);
                return View(ticket);

            }
        }

        /// <summary>
        /// Guarda una imagen en un directorio espacificado.
        /// En la base de datos guarda la relacion con el ticket y la ruta y nombre 
        /// </summary>
        /// <param name="IdTicket"></param>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [AuthorizeUser(idOperacion: 7)]
        public ActionResult AddFiles(int IdTicket , HttpPostedFileBase postedFile)
        {
            try
            {
                string filepath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/TicketFiles/" + IdTicket + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filepath = path + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filepath);
                }
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    Files file = new Files();
                    file.Nombre = postedFile.FileName;
                    file.URL = filepath;
                    file.Tipo = postedFile.GetType().Name;

                    db.Files.Add(file);
                    db.SaveChanges();

                    Tickets_Files tickets_Files = new Tickets_Files();
                    tickets_Files.id_Ticket = IdTicket;
                    tickets_Files.id_File = db.Files.FirstOrDefault(f => f.Nombre == postedFile.FileName).id;

                    db.Tickets_Files.Add(tickets_Files);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~/Logs/Tickets/");
                Log oLog = new Log(path);
                oLog.Add("Excepcion on ticket " + IdTicket + ": " + ex.Message);
                ViewBag.Exception = ex.Message;
            }
            return Redirect("~/Tickets/ShowTicket/"+IdTicket);
        }

        /// <summary>
        /// Metodo que se encarga de descargar el archivo del ticket seleccionado
        /// </summary>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        public FileResult DonloadTicketFile(int id)
        {
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var file = db.Files.FirstOrDefault(f => f.id == id);
                string NameFile = file.Nombre;
                string rute = Server.MapPath("~/TicketFiles/" + NameFile);
                return File(rute, "image/jpeg", NameFile);
            }
        }

        /// <summary>
        /// Devuelve a la vista una lista de las categorias del ticket 
        /// </summary>
        private void GetCategories()
        {
            List<SelectListItem> Categorias = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1 ())
            {
                var aux = (from s in db.Categoria select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Categorias.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()
                            
                        });
                    }
                }
            }
            ViewBag.Categorias = Categorias;
        }

        /// <summary>
        /// Devuelve a la vista una lista de las Terminales del ticket 
        /// </summary>
        private void GetTerminales()
        {

            List<SelectListItem> Terminales = new List<SelectListItem>();
            Terminales.Add(new SelectListItem
            {
                Text = "Seleccione una Terminal",
                Value = "08",
                Selected = true
            });

            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Terminal select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Terminales.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Terminales = Terminales;

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
        /// Devuelve a la vista una lista de los status
        /// </summary>
        private void GetStatus( int status)
        {

            List<SelectListItem> Status = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Status select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        if (a.Status1 == status)
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Selected  = true
                                //Disabled = true                                
                            });
                        }
                        else
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString()
                                //Disabled = true
                            });
                        } 
                    }
                }
            }
            ViewBag.Status = Status;

        }

        /// <summary>
        /// Devuelve a la vista una lista de los status deshabilitada para su edicion 
        /// </summary>
        private void GetStatusEditTicket(int status)
        {

            List<SelectListItem> Status = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Status select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        if (a.Status1 == status)
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Selected = true,
                                //Disabled = true                                
                            });
                        }
                        else
                        {
                            Status.Add(new SelectListItem
                            {
                                Text = a.descripcion,
                                Value = a.Status1.ToString(),
                                Disabled = true
                            });
                        }
                    }
                }
            }
            ViewBag.Status = Status;

        }

        /// <summary>
        /// Devuelve a la vista una lista de los Subsistemas
        /// </summary>
        private void GetSubsistemas()
        {

            List<SelectListItem> Subsistemas = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Subsistema select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Subsistemas.Add(new SelectListItem
                        {
                            Text = a.Nombre,
                            Value = a.id.ToString()
                        });                        

                    }
                }
            }
            ViewBag.Subsistemas = Subsistemas;

        }

        /// <summary>
        /// Devuelve a la vista un templete para redactar el ticket
        /// </summary>
        private string GetTemplatemessage()
        {            
            StringBuilder sb = new StringBuilder();
          
            sb.Append("");
            sb.AppendLine("->Fecha: " + DateTime.Now);
            sb.AppendLine("->Reportado por:" + ((User)Session["User"]).nombre);
            sb.AppendLine("->Reporte: .....");
            sb.AppendLine("..............");
            sb.AppendLine("->Problematica: ");
            sb.AppendLine(".........");
            sb.AppendLine(".........");
            return sb.ToString();
        }

        /// <summary>
        /// Establece un template del comentario a ser agregadp
        /// </summary>
        /// <returns></returns>
        private string GetTemplateComent()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.AppendLine("->Fecha: " + DateTime.Now);
            sb.AppendLine("->Reportado por:" + ((User)Session["User"]).nombre);
            sb.AppendLine("->Reporte:");
            sb.AppendLine(".......");
            sb.AppendLine(".......");
            sb.AppendLine("->Acciones a realizar: ");
            sb.AppendLine(".........");
            sb.AppendLine(".........");
            return sb.ToString();
        }

        /// <summary>
        /// Establece catalogo del Status Deshabilitado
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        private string GetTemplateStatus(int Status)
        {
            StringBuilder sb = new StringBuilder();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.Status where s.Status1 == Status select s);
                sb.Append("\n");
                sb.AppendLine("*****************************************************");
                sb.AppendLine("* Cambio de Status a:" + aux.FirstOrDefault().descripcion);
                sb.AppendLine("*****************************************************");
            }         
            return sb.ToString();
        }


        /// <summary>
        /// Este metodo obtiene los datos de los tickets existentes
        /// </summary>
        /// <returns> FilterTicketsViewModel </returns>
        [HttpGet]
        public ActionResult Filter_Tickets()
        {
            FilterTicketsViewModel Filter = new FilterTicketsViewModel() { id_Terminal = 1};
            GetTerminales();
            GetSubsistemas();
            GetStatus(1);
            GetCategories();
            GetUsuarios();
            return View(Filter);
        }

        /// <summary>
        /// Devuelve a la vista un objeto para filtrar el ticket
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Filter_Tickets(FilterTicketsViewModel Filter)
        {
            GetTerminales();
            GetSubsistemas();
            GetStatus(1);
            GetCategories();
            GetUsuarios();
            //se declara la lista que sera llenada con el resultado del filtro
            List<ListbyFilterTicket> currentLists = new List<ListbyFilterTicket>();

            if (ModelState.IsValid) //Valida el modelo
            {
                if (Filter.just_closed && Filter.is_closed)
                {
                    //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                    ViewBag.warning = "Query Error: No puedes seleccionar los dos ultimos checkbox juntos para realizar una busqueda ";
                    return View(Filter);
                }
                using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
                {
                    if (Filter.isSelected_id) //si el Id fue seleccionado
                    {
                        //realiza la busqueda por el ID
                        Ticket filter = db.Ticket.Find(Filter.id);
                        if (filter == null) // si la busqueda por el id es null
                        {
                            //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                            ViewBag.warning = "Ningun registro coicide con el id: " + Filter.id;
                            return View(Filter);
                        }
                        //agrega a la lista en curso 
                        currentLists.Add(new ListbyFilterTicket() { id = filter.id });
                    }
                    else
                    {
                        var listFilter = new List<Ticket>();

                        if (Filter.is_closed)
                        {
                            listFilter = db.Ticket.Take(92).ToList();
                        }
                        else if (Filter.just_closed)
                        {
                            listFilter = db.Ticket.Where(x => x.status == 12).Take(92).ToList();
                        }
                        else
                        {
                            listFilter = db.Ticket.Where(x => x.status != 12).Take(92).ToList();
                        }
                        if (Filter.isSelected_Terminal)
                        {
                            listFilter = listFilter.Where(t => t.id_Terminal == Filter.id_Terminal).ToList();
                            if (listFilter.Count() < 1)
                            {
                                //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                                ViewBag.warning = "Ningun registro coicide con la terminal: " + Filter.id_Terminal;
                                return View(Filter);

                            }
                        }
                        if (Filter.isSelected_Categ)
                        {
                            listFilter = listFilter.Where(t => t.id_Categoria == Filter.id_Categoria).ToList();
                            if (listFilter.Count() < 1)
                            {
                                //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                                ViewBag.warning = "Ningun registro coicide con la Categoría: " + Filter.id_Categoria;
                                return View(Filter);

                            }
                        }
                        if (Filter.isSelected_subsistema)
                        {
                            listFilter = listFilter.Where(t => t.id_Subsistema == Filter.id_Subsistema).ToList();
                            if (listFilter.Count() < 1)
                            {
                                //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                                ViewBag.warning = "Ningun registro coicide con el Subsistema: " + Filter.id_Subsistema;
                                return View(Filter);

                            }
                        }
                        if (Filter.isSelected_Status)
                        {
                            listFilter = listFilter.Where(t => t.status == Filter.status).ToList();
                            if (listFilter.Count() < 1)
                            {
                                //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                                ViewBag.warning = "Ningun registro coicide con el estatus: " + Filter.status;
                                return View(Filter);

                            }
                        }

                        if (Filter.isSelected_User)
                        {
                            //Aquí también se establece un límite de 40 tickets a enviar a la lista de la vista
                            listFilter = listFilter.Where(t => t.id_User == Filter.id_User).Take(40).ToList();
                            if (listFilter.Count() < 1)
                            {
                                //devuelve a la vista el modelo y un mensaje de busqueda sin resultados
                                ViewBag.warning = "Ningun registro coicide con usuario: " + Filter.id_User;
                                return View(Filter);

                            }
                        }

                        //el resultado de la busqueda
                        foreach (var item in listFilter)
                        {
                            currentLists.Add(new ListbyFilterTicket() { id = item.id });
                        }
                    }
                }
            }
            else
            {
                ViewBag.warning = "Filtro no valido";
                return View(Filter);
            }

            // Objeto a enviar
            string encodedCurrentList = "";
            // Serialización y codificación
            var serializedObject = JsonConvert.SerializeObject(currentLists);
            encodedCurrentList = HttpUtility.UrlEncode(serializedObject);
            // Creación de la URL con la cadena de consulta
            var url = "IndexWithFilter/?encodedCurrentList=" + encodedCurrentList;
            // Redirecciona a la URL 
            return Redirect(url);
            //return View("IndexWithFilter", encodedCurrentList);
        }

        /// <summary>
        /// Metodo que envia correo de un nuevo ticket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_subjet"></param>
        /// <param name="id_from"></param>
        public void sendEmailUpdateTK(int idtk , int id_subjet , int id_from)
        {
            try
            {
                //logs
                string path = Server.MapPath("~/Logs/Emails/");
            Log oLog = new Log(path);
            TicketViewModel ticket = new TicketViewModel();
            string origen, destinatario;
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var subjet = (from u in db.User where u.id == id_subjet select u).FirstOrDefault();
                destinatario = subjet.email;
                var from = (from u in db.User where u.id == id_from select u).FirstOrDefault();
                origen = from.email;
                var tk = db.Ticket.Find(idtk);
                ticket.titulo = tk.titulo;
                    // Obtener el último comentario del ticket
                    var lastComment = db.Ticket_Comentario
                                        .Where(x => x.id_Ticket == tk.id)
                                        .OrderByDescending(x => x.id)
                                        .FirstOrDefault();

                    // Asignar el mensaje del ticket basado en la existencia del último comentario
                    ticket.mensaje = lastComment?.Comentario.Comentario1 ?? tk.mensaje;
                    //Remplaza el contenido del mensaje. 
                    contenidoHtml = contenidoHtml.Replace("{usuarioName}", subjet.nombre)
                             .Replace("{ticketId}", tk.id.ToString())
                             .Replace("{titulo}", tk.titulo.ToString())
                             .Replace("{estado}", "Pendiente")
                             .Replace("{ultimoComentario}", tk.mensaje)
                             .Replace("{enlaceTicket}", "https://pts-tools.com.mx/Tickets/ShowTicket/" + tk.id);                
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
                Subject = ticket.titulo,
                Body = contenidoHtml,
                IsBodyHtml = true // Si el cuerpo del correo es HTML
            };

            // Añadir destinatario
            mensaje.To.Add(destinatario);
            // Añadir en copia (CC)
            mensaje.CC.Add(origen);
            // Enviar el correo
            clienteSmtp.Send(mensaje);
            oLog.Add("---------------------------");
            oLog.Add($"Correo enviado exitosamente a {destinatario} sobre actualización del ticket.");
            oLog.Add($"Asunto: {ticket.titulo} ");
            oLog.Add($"Cuerpo: {ticket.mensaje} ");
            //Devuelve un mensaje exitoso a la vista 
            ViewBag.InfoMessage = $"Correo enviado exitosamente a {destinatario} sobre actualización del ticket.";
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

    }
}