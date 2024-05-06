using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class TerminalViewModel : TAS360.Models.Terminal
    {
        [Display(Name = "ID")]
        public int _id { get; set; }

        [Required]
        [Display(Name = "Clave")]
        [MaxLength(length: 3, ErrorMessage = "La Descripcion debe contener menos de 3 caracteres")]
        [MinLength(length: 3, ErrorMessage = "La Descripcion debe contener almenos 3 caracteres")]
        public string _Clave { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(length: 60, ErrorMessage = "La Descripcion debe contener menos de 60 caracteres")]
        [MinLength(length: 3, ErrorMessage = "La Descripcion debe contener almenos 3 caracteres")]
        public string _Nombre { get; set; }
    }
}