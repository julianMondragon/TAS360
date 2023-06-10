using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAS360.Models;

namespace TAS360.Models.ViewModel
{
    public class PendientesViewModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Clasificación")]
        public int id_Clasificacion { get; set; }
        [Display(Name = "Observación")]
        public string Observacion { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Rango 1 a 100")]
        [Display(Name = "% de avance")]
        public double Avance { get; set; }
        [Required]
        [Display(Name = "Prioridad")]
        public int id_Prioridad { get; set; }
        [Required]
        [Display(Name = "Acitividades pendientes por el fabricante (SUSESS)")]
        public string Actividades_Pend_Susess { get; set; }
        [Required]
        public string Responsable { get; set; }
        [Required]
        [Display(Name = "Fecha de creacion")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Display(Name = "Fecha Compromiso de entrega")]
        public DateTime Fecha_Compromiso { get; set; }
        [Required]
        [Display(Name = "Usuario")]
        public int id_User { get; set; }
        [Display(Name = "Ticket relacionado")]
        public int id_Ticket { get; set; }
        [Required]
        [Display(Name = "Terminal de Almacenamiento y Reparto")]
        public int id_Terminal { get; set; }
        [Required]
        [Display(Name = "Subsistema relacionado")]
        public int id_Subsistema { get; set; }
        [Required]
        public Clasificacion_Pendiente Clasificacion_Pendiente { get; set; }
        [Required]
        [Display(Name = "Usuario relacionado")]
        public User User { get; set; }
        [Required]
        [Display(Name = "Subsistema relacionado")]
        public Subsistema Subsistema { get; set; }
        [Required]
        [Display(Name = "Status")]
        public int id_status {get ; set; }



    }
}