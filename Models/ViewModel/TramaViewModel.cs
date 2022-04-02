using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Models.ViewModel
{
    public class TramaConfOrdenCargaDescViewModel
    {       
        [Required]
        [Display(Name = "Tipo de Transaccion")]        
        public string Tipo_Transaccion { get; set; }

        [Required]
        [Display(Name = "Numero de Operacion")]
        [StringLength(5, ErrorMessage = "El codigo de operación no puede exeder los 5 digitos.")]
        public string Numero_Operacion { get; set; }

        [Required]
        [Display(Name = "Compartimiento")]
        public string Numero_Compartimiento { get; set; }

        [Required] 
        [Display(Name = "Estado de transaccion")]
        public string Estado_Transaccion { get; set; }

        [Required]
        [Display(Name = "Razones")]
        [StringLength(2, ErrorMessage = "El codigo de razones no debe exeder los 2 digitos.")]
        public string Razones { get; set; }

        [Required]
        [Display(Name = "Modulo Operacion")]
        public string Modulo_Operacion { get; set; }

        [Required]
        [Display(Name = "Volumen natural")]
        [StringLength(11, ErrorMessage = "El codigo de voluumen natural no debe exeder los 11 digitos.")]
        public string volumen_natural { get; set; }

        [Required]
        [Display(Name = "Volumen neto")]
        [StringLength(11, ErrorMessage = "El codigo de volumen neto no debe exeder los 2 digitos.")]
        public string volumen_neto { get; set; }

        [Required]
        [Display(Name = "Temperatura")]
        [StringLength(5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9.")]  
        public string temperatura { get; set; }

        [Required]
        [Display(Name = "Codigo Anterior de Producto")]
        public string Codigo_Anterior_producto { get; set; }

        [Required]
        [Display(Name = "Fecha de inicio")]
        public string Fecha_inicio { get; set; }

        [Required]
        [Display(Name = "Hora de inicio")]
        public string hora_inicio { get; set; }

        [Required]
        [Display(Name = "Fecha fin")]
        public string Fecha_fin { get; set; }

        [Required]
        [Display(Name = "Hora de fin")]
        public string hora_fin { get; set; }

        [Required]
        [Display(Name = "Posicion de carga")]
        [StringLength(2, ErrorMessage = "El codigo de posicion de carga no debe exeder los 2 digitos.")]
        public string posicion_carga { get; set; }

        [Required]
        [Display(Name = "Flujo promedio")]
        [StringLength(4, ErrorMessage = "El codigo de flujo promedio no debe exeder los 4 digitos.")]
        public string flujo_promedio { get; set; }

        [Required]
        [Display(Name = "Factor de medicion")]
        [StringLength(6, ErrorMessage = "El codigo de factor de medicion debe ingresarse como 9.9999 .")]
        public string factor_medicion { get; set; }

        [Required]
        [Display(Name = "Codigo nuevo de Producto")]
        public string Codigo_nuevo_producto { get; set; }
    }

    public class TramaVolumenesTanquesViewModel
    {
        [Required]
        [Display(Name = "Tipo de Transaccion")]
        public string Tipo_Transaccion { get; set; }

        [Required]
        [Display(Name = "Numero de tanque")]
        [StringLength(3, ErrorMessage = "El codigo de numero de tanque no puede exeder los 3 digitos.")]
        public string numero_tanque { get; set; }

        [Required]
        [Display(Name = "Codigo de Producto Anterior")]
        public string codigo_Anterior_producto { get; set; }

        [Required]
        [Display(Name = "Volumen Total Neto")]
        [StringLength(11, ErrorMessage = "El codigo de volumen total neto no puede exeder los 11 digitos.")]
        public string volumen_total_neto { get; set; }

        [Required]
        [Display(Name = "Volumen neto")]
        [StringLength(11, ErrorMessage = "El codigo de volumen neto no puede exeder los 11 digitos.")]
        public string volumen_neto { get; set; }

        [Required]
        [Display(Name = "Volumen natural")]
        [StringLength(11, ErrorMessage = "El codigo de volumen natural no puede exeder los 11 digitos.")]
        public string volumen_natural { get; set; }

        [Required]
        [Display(Name = "Temperatura promedio")]
        [StringLength(5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9 .")]
        public string temperatura_promedio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public string fecha { get; set; }

        [Required]
        [Display(Name = "Hora")]
        public string Hora { get; set; }

        [Required]
        [Display(Name = "Codigo nuevo de Producto.")]
        public string codigo_nuevo_producto { get; set; }
    }

}