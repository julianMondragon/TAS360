using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class TramaConfOrdenCargaDescViewModel
    {
        [Required]
        [Display(Name = "Tipo de Transaccion")]
        
        public string Tipo_Transaccion { get; set; }

        [Required]
        [Display(Name = "Numero de Operacion")]
        [StringLength(5, ErrorMessage = "El {0} debe tener al menos {1} caracter", MinimumLength = 1)]
        public string Numero_Operacion { get; set; }

        [Required]
        [Display(Name = "Numero de compartimiento")]
        public string Numero_Compartimiento { get; set; }

        [Required]
        [Display(Name = "Estado de transaccion")]
        public string Estado_Transaccion { get; set; }

        [Required]
        [Display(Name = "Razones")]
        public string Razones { get; set; }

        [Required]
        [Display(Name = "Modulo Operacion")]
        public string Modulo_Operacion { get; set; }

        [Required]
        [Display(Name = "Volumen natural")]
        public double volumen_natural { get; set; }

        [Required]
        [Display(Name = "Volumen neto")]
        public double volumen_neto { get; set; }

        [Required]
        [Display(Name = "Temperatura")]
        public double temperatura { get; set; }

        [Required]
        [Display(Name = "Codigo Anterior de Producto")]
        public string codigo_Anterior_producto { get; set; }

        [Required]
        [Display(Name = "Fecha de inicio")]
        public DateTime Fecha_inicio { get; set; }

        [Required]
        [Display(Name = "Hora de inicio")]
        public string hora_inicio { get; set; }

        [Required]
        [Display(Name = "Fecha fin")]
        public DateTime Fecha_fin { get; set; }

        [Required]
        [Display(Name = "Hora de fin")]
        [StringLength(4, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 4)]
        public string hora_fin { get; set; }

        [Required]
        [Display(Name = "Posicion de carga")]
        public string posicion_carga { get; set; }

        [Required]
        [Display(Name = "Flujo promedio")]
        public string flujo_promedio { get; set; }

        [Required]
        [Display(Name = "Factor de medicion")]
        public string factor_medicion { get; set; }

        [Required]
        [Display(Name = "Codigo nuevo de Producto")]
        public string codigo_nuevo_producto { get; set; }

       
    }
}