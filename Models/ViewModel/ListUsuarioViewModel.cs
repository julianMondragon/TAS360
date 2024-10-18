using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TAS360.Models.ViewModel
{
    public class ListUsuarioViewModel
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(60, ErrorMessage = "El nombre debe contener menos de 60 caracteres")]
        [MinLength(3, ErrorMessage = "El nombre debe contener al menos 3 caracteres")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden")]
        public string confirmPassword { get; set; }

        [Required]
        [Display(Name ="Roll")]
        public int Rolid { get; set; }



    }
}

