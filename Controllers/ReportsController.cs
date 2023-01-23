using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class ReportsController : Controller
    {
        public static ConnexionSQLViewModel connexionSQLViewModel = null;
        // GET: Reports
        public ActionResult Index()
        {
            if(connexionSQLViewModel == null)
            {
                connexionSQLViewModel = new ConnexionSQLViewModel();
                return View(connexionSQLViewModel);
            }
            else
            {
                //TODO añadir parametro.
                return Redirect("OpenConexion");
            }
        }
        /// <summary>
        /// Abre la conexion con la base de datos 
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        [HttpPost]
       public ActionResult OpenConexion(ConnexionSQLViewModel conn)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = conn.server;
                builder.InitialCatalog = conn.namedatabase;
                builder.UserID = conn.user;
                builder.Password = conn.password;
                builder.ApplicationName = "MyApp";
              
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    ViewBag.Open = "Se abrio la conexion con la base de datos:" + builder.InitialCatalog ;
                    // Do work here.  
                    SqlCommand command = new SqlCommand("SELECT @@Version AS Version;", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ViewBag.Info = reader.GetString(0);
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
                connexionSQLViewModel  = new ConnexionSQLViewModel();
                connexionSQLViewModel.server = conn.server;
                connexionSQLViewModel.user = conn.user;
                connexionSQLViewModel.password = conn.password;
                connexionSQLViewModel.namedatabase = conn.namedatabase;
            }
            catch (Exception ex)
            {
                ViewBag.Warning = ex.Message;
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
       public ActionResult CreateRequest()
        {
            CreateReportViewModel createReport = new CreateReportViewModel();
            try
            {              
                createReport = SetCatalogos(createReport);

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                List<SelectListItem> tables = new List<SelectListItem>();
                builder.DataSource = connexionSQLViewModel.server;
                builder.InitialCatalog = connexionSQLViewModel.namedatabase;
                builder.UserID = connexionSQLViewModel.user;
                builder.Password = connexionSQLViewModel.password;
                builder.ApplicationName = "MyApp";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    string query = $"use {connexionSQLViewModel.namedatabase} SELECT TABLE_NAME FROM information_schema.tables WHERE table_type = 'BASE TABLE';";
                    // Do work here.  
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int i = 0;
                        
                        while (reader.Read())
                        {
                            tables.Add(new SelectListItem
                            {
                                Text = reader.GetString(0),
                                Value = reader.GetString(0)
                            });
                            //reader.NextResult();
                            i++;
                        }
                        
                    }
                    reader.Close();
                    connection.Close();
                }
                createReport.Table = tables;
            }
            catch(Exception ex)
            {

            }
            return View(createReport);

        }

        private CreateReportViewModel SetCatalogos(CreateReportViewModel createReport)
        {

            List<SelectListItem> ListTypeQuery = new List<SelectListItem>();
            ListTypeQuery.Add(new SelectListItem
            {
                Text = "Selecciona",
                Value = "SELECT",
                Selected = true
            });

            List<SelectListItem> ListWhatRequestQuery = new List<SelectListItem>();
            ListWhatRequestQuery.Add(new SelectListItem
            {
                Text = "Todo",
                Value = " * ",
                Selected = true
            });
            ListWhatRequestQuery.Add(new SelectListItem
            {
                Text = "10",
                Value = " TOP(10)",
                Disabled = true

            });
            ListWhatRequestQuery.Add(new SelectListItem
            {
                Text = "100",
                Value = " TOP(100)",
                Disabled = true

            });

            List<SelectListItem> ListConditionQuery = new List<SelectListItem>();
            ListConditionQuery.Add(new SelectListItem
            {
                Text = "Donde",
                Value =" Where ",
                Disabled=true
            });

            createReport.TypeQuery = ListTypeQuery;
            createReport.whatRequest = ListWhatRequestQuery;
            createReport.condition = ListConditionQuery;

            return createReport;

        }
    }
}
