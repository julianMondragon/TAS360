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
    
    public partial class Tickets_Files
    {
        public int id { get; set; }
        public Nullable<int> id_Ticket { get; set; }
        public Nullable<int> id_File { get; set; }
    
        public virtual Files Files { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
