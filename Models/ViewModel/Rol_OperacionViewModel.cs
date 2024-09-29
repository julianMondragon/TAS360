using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class Rol_OperacionViewModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public int id_rol { get; set; }
        [Required]
        [Display(Name = "Operacion")]
        public int id_operacion { get; set; }
    }
}