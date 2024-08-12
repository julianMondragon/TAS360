using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls.WebParts;
using TAS360.Models.ViewModel;
using System.Web.Mvc;

namespace TAS360.Models.ViewModel
{
    // Declaración de la clase FilterTicketsViewModel que valida el objeto
    public class FilterTicketsViewModel : IValidatableObject
    {
        //Aquí se encuentran las propiedades que indican si se han seleccionado realizar la busqueda 
        //por ID, Categoría, estatus, usuario, terminal y subsistema.
        public bool isSelected_id { get; set; }

        [Display(Name = " Buscar por Id")]
        [ConditionalRequired("isSelected_id", ErrorMessage = "El campo Id a buscar es requerido.")]
        [Range(1, 200)]
        public int? id { get; set; }

        public bool isSelected_Categ { get; set; }

        [Display(Name = "Buscar por Categoría")]
        [ConditionalRequired("isSelected_Categ", ErrorMessage = "El campo Categoría es requerido.")]
        public int? id_Categoria { get; set; }


        public bool isSelected_Status { get; set; }

        [Display(Name = "Buscar por estatus")]
        [ConditionalRequired("isSelected_Status", ErrorMessage = "El campo estatus es requerido.")]
        public int? status { get; set; }


        public bool isSelected_User { get; set; }

        [Display(Name = "Buscar por usuario")]
        [ConditionalRequired("isSelected_User", ErrorMessage = "El campo usuario es requerido.")]
        public int? id_User { get; set; }


        public bool isSelected_Terminal { get; set; }

        [Display(Name = "Buscar por TADs")]
        [ConditionalRequired("isSelected_Terminal", ErrorMessage = "El campo Terminal es requerido.")]
        public int? id_Terminal { get; set; }

        public bool isSelected_subsistema { get; set; }

        [Display(Name = "Buscar por Subsistema")]
        [ConditionalRequired("isSelected_subsistema", ErrorMessage = "El campo Subsistema es requerido.")]
        public int? id_Subsistema { get; set; }

        [Display(Name = "Incluir Tickets cerrados")]
        public bool is_closed { get; set; }

        [Display(Name = "Solo Tickets cerrados")]
        public bool just_closed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(id, new ValidationContext(this, null, null) { MemberName = "id" }, results);
            Validator.TryValidateProperty(id_Terminal, new ValidationContext(this, null, null) { MemberName = "id_Terminal" }, results);
            return results;
        }
    }
}