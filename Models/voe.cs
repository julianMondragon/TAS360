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
    
    public partial class voe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public voe()
        {
            this.proceso_voes = new HashSet<proceso_voes>();
            this.Tanque_Voe = new HashSet<Tanque_Voe>();
        }
    
        public int id_voe { get; set; }
        public string nombre { get; set; }
        public Nullable<bool> modo { get; set; }
        public Nullable<int> marcha_paro { get; set; }
        public Nullable<int> control_cierre_val_entrada_poli { get; set; }
        public Nullable<int> control_cierre_val_entrada { get; set; }
        public Nullable<int> control_cierre_val_salida_tq { get; set; }
        public Nullable<int> control_apertura_val_entrada_poli { get; set; }
        public Nullable<int> control_apertura_val_entrada { get; set; }
        public Nullable<int> control_apertura_val_salida_tq { get; set; }
        public Nullable<int> control_paro_val_entrada_poli { get; set; }
        public Nullable<int> control_paro_val_entrada { get; set; }
        public Nullable<int> control_paro_val_salida_tq { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proceso_voes> proceso_voes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tanque_Voe> Tanque_Voe { get; set; }
    }
}
