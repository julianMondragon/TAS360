using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS360.Models.ViewModel;
using System.Web.Mvc;
using System.IO;
using SpreadsheetLight;

namespace TAS360.Controllers
{
    public class TablaCubController : Controller
    {
        // GET: TablaCub
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            TablaCubViewModel tabla = new TablaCubViewModel();
            string filepath = string.Empty;
            if(postedFile != null)
            {
                string path = Server.MapPath("~/TEST_CSV/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                filepath = path + Path.GetFileName(postedFile.FileName);
                try
                {   
                    SLDocument TablaCub  = new SLDocument(filepath);
                    int row = 2;
                    while (!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, 1)))
                    {
                        if(row == 2)
                            tabla.TAD = TablaCub.GetCellValueAsString(row, 2);
                        if (row == 3)
                            tabla.Tag = TablaCub.GetCellValueAsString(row, 2);
                        if (row == 4)
                            tabla.Capacidad = TablaCub.GetCellValueAsString(row, 2);
                        if (row == 5)
                            tabla.Altura = TablaCub.GetCellValueAsDouble(row, 2);
                        if (row == 6)
                            tabla.Producto = TablaCub.GetCellValueAsString(row, 2);
                        row++;
                    }                    
                    return View(tabla);
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                }

            }
            return View();
        }
    }
}