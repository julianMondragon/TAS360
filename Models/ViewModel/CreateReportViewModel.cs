using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAS360.Models.ViewModel
{
    public class CreateReportViewModel
    {
        [Required]
        [Display(Name ="Tipo de Query")]
        public List<SelectListItem> TypeQuery { get; set; }
        [Required]
        public List<SelectListItem> whatRequest { get; set; }
        public List<SelectListItem> Table { get; set; }
        public List<SelectListItem>  condition { get; set; }
        public string conditionLogic { get; set; }
        public List<SelectListItem> adition { get; set; }
        public string aditionLogic { get; set; }
        public string otherQuery { get; set; }
        public string fynalQuery { get; set; }
    }
}