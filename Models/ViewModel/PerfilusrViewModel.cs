using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class PerfilusrViewModel
    {
        [Key]
        public int id_User { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [Display(Name = "Correo Electrónico")]
        public string email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Display(Name = "Teléfono")]
        public string Cel { get; set; }

        [Display(Name = "Género")]
        public string Género { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Foto de Usuario")]
        public byte[] Foto_usuario { get; set; }  // Cambié a byte[] ya que lo más común es que las imágenes se almacenen en un arreglo de bytes

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime fecha_nacimeiento { get; set; }

        [Display(Name = "Creado el")]
        public DateTimeFormat createAt { get; set; }

        [Display(Name = "Actualizado el")]
        public DateTimeFormat updateAt { get; set; }
    }

}
