using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Models.ViewModel
{
    // Clase que representa el modelo de vista para un ticket
    public class TicketViewModel
    {

        internal List<CurrentList> currentList;
        public TicketViewModel()
        {
            CurrentList2 = new List<CurrentList2>();
            ListbyFilterTicket = new List<ListbyFilterTicket>();
        }
        public int id { get; set; }
        [Required]
        [Display(Name = "Titulo")]
        [StringLength(60)]
        public string titulo { get; set; }
        [Required]
        [Display(Name = "Reporte")]
        [StringLength(3000)]
        [AllowHtml]
        public string mensaje { get; set; }
        public string LastComent { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public int? Status { get; set; }
        public string status_name { get; set; }
        [Required]
        [Display(Name = "Responsable")]
        public int? id_Resp { get; set; }
        public string Resp_name { get; set; }
        public int? id_Usuario { get; set; }
        public string usuario_name { get; set; }
        [Required]
        [Display(Name = "Terminal de Almacenamiento y Reparto")]
        public int? id_Terminal { get; set; }
        public string terminal_name { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public int? id_Categoria { get; set; }
        public string categoria_name { get; set; }
        //[Required]
        [Display(Name = "Prioridad")]
        public int? id_Prioridad { get; set; }
        public string Prioridad_name { get; set; }
        [Required]
        [Display(Name = "Subsistema")]
        public int? id_Subsistema { get; set; }
        public string Subsistema_name { get; set; }
        public DateTime? Date { get; set; }
        [Display(Name = "Fecha Compromiso")]
        public DateTime? Datetobedone { get; set; }
        public List<Archivos> Files { get; set; }
        public List<Comentarios> Comentarios { get; set; }
        public List<string> RecordStatus { get; set; }
        [Display(Name = "Identificador")]
        public string id_externo { get; set; }

        public bool is_selected { get; set; }
        public List<CurrentList2> CurrentList2 { get; set; }
        public List<ListbyFilterTicket> ListbyFilterTicket { get; set; }
    }

    public class Archivos
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string URL { get; set; }
        public string Tipo { get; set; }
    }

    public class Comentarios
    {
        public int id { get; set; }
        [StringLength(3000)]
        public string Comentario1 { get; set; }
        public int? id_User { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    // Clase que representa un elemento de la lista CurrentList2
    public class CurrentList2
    {
        public int id { get; set; }
        public bool is_selected { get; set; }
    }

    public class ListbyFilterTicket 
    {
        public int id { get; set; }
    }
}