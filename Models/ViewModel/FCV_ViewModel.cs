using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TAS360.Models.ViewModel
{
    public class FCV_ViewModel
    {
        [Required]
        [Display(Name = "Temperatura Observada")]
        [Range(-18, 150, ErrorMessage = "Rango -18 a 150")]
        public float temp { get; set; }
        [Required]
        [Display(Name = "Densidad Observada")]
        //[RegularExpression(@"^\d+(\.\d{1,1})?$", ErrorMessage = "Formato #.#")]
        [Range(653, 1075, ErrorMessage = "Rango 653 a 1075")]
        public float dens { get; set; }
        [Required]
        [Display(Name = "Volumen Natural")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Rango 1 a 99999999.99")]
        public long volnat { get; set; }
        public double factor { get; set; }
        public double volcor { get; set; }
    }
}