using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class TicketViewModel
    {
 
        public int id { get; set; }
        [Required]
        [Display(Name = "Titulo")]
        [StringLength(60)]
        public string titulo { get; set; }
        [Required]
        [Display(Name = "Reporte")]
        [StringLength(3000)]
        public string mensaje { get; set; }
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
        [Required]
        [Display(Name = "Subsistema")]
        public int? id_Subsistema { get; set; }
        public string Subsistema_name { get; set; }
        public DateTime? Date { get; set; }
        public List<Archivos> Files { get; set; }
        public List<Comentarios> Comentarios { get; set; }
        public List<string> RecordStatus { get; set; }

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
}