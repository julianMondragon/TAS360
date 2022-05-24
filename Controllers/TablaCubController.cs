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
        /// <summary>
        /// Metodo de tipo GET que se ejecuta al inicio 
        /// </summary>
        /// <returns>La vista principal</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Metodo de tipo Post que se ejecuta al oprimir el boton de Examinar dentro de la vista principa
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>La vista principal con la informacion </returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            TablaCubViewModel tabla = new TablaCubViewModel();
            string filepath = string.Empty;
            int row = 2;
            int column = 2;
            if (postedFile != null)
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

                    #region Obtiene la informacion primaria de la tabla de cubicacion
                    while (!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, column)))
                    {
                        if(row == 2)
                            tabla.TAD = TablaCub.GetCellValueAsString(row, column);
                        if (row == 3)
                            tabla.Tag = TablaCub.GetCellValueAsString(row, column);
                        if (row == 4)
                            tabla.Capacidad = TablaCub.GetCellValueAsString(row, column);
                        if (row == 5)
                            tabla.Altura = TablaCub.GetCellValueAsDouble(row, column);
                        if (row == 6)
                            tabla.Producto = TablaCub.GetCellValueAsString(row, column);
                        row++;
                    }

                    row = 9;
                    column = 3;
                    tabla.Fondo_Rango1 = 0;
                    tabla.Fondo_Rango2 = TablaCub.GetCellValueAsDouble(row,column);
                    row++;
                    column--;
                    tabla.ZonaCritica_Rango1 = TablaCub.GetCellValueAsDouble(row,column);
                    column++;
                    tabla.ZonaCritica_Rango2 =(TablaCub.GetCellValueAsDouble(row,column));
                    tabla.VolumenXmil = TablaCub.GetCellValueAsDouble(3,7);
                    #endregion

                    #region Iteracion de la tabla 

                    row = 12;
                    column = 1;

                    TablaCub.SetCellValue(row, 7, "Nivel (m)");
                    TablaCub.SetCellValue(row, 8, "Volumen (Bls)");
                    TablaCub.SetCellValue(row, 9, "Volumen (m3)");
                    row++;


                    //TablaCub.AddWorksheet("DanceFloor");
                    //TablaCub.SelectWorksheet("Sheet3");
                    
                    while (tabla.Fondo_Rango2 != TablaCub.GetCellValueAsDouble(row, column))
                    {
                        double valorA = TablaCub.GetCellValueAsDouble(row,1);
                        double valorB = TablaCub.GetCellValueAsDouble(row,2);
                        double valorC = TablaCub.GetCellValueAsDouble(row,3);

                        //TablaCub.SetCellValue(row, 7, valorA);
                        //TablaCub.SetCellValue(row, 8, valorB);
                        //TablaCub.SetCellValue(row, 9, valorC);

                        tabla.Tabla.Add(new Tabla
                        {
                            nivel = valorA,
                            bls = valorB,
                            volumen_m3 = valorC
                        }); 
                        row++;
                    }


                    TablaCub.SetCellValue(12, 9, "Esto es una prueba");
                    TablaCub.Save();
                    //TablaCub.SaveAs("/TEST_CSV/Prueba_1.xlsx");
                    #endregion 
                    return View(tabla);
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = String.Format("En el renglon {0} y columna {1} ocurrio el siguiente error: {2}",row, column, ex.Message);
                }

            }
            return View();
        }
    }
}