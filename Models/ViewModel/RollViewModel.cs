using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class RollViewModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        [StringLength(60)]
        public string nombre { get; set; }
    }
}