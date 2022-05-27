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
                    SLDocument NewTablaCub = new SLDocument();

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
                    //Establece el encabezado del nuevo archivo 
                    NewTablaCub.SetCellValue(row, 7, "Nivel (m)");
                    NewTablaCub.SetCellValue(row, 8, "Volumen (Bls)");
                    NewTablaCub.SetCellValue(row, 9, "Volumen (m3)");
                    row++;

                    //Obtiene el fondo y lo escribe en escribe en el nuevo archivo 
                    while (tabla.Fondo_Rango2 != TablaCub.GetCellValueAsDouble(row, column))
                    {
                        double valorA = TablaCub.GetCellValueAsDouble(row,1);
                        double valorB = TablaCub.GetCellValueAsDouble(row,2);
                        double valorC = TablaCub.GetCellValueAsDouble(row,3);

                        NewTablaCub.SetCellValue(row, 7, valorA);
                        NewTablaCub.SetCellValue(row, 8, valorB);
                        NewTablaCub.SetCellValue(row, 9, valorC);

                        tabla.Tabla.Add(new Tabla
                        {
                            nivel = valorA,
                            bls = valorB,
                            volumen_m3 = valorC
                        }); 
                        row++;
                    }

                    //TODO (por hacer)
                    //seguir iterando la tabla y aplicar la formula realizar
                    //la cubicacion mm x mm evitando la zona critica de toda la tabla de cubicacion
                    int newRow = row;
                    while(!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, column)))
                    {
                        double valorA = TablaCub.GetCellValueAsDouble(row, 1);
                        double valorB = TablaCub.GetCellValueAsDouble(row, 2);
                        double valorC = TablaCub.GetCellValueAsDouble(row, 3);
                        for (int i = 0; i < 9; i++)
                        { 
                            if(i != 0)
                                valorA += 0.001;
                            NewTablaCub.SetCellValue(newRow, 7, valorA);
                            //NewTablaCub.SetCellValue(newRow, 8, valorB);
                            if (i != 0)
                            {
                                double x = TablaCub.GetCellValueAsDouble(3, 7);
                                valorC = valorC + x;
                            }
                            NewTablaCub.SetCellValue(newRow, 9, valorC);
                            newRow ++;
                            tabla.Tabla.Add(new Tabla
                            {
                                nivel = valorA,
                                bls = valorB,
                                volumen_m3 = valorC
                            });
                        }                        
                        row++;
                    }

                    //Se crea e inserta un objeto<SLTabla> en el nuevo archivo, posteriormente se guarda el archivo
                    SLTable lTable = NewTablaCub.CreateTable("G12", String.Format("I{0}", newRow--));
                    NewTablaCub.InsertTable(lTable);
                    NewTablaCub.SaveAs(path + "TablaCub_mm_x_mm.xlsx");
                    
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