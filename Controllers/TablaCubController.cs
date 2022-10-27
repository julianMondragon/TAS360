using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS360.Models.ViewModel;
using System.Web.Mvc;
using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

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
            string WarnigMesagge = string.Empty;
            string SucessMesagge = string.Empty;
            int contWarningm3 = 0;
            int contWarningBls = 0;
            int row = 2;
            int column = 2;
            int newRow = 1;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Tablas_CSV/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                filepath = path + Path.GetFileName(postedFile.FileName);
                try
                {
                    SLDocument TablaCub = new SLDocument(filepath);
                    SLDocument NewTablaCub = new SLDocument();
                    SLStyle style1 = NewTablaCub.CreateStyle();
                    style1.Fill.SetPattern(PatternValues.Solid, SLThemeColorIndexValues.Accent2Color, SLThemeColorIndexValues.Accent4Color);

                    #region Obtiene la informacion primaria de la tabla de cubicacion
                    while (!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, column)))
                    {
                        if (row == 2)
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
                    tabla.Fondo_Rango2 = TablaCub.GetCellValueAsDouble(row, column);
                    row++;
                    column--;
                    tabla.ZonaCritica_Rango1 = TablaCub.GetCellValueAsDouble(row, column);
                    column++;
                    tabla.ZonaCritica_Rango2 = (TablaCub.GetCellValueAsDouble(row, column));
                    tabla.VolumenXmil = TablaCub.GetCellValueAsDouble(3, 7);

                    var zona_critica_R1_aviable = double.TryParse(tabla.ZonaCritica_Rango1.ToString(), out double ZCR1);
                    var zona_critica_R2_aviable = double.TryParse(tabla.ZonaCritica_Rango2.ToString(), out double ZCR2);
                    if(ZCR1 == 0 && ZCR2 == 0)
                    {
                        zona_critica_R1_aviable = false; 
                        zona_critica_R2_aviable = false;
                    }

                    #endregion

                    #region Iteracion de la tabla de cubicacion 

                    row = 12;
                    column = 1;
                    //Establece el encabezado del nuevo archivo 
                    NewTablaCub.SetCellValue(newRow, 1, "Nivel (mm)");
                    NewTablaCub.SetCellValue(newRow, 2, "Volumen (Bls)");
                    NewTablaCub.SetCellValue(newRow, 3, "Volumen (m3)");
                    row++;
                    newRow++;

                    //Obtiene el fondo y lo escribe en escribe en el nuevo archivo 
                    while (tabla.Fondo_Rango2 != TablaCub.GetCellValueAsDouble(row, column))
                    {
                        //obtiene los valores por cada renglon de la tabla actual
                        double valorA = TablaCub.GetCellValueAsDouble(row, 1);
                        double valorB = TablaCub.GetCellValueAsDouble(row, 2);
                        double valorC = TablaCub.GetCellValueAsDouble(row, 3);

                        //establece los valores en un nuevo reglon de la nueva tabla de Cub
                        NewTablaCub.SetCellValue(newRow, 1, valorA * 1000);
                        NewTablaCub.SetCellValue(newRow, 2, valorB);
                        NewTablaCub.SetCellValue(newRow, 3, valorC);

                        //agrega los registros a la tabla que se va mostrar en la interfaz web 
                        tabla.Tabla.Add(new Tabla
                        {
                            nivel = valorA,
                            bls = valorB,
                            volumen_m3 = valorC
                        });
                        row++;
                        newRow++;
                    }

                    //Iteracion de toda la tabla apartir del fondo.
                    while (!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, column)))
                    {
                        //obtiene los valores por cada renglon de la tabla actual 
                        double valorA = TablaCub.GetCellValueAsDouble(row, 1);
                        double valorB = TablaCub.GetCellValueAsDouble(row, 2);
                        double valorC = TablaCub.GetCellValueAsDouble(row, 3);
                        //obtiene el valor del siguiente reglon para su validacion 
                        double valorD = TablaCub.GetCellValueAsDouble(row + 1, 2);
                        double valorF = TablaCub.GetCellValueAsDouble(row + 1, 3);

                        //Evalua si existe una zona critica definida para que se salte la iteracion por milimetros
                        if (zona_critica_R1_aviable && zona_critica_R2_aviable)
                        {
                            //evalua si el valor de nivel esta dentro de los rangos de la zona critica
                            if (valorA >= ZCR1 && valorA < ZCR2)
                            {
                                //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                NewTablaCub.SetCellValue(newRow, 1, valorA * 1000);
                                NewTablaCub.SetCellValue(newRow, 2, valorB);
                                NewTablaCub.SetCellValue(newRow, 3, valorC);

                                //agrega los registros a la tabla que se va mostrar en la interfaz web 
                                tabla.Tabla.Add(new Tabla
                                {
                                    nivel = valorA,
                                    bls = valorB,
                                    volumen_m3 = valorC
                                });
                                row++;
                                newRow++;
                            }
                            else
                            {
                                //agrega los 10 registros de cada milimetro.
                                for (int i = 0; i < 10; i++)
                                {
                                    if (i != 0)
                                    {
                                        valorA += 0.001;
                                        double x = TablaCub.GetCellValueAsDouble(3, 7);
                                        valorC = valorC + x;
                                        double y = TablaCub.GetCellValueAsDouble(3, 6);
                                        valorB = valorB + y;
                                    }

                                    //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                    NewTablaCub.SetCellValue(newRow, 1, valorA * 1000);

                                    //valida que el valor de volumen ingresado en BLS en mm no sea mayor al siguiente valor en cm  
                                    if (valorB > valorD && valorD != 0)
                                    {
                                        NewTablaCub.SetCellValue(newRow, 2, valorB);
                                        NewTablaCub.SetCellStyle(newRow, 2, style1);
                                        contWarningm3++;
                                    }
                                    else
                                    {
                                        NewTablaCub.SetCellValue(newRow, 2, valorB);
                                    }
                                    //valida que el valor de volumen ingresado en m3 en mm no sea mayor al siguiente valor en cm   
                                    if (valorC > valorF && valorF != 0)
                                    {
                                        NewTablaCub.SetCellValue(newRow, 3, valorC);
                                        NewTablaCub.SetCellStyle(newRow, 3, style1);
                                        contWarningBls++;
                                    }
                                    else
                                    {
                                        NewTablaCub.SetCellValue(newRow, 3, valorC);
                                    }
                                    //se incrementa un nuevo reglon     
                                    newRow++;

                                    //agrega los registros a la tabla que se va mostrar en la interfaz web 
                                    tabla.Tabla.Add(new Tabla
                                    {
                                        nivel = valorA,
                                        bls = valorB,
                                        volumen_m3 = valorC
                                    });
                                }
                                row++;
                            }

                        }
                        else
                        {
                            //agrega los 10 registros de cada milimetro.
                            for (int i = 0; i < 10; i++)
                            {
                                if (i != 0)
                                {
                                    valorA += 0.001;
                                    double x = TablaCub.GetCellValueAsDouble(3, 7);
                                    valorC = valorC + x;
                                    double y = TablaCub.GetCellValueAsDouble(3, 6);
                                    valorB = valorB + y;
                                }

                                //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                NewTablaCub.SetCellValue(newRow, 1, valorA * 1000);

                                //valida que el valor de volumen ingresado en BLS en mm no sea mayor al siguiente valor en cm  
                                if (valorB > valorD && valorD != 0)
                                {
                                    NewTablaCub.SetCellValue(newRow, 2, valorB);
                                    NewTablaCub.SetCellStyle(newRow, 2, style1);
                                    contWarningm3++;
                                }
                                else
                                {
                                    NewTablaCub.SetCellValue(newRow, 2, valorB);
                                }
                                //valida que el valor de volumen ingresado en m3 en mm no sea mayor al siguiente valor en cm   
                                if (valorC > valorF && valorF != 0)
                                {
                                    NewTablaCub.SetCellValue(newRow, 3, valorC);
                                    NewTablaCub.SetCellStyle(newRow, 3, style1);
                                    contWarningBls++;
                                }
                                else
                                {
                                    NewTablaCub.SetCellValue(newRow, 3, valorC);
                                }
                                //se incrementa un nuevo reglon     
                                newRow++;

                                //agrega los registros a la tabla que se va mostrar en la interfaz web 
                                tabla.Tabla.Add(new Tabla
                                {
                                    nivel = valorA,
                                    bls = valorB,
                                    volumen_m3 = valorC
                                });
                            }
                            row++;
                        }

                    }

                    //Se crea e inserta un objeto<SLTabla> en el nuevo archivo, posteriormente se guarda el archivo
                    SLTable lTable = NewTablaCub.CreateTable("A1", String.Format("C{0}", newRow--));
                    NewTablaCub.InsertTable(lTable);
                    string[] name = postedFile.FileName.Split('.');

                    string mesage = "Se genero correctamente la tabla de cubicacion";
                    if (contWarningm3 > 0 || contWarningBls > 0)
                    {
                        NewTablaCub.SaveAs(path + name[0] + "_mm_x_mm.xlsx");
                        mesage = "Se genero correctamente la tabla de cubicacion, pero se encontraron iregularidades de volumen en m3 (" +  contWarningm3 + " puntos) y en Bls (" + contWarningBls + " puntos) en la tabla milimetrica generada (marcados en rojo) favor de realizar los ajustes de manera manual antes de cargar la tabla al TAS360";
                        ViewBag.Warning = mesage;
                    }
                    else
                    {
                        NewTablaCub.SaveAs(path + name[0] + "_mm_x_mm.csv");
                        ViewBag.sucess = mesage;
                    }

                    #endregion 

                    return View(tabla);
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("El proceso no puede obtener acceso al archivo"))
                    {
                        ViewBag.Exception = "No puedes importar un archivo mientras este abierto, favor de cerrar el archivo";
                    }
                    else
                    {
                        ViewBag.Exception = String.Format("En el renglon {0} y columna {1} ocurrio el siguiente error: {2}", row, column, ex.Message);
                    }
                    
                }

            }
            return View();
        }

        public ActionResult DownloadPrototypeInfo()
        {

            return View();
        }

        /// <summary>
        /// Descarga el archivo prototipo para llenar la tabla de Cubicacion
        /// </summary>
        /// <returns></returns>
        public FileResult DownloadPrototype()
        {
            string rute = Server.MapPath("~/Prototipo_Tabla/Prototipo2.xlsx");
            return File(rute, "application/xlsx", "TV-0x.xlsx");
        }
    }
}