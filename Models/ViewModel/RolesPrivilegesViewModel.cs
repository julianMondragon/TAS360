using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS360.Models;

namespace TAS360.Models.ViewModel
{
    public class RolesPrivilegesViewModel
    {
        public RolesPrivilegesViewModel()
        {
            this.Rolls = new List<Roll>();
            this.Modules = new List<Modulo>();
            this.Rol_OperacionAdmin = new List<Roll_Operacion>();
            this.Rol_OperacionResp = new List<Roll_Operacion>();
            this.Rol_OperacionContac = new List<Roll_Operacion>();
            this.Rol_OperacionVist = new List<Roll_Operacion>();
            this.Operacions = new List<Operacion>();
        }
        public List<Roll> Rolls { get; set; }
        public List<Modulo> Modules { get; set; }
        public List<Roll_Operacion> Rol_OperacionAdmin { get; set; }
        public List<Roll_Operacion> Rol_OperacionResp { get; set; }
        public List<Roll_Operacion> Rol_OperacionContac { get; set; }
        public List<Roll_Operacion> Rol_OperacionVist { get; set; }
        public List<Operacion> Operacions { get; set; }
    }
}