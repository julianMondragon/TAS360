using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class Roles_and_PrivilegeController : Controller
    {
        /// <summary>
        ///  Metodo principal que muestra los modulos, roles y privilegios 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RolesPrivilegesViewModel rolesPrivileges = new RolesPrivilegesViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var modules = from m in db.Modulo select m;
                if (modules.Any())
                {
                    foreach (var item in modules)
                    {
                        rolesPrivileges.Modules.Add(item);
                    }
                }
                var roles = from r in db.Roll select r;
                if (roles.Any())
                {
                    foreach (var item in roles)
                    {
                        rolesPrivileges.Rolls.Add(item);
                    }
                }
                var operacion = from r in db.Operacion select r;
                if (roles.Any())
                {
                    foreach (var item in operacion)
                    {
                        rolesPrivileges.Operacions.Add(item);
                    }
                }
                var rol_operacionA = from R in db.Roll_Operacion where R.id_Roll == 1 select R;
                if (rol_operacionA.Any())
                {
                    foreach( var item in rol_operacionA)
                    {
                        rolesPrivileges.Rol_OperacionAdmin.Add(item);
                    }
                }
                var rol_operacionR = from R in db.Roll_Operacion where R.id_Roll == 2 select R;
                if (rol_operacionR.Any())
                {
                    foreach (var item in rol_operacionR)
                    {
                        rolesPrivileges.Rol_OperacionResp.Add(item);
                    }
                }
                var rol_operacionC = from R in db.Roll_Operacion where R.id_Roll == 3 select R;
                if (rol_operacionC.Any())
                {
                    foreach (var item in rol_operacionC)
                    {
                        rolesPrivileges.Rol_OperacionContac.Add(item);
                    }
                }
                var rol_operacionV = from R in db.Roll_Operacion where R.id_Roll == 4 select R;
                if (rol_operacionV.Any())
                {
                    foreach (var item in rol_operacionV)
                    {
                        rolesPrivileges.Rol_OperacionVist.Add(item);
                    }
                }

            }
            GetUsuarios();
            return View(rolesPrivileges);
        }
        /// <summary>
        /// Devuelve a la vista una lista de los usuarios especialistas tecnicos 
        /// </summary>
        private void GetUsuarios()
        {

            List<SelectListItem> Usuarios = new List<SelectListItem>();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var aux = (from s in db.User select s);
                if (aux != null && aux.Any())
                {
                    foreach (var a in aux)
                    {
                        Usuarios.Add(new SelectListItem
                        {
                            Text = a.nombre,
                            Value = a.id.ToString()

                        });
                    }
                }
            }
            ViewBag.Usuarios = Usuarios;

        }
    }
}
