//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAS360.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tanque_Voe
    {
        public int id { get; set; }
        public int id_tanque { get; set; }
        public int id_voe { get; set; }
    
        public virtual tanque tanque { get; set; }
        public virtual voe voe { get; set; }
    }
}
