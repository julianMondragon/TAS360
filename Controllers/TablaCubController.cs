using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS360.Models.ViewModel;
using System.Web.Mvc;
using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Data;

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
            ViewBag.IsVisbleDlcsv = false;
            ViewBag.IsVisbleDlxlsx = false;
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
            StringBuilder stringbuilder = new StringBuilder();
            string filepath = string.Empty;
            string WarnigMesagge = string.Empty;
            string SucessMesagge = string.Empty;
            double convert_number;
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
                postedFile.SaveAs(filepath);
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
                    tabla.Fondo_Rango2 = Math.Round(TablaCub.GetCellValueAsDouble(row, column),3);
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

                    stringbuilder.AppendLine("Nivel (mm),Volumen (Bls), Volumen (m3)");

                    //Obtiene el fondo y lo escribe en escribe en el nuevo archivo 
                    while (tabla.Fondo_Rango2 != Math.Round(TablaCub.GetCellValueAsDouble(row, column), 3))
                    {
                        //obtiene los valores por cada renglon de la tabla actual
                        //VALOR A - Nivel 
                        double valorA = TablaCub.GetCellValueAsDouble(row, 1);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 1), out convert_number))
                        {
                            valorA = convert_number;
                        }
                        else
                        {
                            string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 1)} as double type, \n Verifica: El contenido del renglon {row} y la columna 1.";
                            throw new Exception(message);
                        }

                        //VALOR B - Barilles 
                        double valorB = Math.Round(TablaCub.GetCellValueAsDouble(row, 2),2);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 2), out convert_number))
                        {
                            valorB = Math.Round(convert_number,2);
                        }
                        else
                        {
                            string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 2)} as double type, \n Verifica: El contenido del renglon {row} y la columna 2.";
                            throw new Exception(message);
                        }

                        //VALOR C - Metros cubicos
                        double valorC = Math.Round(TablaCub.GetCellValueAsDouble(row, 3),3);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 3), out convert_number))
                        {
                            valorC = Math.Round(convert_number, 3);
                        }
                        else
                        {
                            //Console.WriteLine("Unable to parse '{0}'.", value);
                            string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 3)} as double type, \n Verifica: El contenido del renglon {row} y la columna 3.";
                            throw new Exception(message);
                        }

                        //establece los valores en un nuevo reglon de la nueva tabla de Cub
                        NewTablaCub.SetCellValue(newRow, 1, Math.Round(valorA * 1000));
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
                        stringbuilder.AppendLine(Math.Round(valorA * 1000) + "," + valorB + "," + valorC);
                    }

                    //Iteracion de toda la tabla apartir del fondo.
                    while (!string.IsNullOrEmpty(TablaCub.GetCellValueAsString(row, column)))
                    {
                        if (TablaCub.GetCellValueAsString(row, column).Contains("Fin"))
                        {
                            row++;
                            continue;
                        }
                        //obtiene los valores por cada renglon de la tabla actual 
                        double valorA = TablaCub.GetCellValueAsDouble(row, 1);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 1), out convert_number))
                        {
                            valorA = convert_number;
                        }
                        else
                        {
                            if (!TablaCub.GetCellValueAsString(row, 2).Contains("Fin"))
                            {
                                string aux = TablaCub.GetCellValueAsString(row + 1, 2);
                                if (TablaCub.GetCellValueAsString(row + 1, 2) == "")
                                {
                                    string mgs = String.Format("Unable to parse value NULL as double type \n Verifica el contenido del renglon {0} y la columna 2 \n Asegurate que al final de la tabla de cubicacion en cada columna contenga Fin de Tabla", row);
                                    throw new Exception(mgs);
                                }
                                else
                                {
                                    string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 1)} as double type \n Verifica: El contenido del renglon {row} y la columna 1.";
                                    throw new Exception(message);
                                }
                            }                            
                        }

                        double valorB = Math.Round(TablaCub.GetCellValueAsDouble(row, 2),2);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 2), out convert_number))
                        {
                            valorB = Math.Round(convert_number, 2);
                        }
                        else
                        {
                            if (!TablaCub.GetCellValueAsString(row, 2).Contains("Fin"))
                            {
                                string aux = TablaCub.GetCellValueAsString(row + 1, 2);
                                if (TablaCub.GetCellValueAsString(row, 2) == "")
                                {
                                    string mgs = String.Format("Unable to parse value NULL as double type \n Verifica el contenido del renglon {0} y la columna 2 \n Asegurate que al final de la tabla de cubicacion en cada columna contenga Fin de Tabla", row);
                                    throw new Exception(mgs);
                                }
                                else
                                {
                                    string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 2)} as double type \n Verifica: El contenido del renglon {row} y la columna 2";
                                    throw new Exception(message);
                                }
                            }
                            
                        }

                        double valorC = Math.Round(TablaCub.GetCellValueAsDouble(row, 3),3);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row, 3), out convert_number))
                        {
                            valorC = Math.Round(convert_number, 3);
                        }
                        else
                        {
                            if (!TablaCub.GetCellValueAsString(row, 3).Contains("Fin"))
                            {
                                string aux = TablaCub.GetCellValueAsString(row , 3);
                                if (TablaCub.GetCellValueAsString(row, 3) == "")
                                {
                                    string mgs = String.Format("Unable to parse value NULL as double type \n Verifica el contenido del renglon {0} y la columna 2 \n Asegurate que al final de la tabla de cubicacion en cada columna contenga Fin de Tabla", row);
                                    throw new Exception(mgs);
                                }
                                else
                                {
                                    string message = $"Unable to parse {TablaCub.GetCellValueAsString(row, 3)} as double type \n Verifica el contenido del renglon {row} y la columna 3.";
                                    throw new Exception(message);
                                }
                            }
                            
                        }

                        //obtiene el valor del siguiente reglon para su validacion 
                        double valorD = TablaCub.GetCellValueAsDouble(row + 1, 2);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row + 1, 2), out convert_number))
                        {
                            valorD = convert_number;
                        }
                        else
                        {
                            string var = TablaCub.GetCellValueAsString(row + 1, 2);
                            if (!TablaCub.GetCellValueAsString(row +1, 2).Contains("Fin"))
                            {
                                string aux = TablaCub.GetCellValueAsString(row + 1, 2);
                                if (TablaCub.GetCellValueAsString(row + 1, 2) == "")
                                {
                                    string mgs = String.Format("Unable to parse value NULL as double type \n Verifica el contenido del renglon {0} y la columna 2 \n Asegurate que al final de la tabla de cubicacion en cada columna contenga Fin de Tabla", row + 1);
                                    throw new Exception(mgs);
                                }
                                else
                                {
                                    string message = $"Unable to parse {TablaCub.GetCellValueAsString(row + 1, 2)} as double type \n Verifica el contenido del renglon {row + 1} y la columna 2";
                                    throw new Exception(message);
                                }                                
                            }                            
                        }

                        double valorF = TablaCub.GetCellValueAsDouble(row + 1, 3);
                        //Validacion de convercion a double del valor de celda 
                        if (Double.TryParse(TablaCub.GetCellValueAsString(row + 1, 3), out convert_number))
                        {
                            valorF = convert_number;
                        }
                        else
                        {
                            if (!TablaCub.GetCellValueAsString(row +1, 3).Contains("Fin"))
                            {
                                string aux = TablaCub.GetCellValueAsString(row + 1, 3);
                                if (TablaCub.GetCellValueAsString(row + 1, 3) == "")
                                {
                                    string mgs = String.Format("Unable to parse value NULL as double type \n Verifica el contenido del renglon {0} y la columna 3 \n Asegurate que al final de la tabla de cubicacion en cada columna contenga Fin de Tabla", row + 1);
                                    throw new Exception(mgs);
                                }
                                else
                                {
                                    string message = $"Unable to parse {TablaCub.GetCellValueAsString(row + 1, 3)} as double \n Verifica el contenido del renglon {row + 1} y la columna 3";
                                    throw new Exception(message);
                                }
                            }
                        }

                        double valorG = valorB;
                        double valorH = valorC;

                        //Evalua si existe una zona critica definida para que se salte la iteracion por milimetros
                        if (zona_critica_R1_aviable && zona_critica_R2_aviable)
                        {
                            //evalua si el valor de nivel esta dentro de los rangos de la zona critica
                            if (valorA >= ZCR1 && valorA < ZCR2)
                            {
                                //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                NewTablaCub.SetCellValue(newRow, 1, Math.Round(valorA * 1000));
                                NewTablaCub.SetCellValue(newRow, 2, valorB);
                                NewTablaCub.SetCellValue(newRow, 3, valorC);

                                //agrega los registros a la tabla que se va mostrar en la interfaz web 
                                tabla.Tabla.Add(new Tabla
                                {
                                    nivel = valorA,
                                    bls = valorB,
                                    volumen_m3 = valorC
                                });

                                stringbuilder.AppendLine(Math.Round(valorA * 1000) + "," + valorB + "," + valorC);

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
                                        int indexRow = 3;
                                        switch (i) 
                                        {                                     
                                          case 2:
                                                indexRow = 4;
                                                break;
                                          case 3:
                                                indexRow = 5;
                                                break;
                                          case 4:
                                                indexRow = 6;
                                                break;
                                          case 5:
                                                indexRow = 7;
                                                break;
                                          case 6:
                                                indexRow = 8;
                                                break;
                                          case 7:
                                                indexRow = 9;
                                                break;
                                          case 8:
                                                indexRow = 10;
                                                break;
                                          case 9:
                                                indexRow = 11;
                                                break;
                                        }
                                        valorA += 0.001;
                                        double x = Math.Round(TablaCub.GetCellValueAsDouble(indexRow, 7),3);
                                        //Validacion de convercion a double del valor de celda 
                                        if (Double.TryParse(TablaCub.GetCellValueAsString(indexRow, 7), out convert_number))
                                        {
                                            x = Math.Round(convert_number,3);
                                        }
                                        else
                                        {
                                            string message = $"Unable to parse {TablaCub.GetCellValueAsString(indexRow, 7)} as double type \n Verifica el contenido del renglon {row} y la columna 7";
                                            throw new Exception(message);
                                        }
                                        valorC = valorH + x;
                                        double y = Math.Round(TablaCub.GetCellValueAsDouble(indexRow, 6),2);
                                        //Validacion de convercion a double del valor de celda 
                                        if (Double.TryParse(TablaCub.GetCellValueAsString(indexRow, 6), out convert_number))
                                        {
                                            y = Math.Round(convert_number, 2);
                                        }
                                        else
                                        {
                                            string message = $"Unable to parse {TablaCub.GetCellValueAsString(indexRow, 6)} as double type \n Verifica el contenido del renglon {row} y la columna 6";
                                            throw new Exception(message);
                                        }
                                        valorB = valorG + y;
                                    }

                                    //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                    NewTablaCub.SetCellValue(newRow, 1, Math.Round(valorA * 1000));

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
                                    stringbuilder.AppendLine(Math.Round(valorA * 1000) + "," + valorB + "," + valorC);
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
                                    int indexRow = 3;
                                    switch (i)
                                    {
                                        case 2:
                                            indexRow = 4;
                                            break;
                                        case 3:
                                            indexRow = 5;
                                            break;
                                        case 4:
                                            indexRow = 6;
                                            break;
                                        case 5:
                                            indexRow = 7;
                                            break;
                                        case 6:
                                            indexRow = 8;
                                            break;
                                        case 7:
                                            indexRow = 9;
                                            break;
                                        case 8:
                                            indexRow = 10;
                                            break;
                                        case 9:
                                            indexRow = 11;
                                            break;
                                    }
                                    valorA += 0.001;
                                    double x = Math.Round(TablaCub.GetCellValueAsDouble(indexRow, 7), 3);
                                    //Validacion de convercion a double del valor de celda 
                                    if (Double.TryParse(TablaCub.GetCellValueAsString(indexRow, 7), out convert_number))
                                    {
                                        x = Math.Round(convert_number, 3);
                                    }
                                    else
                                    {
                                        string message = $"Unable to parse {TablaCub.GetCellValueAsString(indexRow, 7)} as double type \n Verifica el contenido del renglon {indexRow} y la columna 7";
                                        throw new Exception(message);
                                    }
                                    valorC = valorH + x;
                                    double y = Math.Round(TablaCub.GetCellValueAsDouble(indexRow, 6), 2);
                                    //Validacion de convercion a double del valor de celda 
                                    if (Double.TryParse(TablaCub.GetCellValueAsString(indexRow, 6), out convert_number))
                                    {
                                        y = Math.Round(convert_number, 2);
                                    }
                                    else
                                    {
                                        string message = $"Unable to parse {TablaCub.GetCellValueAsString(indexRow, 6)} as double type \n Verifica el contenido del renglon {indexRow} y la columna 6";
                                        throw new Exception(message);
                                    }
                                    valorB = valorG + y;
                                }

                                //establece los valores en un nuevo reglon de la nueva tabla de Cub
                                NewTablaCub.SetCellValue(newRow, 1, Math.Round(valorA * 1000));

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
                                stringbuilder.AppendLine(Math.Round(valorA * 1000) + "," + valorB + "," + valorC);
                            }
                            row++;
                        }

                    }

                    //Se crea e inserta un objeto<SLTabla> en el nuevo archivo, posteriormente se guarda el archivo
                    SLTable lTable = NewTablaCub.CreateTable("A1", String.Format("C{0}", newRow--));
                    NewTablaCub.InsertTable(lTable);
                    string[] name = postedFile.FileName.Split('.');

                    #endregion

                    string mesage = "Se genero correctamente la tabla de cubicacion";
                    if (contWarningm3 > 0 || contWarningBls > 0)
                    {
                        NewTablaCub.SaveAs(path + name[0] + "_mm_x_mm_"+ tabla.TAD +".xlsx");
                        mesage = "Se genero correctamente la tabla de cubicacion, \n pero se encontraron iregularidades de volumen en m3 (" +  contWarningm3 + " puntos) y en Bls (" + contWarningBls + " puntos) \n en la tabla milimetrica generada (marcados en rojo) favor de realizar \n los ajustes de manera manual antes de cargar la tabla al TAS360";
                        ViewBag.Warning = mesage;
                        System.IO.File.Delete(filepath);
                        ViewBag.IsVisbleDlxlsx = true;
                        ViewBag.IsVisbleDlcsv = false;
                        tabla.fileName = name[0] + "_mm_x_mm_" + tabla.TAD + ".xlsx";
                    }
                    else
                    {
                        //Codigo Anterior 
                        //NewTablaCub.SaveAs(path + name[0] + "_mm_x_mm.csv");
                        //ViewBag.sucess = mesage;

                        //Codigo nuevo
                        System.IO.File.WriteAllText(path + name[0] + "_mm_x_mm_" + tabla.TAD + ".csv", stringbuilder.ToString());
                        ViewBag.sucess = mesage;
                        System.IO.File.Delete(filepath);
                        ViewBag.IsVisbleDlcsv = true;
                        ViewBag.IsVisbleDlxlsx = false;
                        tabla.fileName = name[0] + "_mm_x_mm_" + tabla.TAD + ".csv";

                    }

                   
                    
                    return View(tabla);
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("El proceso no puede obtener acceso al archivo"))
                    {
                        ViewBag.Exception = "No puedes importar un archivo mientras este abierto, favor de cerrar el archivo";
                    }
                    else if(ex.Message.Contains("Unable to parse"))
                    {
                        ViewBag.IsVisbleDlxlsx = false;
                        ViewBag.IsVisbleDlcsv = false;
                        string msg = $" {ex.Message} \n Asegurate de que el formato de las celdas en el documento sea de tipo number y no contengan espacios entre los valores. ";
                        ViewBag.Exception = msg;
                    }
                    else
                    {
                        ViewBag.Exception = String.Format("En el renglon {0} y columna {1} ocurrio el siguiente error: \n {2}", row, column, ex.Message);
                    }
                    
                }
                finally
                {
                    stringbuilder = null;
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
        /// <summary>
        /// Metodo que se encarga de descargar el archivo en excel que fue convertido
        /// </summary>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        public FileResult DownloadFileExcel(string NameFile)
        {
            string rute = Server.MapPath("~/Tablas_CSV/" + NameFile);
            return File(rute, "application/xlsx", NameFile);
        }
        /// <summary>
        /// Metodo que se encarga de descargar el archivo en CSV que fue convertido
        /// </summary>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        public FileResult DownloadFileCSV(string NameFile)
        {
            string rute = Server.MapPath("~/Tablas_CSV/" + NameFile);
            return File(rute, "application/text", NameFile);
        }
    }
}