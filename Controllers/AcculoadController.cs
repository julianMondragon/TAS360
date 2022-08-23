using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAS360.Models.ViewModel;
using System.Collections.Generic;

namespace TAS360.Controllers
{
    public class AcculoadController : Controller
    {

        public static string Warning = "";
        public static string Info = "";
        public static bool IsOpen = false;
        public static string transacciones;
        public static SocketViewModel socket = new SocketViewModel();
        public static Socket sender;

        /// <summary>
        /// (GET) Metodo principal de Accuload
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {           
            StartClient();
            if(Warning != "")
            {
                ViewBag.Warning = Warning;
            }
            if(!string.IsNullOrEmpty(transacciones))
            {
                ViewBag.trasacciones = transacciones;
            }
            ViewBag.IsOpen = IsOpen;
            return View(socket);
        }

        /// <summary>
        /// Connect to a remote device.
        /// Connect the socket to the remote endpoint. Catch any errors. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SocketViewModel model)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
             
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[3];
                // Create EndPoind
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(model.ipAddress), model.Port);
                // Create a TCP/IP  socket.  
                sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                              
                sender.Bind(remoteEP);
                sender.Listen(10); 

                ViewBag.Info = "Socket escuchando ... ";
                IsOpen = true;
                ViewBag.IsOpen = IsOpen;
                Warning = "";
                
                //// Release the socket.  
                //sender.Shutdown(SocketShutdown.Both);
                //sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Warning = "ArgumentNullException : " + ane.ToString();
                ViewBag.Warning = "ArgumentNullException : " + ane.ToString();
            }
            catch (SocketException se)
            {
                Warning = "SocketException : "+ se.ToString();
                ViewBag.Warning = "SocketException : " + se.ToString();
            }
            catch (Exception e)
            {
                Warning = "Unexpected exception : "+ e.ToString();
                ViewBag.Warning = "Unexpected exception : " + e.ToString();
            }

            return View();
        }

        public static void StartClient()
        {             
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());                
                IPAddress ipAddress = ipHostInfo.AddressList[2];

                socket.ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()).HostName;
                socket.ipAddress = ipAddress.ToString();
                socket.Port = 11000;
                Warning = "";

            }
            catch (Exception e)
            {
                Warning = e.Message;
            }
        }

        /// <summary>
        /// Metodo que lee el socket.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadCommandPort()
        {
            try
            {
                byte[] buffer = new byte[128];
                Socket cliente = sender.Accept();
                cliente.Receive(buffer);
                int cont = buffer.Count();
                string result = System.Text.Encoding.UTF8.GetString(buffer, 0 , cont);
                transacciones += "* " + result + "\n";
               

            }
            catch (Exception ex)
            {
                //warnings = ex.Message;
            }
            return Redirect("Index");
        }


    }
}