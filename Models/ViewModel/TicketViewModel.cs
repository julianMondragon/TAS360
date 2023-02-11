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
        public string titulo { get; set; }
        [Required]
        [Display(Name = "Reporte")]
        public string mensaje { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public int? Status { get; set; }
        [Required]
        [Display(Name = "Soporte")]
        public int? id_Resp { get; set; }
        public int? id_Usuario { get; set; }
        [Required]
        [Display(Name = "Terminal de Almacenamiento y Reparto")]
        public int? id_Terminal { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public int? id_Categoria { get; set; }

    }
}