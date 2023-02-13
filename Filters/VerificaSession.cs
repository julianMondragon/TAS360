using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Controllers;

namespace TAS360.Filters
{
    public class VerificaSession : System.Web.Mvc.ActionFilterAttribute
    {
        private Models.User oUsuario;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            oUsuario = (Models.User)HttpContext.Current.Session["user"];
            if(oUsuario == null)
            {
               if(filterContext.Controller is AccesoController == false)
                {
                    filterContext.HttpContext.Response.Redirect("~/Acceso/login");
                }
            }
        }
    }
}