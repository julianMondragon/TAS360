using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TAS360.Models.ViewModel
{
    public class ConnexionSQLViewModel
    {
        [Required]
        public string server { get; set; }
        [Required]
        public string user { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string namedatabase { get; set; }
    }
}