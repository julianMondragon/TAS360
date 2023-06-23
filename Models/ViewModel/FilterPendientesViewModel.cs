using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TAS360.Models.ViewModel
{
    public class FilterPendientesViewModel : IValidatableObject
    {
        
        public bool isSelected_id { get; set; }

        [Display(Name = " Buscar por Id")]
        [ConditionalRequired("isSelected_id", ErrorMessage = "El campo Id a buscar es requerido.")]
        [Range(1,200)]
        public int? id { get; set; }
        
        public bool isSelected_Descrp { get; set; }

        [Display(Name = "Buscar por Descripcion")]
        [ConditionalRequired("isSelected_Descrp", ErrorMessage = "El campo Descripcion es requerido.")]
        public string Descripcion { get; set; }

        public bool isSelected_Class { get; set; }

        [Display(Name = "Buscar por Clasificacion")]
        [ConditionalRequired("isSelected_Class", ErrorMessage = "El campo Clasificacion es requerido.")]
        public int id_Clasificacion { get; set; }

        public bool isSelected_Observ { get; set; }

        [Display(Name = "Buscar por Observacion")]
        [ConditionalRequired("isSelected_Observ", ErrorMessage = "El campo Observacion es requerido.")]
        public string Observacion { get; set; }

        public bool isSelected_advance { get; set; }

        [Display(Name = "Buscar x % de avance")]
        [ConditionalRequired("isSelected_advance", ErrorMessage = "El campo % de avance es requerido.")]
        [Range(0, 100, ErrorMessage = "Rango 0 a 100")]
        public double? Avance { get; set; }

        public bool isSelected_Prior { get; set; }

        [Display(Name = "Buscar por Prioridad")]
        [ConditionalRequired("isSelected_Prior", ErrorMessage = "El campo Prioridad es requerido.")]
        public int id_Prioridad { get; set; }

        public bool isSelected_Susess { get; set; }

        [Display(Name = "Buscar por Actividad x Susess")]
        [ConditionalRequired("isSelected_Susess", ErrorMessage = "El campo Actividades_Pend_Susess es requerido.")]
        public string Actividades_Pend_Susess { get; set; }

        public bool isSelected_Respons { get; set; }

        [Display(Name = "Buscar por Responsable")]
        [ConditionalRequired("isSelected_Respons", ErrorMessage = "El campo Responsable es requerido.")]
        public string Responsable { get; set; }

        public bool isSelected_User { get; set; }

        [Display(Name = "Buscar por usuario")]
        [ConditionalRequired("isSelected_User", ErrorMessage = "El campo Usuario propietario es requerido.")]
        public int? id_User { get; set; }

        public bool isSelected_Terminal { get; set; }

        [Display(Name = "Buscar por TADs")]
        [ConditionalRequired("isSelected_Terminal", ErrorMessage = "El campo Terminal es requerido.")]
        public int? id_Terminal { get; set; }

        public bool isSelected_subsistema { get; set; }

        [Display(Name = "Buscar por Subsistema")]
        [ConditionalRequired("isSelected_subsistema", ErrorMessage = "El campo Subsistema es requerido.")]
        public int? id_Subsistema { get; set; }

        public bool isSelected_status { get; set; }

        [Display(Name = "Buscar por Status")]
        [ConditionalRequired("isSelected_status", ErrorMessage = "El campo status es requerido.")]
        public int? id_status { get; set; }

        [Display(Name = "Es PAS")]
        public bool is_PAS { get; set; }

        [Display(Name = "Es PAF")]
        public bool is_PAF { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(id, new ValidationContext(this, null, null) { MemberName = "id" }, results);
            Validator.TryValidateProperty(id_Terminal, new ValidationContext(this, null, null) { MemberName = "id_Terminal" }, results);
            return results;
        }
    }

    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string dependentProperty;

        public ConditionalRequiredAttribute(string dependentProperty)
        {
            this.dependentProperty = dependentProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentPropertyValue = validationContext.ObjectType.GetProperty(dependentProperty)?.GetValue(validationContext.ObjectInstance) as bool?;
            if (dependentPropertyValue.HasValue && dependentPropertyValue.Value && value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}