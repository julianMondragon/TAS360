﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bdSimcot_Entities : DbContext
    {
        public bdSimcot_Entities()
            : base("name=bdSimcot_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aditivos> Aditivos { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<Componentes> Componentes { get; set; }
        public virtual DbSet<Entradas> Entradas { get; set; }
        public virtual DbSet<Recetas> Recetas { get; set; }
        public virtual DbSet<Salidas> Salidas { get; set; }
    }
}
