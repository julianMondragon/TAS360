using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TAS360.Models.ViewModel
{
    public class ConfigTanqAditivoViewModel
    {
        [Required]
        [Display(Name = "Identificador del Tanque")]
        public string Tag { get; set; }
        [Required]
        [Display(Name = "Nombre del Aditivo")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Altura del Tanque")]
        public double? Altura_Tanq { get; set; }
        [Required]
        [Display(Name = "Nivel de alarma por bajo inventario")]
        public double? Niv_Bajo_Inv_Tanq { get; set; }
    }
}