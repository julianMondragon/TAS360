using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TAS360.Models;
using TAS360.Models.ViewModel;  // Asegúrate de que el espacio de nombres sea correcto

namespace TAS360.Controllers
{
    //   [Authorize]  // El usuario debe estar autenticado para acceder a estas acciones
    public class ProfileController : Controller
    {

        // Acción para la búsqueda del perfil
        //public ActionResult BuscarPerfil(string criterio)
        //{
        //    if (string.IsNullOrEmpty(criterio))
        //    {
        //        return View(new List<PerfilusrViewModel>());
        //    }

        //    // Buscar perfiles que coincidan con el criterio
        //    var resultados = _context.usr_profile
        //        .Where(p => p.nombre.Contains(criterio) || p.email.Contains(criterio))
        //        .Select(p => new PerfilusrViewModel
        //        {
        //            id_User = p.id_User,
        //            nombre = p.nombre,
        //            email = p.email,
        //            Cel = p.Cel,
        //            Género = p.Género,
        //            Estado = p.Estado,
        //            Foto_usuario = p.Foto_usuario,
        //            fecha_nacimeiento = (DateTime)p.fecha_nacimeinto,
                    
        //        }).ToList();

        //    return View(resultados);
        //}




        // GET: Profile/Index
        public ActionResult Index(int id)
        {
            PerfilusrViewModel perfil = new PerfilusrViewModel();
            using (HelpDesk_Entities1 db = new HelpDesk_Entities1())
            {
                var T = db.usr_profile.Find(id);
                
                perfil.nombre = T.nombre;
                perfil.email = T.email;
                perfil.Cel = T.Cel;
                //Asignar el usuario.
                // ticket.id_Usuario = db.Ticket_User.Where(a => a.id_Ticket == id).OrderByDescending(a => a.CreatedAt).FirstOrDefault().id_User;
            }



            return View(perfil);

        }

        //// GET: Profile/Edit
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        //    }

        //    var profile = _context.usr_profile.Find(id);

        //    if (profile == null)
        //    {
        //        return HttpNotFound("Perfil no encontrado");
        //    }

        //    return View(profile);
        //}

    //    // POST: Profile/Edit
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Edit(usr_profile profile)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var existingProfile = _context.usr_profile.Find(profile.id_User);

    //            if (existingProfile == null)
    //            {
    //                return HttpNotFound("Perfil no encontrado");
    //            }

    //            // Actualizar los campos del perfil
    //            existingProfile.nombre = profile.nombre;
    //            existingProfile.email = profile.email;
    //            existingProfile.Cel = profile.Cel;
    //            existingProfile.Género = profile.Género;
    //            existingProfile.Estado = profile.Estado;
    //            existingProfile.Foto_usuario = profile.Foto_usuario;
    //            existingProfile.updateAt = DateTime.Now;

    //            await _context.SaveChangesAsync();

    //            return RedirectToAction("Index");
    //        }

    //        return View(profile);
    //    }

        
    }

}
