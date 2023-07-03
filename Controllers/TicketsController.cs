using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TAS360.Filters;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class TicketsController : Controller
    {
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
                var Tickets = (from s in db.Ticket select s);
                if (Tickets != null && Tickets.Any())
                {
                    foreach (var t in Tickets)
                    {
                        tickets.Add(new TicketViewModel
                        {
                            id = t.id,
                            titulo = t.titulo,
                            mensaje = t.mensaje,
                            usuario_name = t.Ticket_User.OrderByDescending(x => x.CreatedAt).FirstOrDefault().User.nombre,
                            categoria_name = t.Categoria.nombre,
                            status_name = t.Ticket_Record_Status.OrderByDescending(x => x.CreatedAt).FirstOrDefault().Status.descripcion,
                            Subsistema_name = t.Subsistema.Nombre,
                            Status =  t.status
                        });
                    }
                }
            }
            return View(tickets);
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

                        //db.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
                        db.Ticket.Add(ticket);
                        db.SaveChanges();

                        int idTicket = db.Ticket.FirstOrDefault(a => a.titulo == model.titulo).id;
                        oLog.Add("Se creo el Ticket: " + idTicket );
                        oLog.Add("Titulo del Ticket: " + model.titulo);
                        oLog.Add("Usuario creador: " + ((User)Session["User"]).nombre);
                        if (idTicket != 0)
                        {
                            Ticket_Record_Status New_ticket_Record = new Ticket_Record_Status();
                            New_ticket_Record.id_Ticket = idTicket;
                            New_ticket_Record.id_Status = ticket.status;

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

                    return Redirect("~/Tickets/Index");
                }
                else
                {
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
                    ticket.id_Resp = t.Ticket_User.OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User;
                    

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

                    if (ticket.id_Resp != db.Ticket_User.OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User)
                    {
                        
                        Ticket_User ticket_User = new Ticket_User();
                        ticket_User.id_Ticket = ticket.id;
                        ticket_User.id_User = (int)ticket.id_Resp;
                        ticket_User.CreatedAt = DateTime.Now;

                        db.Ticket_User.Add(ticket_User);
                        db.SaveChanges();
                        //Logs
                        oLog.Add("actual Responsable user id : " + db.Ticket_User.OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User);
                        oLog.Add("Se agrega una relacion entre usuario responsable y ticket");                                             
                        oLog.Add("Nuevo id User: " + ticket_User.id_User);
                    }
                    
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

                        db.Entry(Ticket).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        oLog.Add("Se guardan cambios en el Ticket");
                        oLog.Add("Nuevo Titulo: " + ticket.titulo);
                        oLog.Add("Nuevo Terminal: " + ticket.id_Terminal);
                        oLog.Add("Nuevo Categoria: " + ticket.id_Categoria);
                        oLog.Add("Nuevo Subsistema: " + ticket.id_Subsistema);
                        oLog.Add("Nuevo Usuario: " + Ticket.id_User);
                        oLog.Add("Nuevo Mensaje: " + ticket.mensaje);
                    }

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
            string filepath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/TicketFiles/");
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
            sb.AppendLine("Fecha: dd/mm/yyyy");
            sb.AppendLine("Reportado por: .....");
            sb.AppendLine("");
            sb.AppendLine("Reporte: .....");
            sb.AppendLine("..............");
            sb.AppendLine("Problematica: ");
            sb.AppendLine(".........");
            sb.AppendLine(".........");
            sb.AppendLine("Acciones a realizar: ");
            sb.AppendLine(".........");
            sb.AppendLine(".........");
            return sb.ToString();
        }

        /// <summary>
        /// establece un template del comentario a ser agregadp
        /// </summary>
        /// <returns></returns>
        private string GetTemplateComent()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("");
            sb.AppendLine("Fecha: dd/mm/yyyy");
            sb.AppendLine("");
            sb.AppendLine("Reporte:");
            sb.AppendLine(".......");
            sb.AppendLine(".......");
            sb.AppendLine("Acciones a realizar: ");
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
    }
}