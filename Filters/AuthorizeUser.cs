using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;

namespace TAS360.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private User oUser;
        private HelpDesk_Entities1 db = new HelpDesk_Entities1();
        private int idOperacion;

        public AuthorizeUser(int idOperacion = 0)
        {
            this.idOperacion = idOperacion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            string nombreOperacion = "";
            string nombreModulo = "";

            try
            {              
               
                oUser = (User)HttpContext.Current.Session["User"];
                if (oUser == null)
                {
                    filterContext.Result = new RedirectResult("~/Acceso/Login");
                }
                else
                {
                    var MyOperationsList = from Op in db.Roll_Operacion
                                           where Op.id_Roll == oUser.id_Roll && Op.id_Operacion == idOperacion
                                           select Op;
                    if (MyOperationsList.ToList().Count() == 0)
                    {
                        var oOperation = db.Operacion.Find(idOperacion);
                        int? IdModulo = oOperation.id_Modulo;
                        nombreOperacion = getNombreDeOperacion(idOperacion);
                        nombreModulo = getNombreModulo(IdModulo);
                        filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "?modulo=" + nombreModulo + "?message=No tienes privilegios de acceso, verificalo con el administrador del sistema");
                    }
                }
                
            }
            catch(Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "?modulo=" + nombreModulo + "?message=" + ex.Message);
            }
            
        }

        private string getNombreDeOperacion(int id)
        {
            var nombre = from op in db.Operacion
                         where op.id == id 
                         select op.nombre;
            String Nombre = "";
            try
            {
                Nombre = nombre.First();
            }
            catch (Exception)
            {

            }

            return Nombre;
        }

        private string getNombreModulo(int? id)
        {
            var nombre = from m in db.Modulo
                         where m.id == id
                         select m.nombre;
            String Nombre = "";
            try
            {
                Nombre = nombre.First();
            }
            catch (Exception)
            {

            }
            return Nombre;
        }
    }
}