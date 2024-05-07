using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAS360.Models.ViewModel;
using System.Data.SqlClient;
using System.Data;

namespace TAS360.StorProc
{
    public class LSA_Reportes
    {
        public List<G_TicketsByStatusViewModel> GetTicktsByStatus()
        {
           
            List<G_TicketsByStatusViewModel> objLista = new List<G_TicketsByStatusViewModel>();

            
            using (SqlConnection oconexion = new SqlConnection("data source=65.99.205.97;initial catalog=ptstools_HelpDesk;persist security info=True;user id=ptstools_Jmondragon;password=x4fr73E*0;MultipleActiveResultSets=True"))
            {
                string query = "SP_GetTicketsByStatus";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            objLista.Add(new G_TicketsByStatusViewModel()
                            {
                                Estado = dr["Estado"].ToString(),
                                Mas_Antiguo = DateTime.Parse(dr["Mas_Antiguo"].ToString()),
                                Mas_Reciente = DateTime.Parse(dr["Mas_Reciente"].ToString()),
                                Cantidad = int.Parse(dr["Cantidad_Tickets"].ToString())
                            });
                        }
                    }
                    catch(Exception ex)
                    {
                    }                    
                }

                oconexion.Close();
            }

            return objLista;
        }
        public List<G_TicketsByCategoriaViewModel> GetTicketsByCategoria()
        {

            List<G_TicketsByCategoriaViewModel> objLista = new List<G_TicketsByCategoriaViewModel>();


            using (SqlConnection oconexion = new SqlConnection("data source=65.99.205.97;initial catalog=ptstools_HelpDesk;persist security info=True;user id=ptstools_Jmondragon;password=x4fr73E*0;MultipleActiveResultSets=True"))
            {
                string query = "SP_GetTicketsByCategoria";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            objLista.Add(new G_TicketsByCategoriaViewModel()
                            {
                                Categoria = dr["Categoria"].ToString(),
                                Mas_Antiguo = DateTime.Parse(dr["Mas_Antiguo"].ToString()),
                                Mas_Reciente = DateTime.Parse(dr["Mas_Reciente"].ToString()),
                                Cantidad = int.Parse(dr["Cantidad_Tickets"].ToString())
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                oconexion.Close();
            }

            return objLista;
        }
        public List<G_TicketsByTerminalViewModel> GetTicketsByTerminal()
        {

            List<G_TicketsByTerminalViewModel> objLista = new List<G_TicketsByTerminalViewModel>();


            using (SqlConnection oconexion = new SqlConnection("data source=65.99.205.97;initial catalog=ptstools_HelpDesk;persist security info=True;user id=ptstools_Jmondragon;password=x4fr73E*0;MultipleActiveResultSets=True"))
            {
                string query = "SP_GetTicketsByTerminal";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            objLista.Add(new G_TicketsByTerminalViewModel()
                            {
                                Terminal = dr["Terminal"].ToString(),
                                Cantidad = int.Parse(dr["Cantidad_Tickets"].ToString())
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                oconexion.Close();
            }

            return objLista;
        }
        public List<G_TicketsByTerminalViewModel> GetTicketsByTerminalOnLastMonth()
        {

            List<G_TicketsByTerminalViewModel> objLista = new List<G_TicketsByTerminalViewModel>();


            using (SqlConnection oconexion = new SqlConnection("data source=65.99.205.97;initial catalog=ptstools_HelpDesk;persist security info=True;user id=ptstools_Jmondragon;password=x4fr73E*0;MultipleActiveResultSets=True"))
            {
                string query = "SP_GetTicketsByTerminalOnLastMonth";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            objLista.Add(new G_TicketsByTerminalViewModel()
                            {
                                Terminal = dr["Terminal"].ToString(),
                                Cantidad = int.Parse(dr["Cantidad_Tickets"].ToString())
                            });
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                oconexion.Close();
            }

            return objLista;
        }
    }
}