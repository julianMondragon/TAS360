using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class WebAPIPemexViewModel
    {
        [Required]
        [Display(Name = "Fecha de Solicitud con un formato ddmmaaaa (01012023):")]
        //[RegularExpression(@"[0-9]", ErrorMessage = "Formato ddmmaaaa ej: 22012023")]
        
        public string date { get; set; }
    }
}