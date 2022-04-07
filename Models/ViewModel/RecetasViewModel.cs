using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class RecetasViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Nombre de la Receta")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Nombre del Producto")]
        public string Producto { get; set; }
        [Required]
        [Display(Name = "% de Producto")]
        public double? Porc_producto { get; set; }
        [Required]
        [Display(Name = "# de Brazo")]
        public int? Brazo { get; set; }
        [Required]
        [Display(Name = "Cantidad de Aditivo")]
        public double? Cantidad_Aditivo { get; set; }
        [Required]
        [Display(Name = "Razon de flujo")]
        public double? Razon_flujo { get; set; }
        [Required]
        [Display(Name = "Producto usando el inyector")]
        public double? Prod_Usando_iny { get; set; }
    }
}