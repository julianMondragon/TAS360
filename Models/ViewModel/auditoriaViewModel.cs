using System; 
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class auditoriaViewModel
    {
        public auditoriaViewModel()
        {
            auditoria = new List<Auditoria>();
        }
        public List<Auditoria> auditoria { get; set; }

        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Estado { get; set; }
        public string FlujoDatos { get; set; }
        public string Secuencial { get; set; }
        public string Transaccion { get; set; }

    }
}
