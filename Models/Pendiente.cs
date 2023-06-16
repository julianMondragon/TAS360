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
    
    public partial class Pendiente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pendiente()
        {
            this.Pendiente_Record_Status = new HashSet<Pendiente_Record_Status>();
        }
    
        public int id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> id_Clasificacion { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> Avance { get; set; }
        public Nullable<int> id_Prioridad { get; set; }
        public string Actividades_Pend_Susess { get; set; }
        public string Responsable { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> Fecha_Compromiso { get; set; }
        public Nullable<int> id_User { get; set; }
        public Nullable<int> id_Ticket { get; set; }
        public Nullable<int> id_Terminal { get; set; }
        public Nullable<int> id_Subsistema { get; set; }
        public Nullable<int> id_status { get; set; }
        public Nullable<bool> is_deleted { get; set; }
    
        public virtual Clasificacion_Pendiente Clasificacion_Pendiente { get; set; }
        public virtual Prioridad_de_Pendiente Prioridad_de_Pendiente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pendiente_Record_Status> Pendiente_Record_Status { get; set; }
        public virtual Status Status { get; set; }
        public virtual Subsistema Subsistema { get; set; }
        public virtual Terminal Terminal { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
    }
}
