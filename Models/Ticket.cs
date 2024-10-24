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
    
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            this.Pendiente = new HashSet<Pendiente>();
            this.Ticket_Comentario = new HashSet<Ticket_Comentario>();
            this.Ticket_Record_Status = new HashSet<Ticket_Record_Status>();
            this.Ticket_User = new HashSet<Ticket_User>();
            this.Tickets_Files = new HashSet<Tickets_Files>();
        }
    
        public int id { get; set; }
        public string titulo { get; set; }
        public string mensaje { get; set; }
        public Nullable<int> id_User { get; set; }
        public Nullable<int> id_Categoria { get; set; }
        public Nullable<int> id_Terminal { get; set; }
        public Nullable<int> id_Subsistema { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string id_externo { get; set; }
        public Nullable<int> id_prioridad { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pendiente> Pendiente { get; set; }
        public virtual Prioridad_de_Pendiente Prioridad_de_Pendiente { get; set; }
        public virtual Subsistema Subsistema { get; set; }
        public virtual Terminal Terminal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_Comentario> Ticket_Comentario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_Record_Status> Ticket_Record_Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_User> Ticket_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tickets_Files> Tickets_Files { get; set; }
    }
}
