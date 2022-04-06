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
        [StringLength(5, ErrorMessage = "El codigo de operación debe ser de 5 digitos.")]
        [MinLength(length: 5, ErrorMessage = "El codigo de operación debe ser de 5 digitos.")]
        public string Numero_Operacion { get; set; }

        [Required]
        [Display(Name = "Compartimiento")]
        public string Numero_Compartimiento { get; set; }

        [Required] 
        [Display(Name = "Estado de transaccion")]
        public string Estado_Transaccion { get; set; }

        [Required]
        [Display(Name = "Razones")]
        [StringLength(2, ErrorMessage = "El codigo de razones debe ser de 2 digitos.")]
        [MinLength(length: 2, ErrorMessage = "El codigo de razones debe ser de 2 digitos.")]
        public string Razones { get; set; }

        [Required]
        [Display(Name = "Modulo Operacion")]
        public string Modulo_Operacion { get; set; }

        [Required]
        [Display(Name = "Volumen natural")]
        [StringLength(11, ErrorMessage = "El codigo de voluumen natural debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de voluumen natural debe ser de 11 digitos.")]
        public string volumen_natural { get; set; }

        [Required]
        [Display(Name = "Volumen neto")]
        [StringLength(11, ErrorMessage = "El codigo de voluumen neto debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de voluumen neto debe ser de 11 digitos.")]
        public string volumen_neto { get; set; }

        [Required]
        [Display(Name = "Temperatura")]
        [StringLength(5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9.")]
        [MinLength(length: 5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9.")]
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
        [StringLength(2, ErrorMessage = "El codigo de posicion de carga debe ser de 2 digitos.")]
        [MinLength(length: 2, ErrorMessage = "El codigo de posicion de carga debe ser de 2 digitos.")]
        public string posicion_carga { get; set; }

        [Required]
        [Display(Name = "Flujo promedio")]
        [StringLength(4, ErrorMessage = "El codigo de flujo promedio debe ser de 4 digitos.")]
        [MinLength(length: 4, ErrorMessage = "El codigo de flujo promedio debe ser de 4 digitos.")]
        public string flujo_promedio { get; set; }

        [Required]
        [Display(Name = "Factor de medicion")]
        [StringLength(6, ErrorMessage = "El codigo de factor de medicion debe ingresarse como 9.9999 .")]
        [MinLength(length: 6, ErrorMessage = "El codigo de factor de medicion debe ingresarse como 9.9999 .")]
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
        [StringLength(3, ErrorMessage = "El codigo de numero de tanque debe ser de 3 digitos.")]
        [MinLength(length: 3, ErrorMessage = "El codigo de numero de tanque debe ser de 3 digitos.")]
        public string numero_tanque { get; set; }

        [Required]
        [Display(Name = "Codigo de Producto Anterior")]
        public string codigo_Anterior_producto { get; set; }

        [Required]
        [Display(Name = "Volumen Total Neto")]
        [StringLength(11, ErrorMessage = "El codigo de volumen total neto debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de volumen total neto debe ser de 11 digitos.")]
        public string volumen_total_neto { get; set; }

        [Required]
        [Display(Name = "Volumen neto")]
        [StringLength(11, ErrorMessage = "El codigo de volumen neto debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de volumen neto debe ser de 11 digitos.")]
        public string volumen_neto { get; set; }

        [Required]
        [Display(Name = "Volumen natural")]
        [StringLength(11, ErrorMessage = "El codigo de volumen natural debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de volumen natural debe ser de 11 digitos.")]
        public string volumen_natural { get; set; }

        [Required]
        [Display(Name = "Temperatura promedio")]
        [StringLength(5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9 .")]
        [MinLength(length: 5, ErrorMessage = "La temperatura debe registrarse como 99.99 o 999.9 .")]
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

    public class TramaCancelacionOrdenesViewModel
    {
        [Required]
        [Display(Name = "Tipo de Transaccion")]
        public string Tipo_Transaccion { get; set; }

        [Required]
        [Display(Name = "Numero de Operacion")]
        [StringLength(5, ErrorMessage = "El codigo de operación debe ser de 5 digitos.")]
        [MinLength(length: 5, ErrorMessage = "El codigo de operación debe ser de 5 digitos.")]
        public string Numero_Operacion { get; set; }

        [Required]
        [Display(Name = "Compartimiento")]
        public string Numero_Compartimiento { get; set; }

        [Required]
        [Display(Name = "Estado de transaccion")]
        public string Estado_Transaccion { get; set; }

        [Required]
        [Display(Name = "Razones")]
        [StringLength(2, ErrorMessage = "El codigo de razones debe ser de 2 digitos.")]
        [MinLength(length: 2, ErrorMessage = "El codigo de razones debe ser de 2 digitos.")]
        public string Razones { get; set; }

        [Required]
        [Display(Name = "Modulo Operacion")]
        public string Modulo_Operacion { get; set; }

        [Required]
        [Display(Name = "Identificador de vehiculo")]
        [StringLength(11, ErrorMessage = "El codigo del vehiculo debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo del vehiculo debe ser de 11 digitos.")]
        public string Identificador_Vehiculo { get; set; }

        [Required]
        [Display(Name = "Codigo de Producto Anterior")]
        public string codigo_Anterior_producto { get; set; }

        [Required]
        [Display(Name = "Volumen natural programado")]
        [StringLength(11, ErrorMessage = "El codigo de volumen natural debe ser de 11 digitos.")]
        [MinLength(length: 11, ErrorMessage = "El codigo de volumen natural debe ser de 11 digitos.")]
        public string volumen_natural { get; set; }

        [Required]
        [Display(Name = "Codigo nuevo de Producto.")]
        public string codigo_nuevo_producto { get; set; }
    }
}