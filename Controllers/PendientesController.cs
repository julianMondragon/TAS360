using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class PendientesController : Controller
    {
        // GET: Pendientes
        public ActionResult Index()
        {
            PendientesViewModel model = new PendientesViewModel();
            return View(model);
        }

        // GET: Pendientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pendientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pendientes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pendientes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pendientes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pendientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pendientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
