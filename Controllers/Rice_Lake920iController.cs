using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Ports;
using System.Threading;
using TAS360.Models.ViewModel;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.EMMA;
using TAS360.Models;

namespace TAS360.Controllers
{
    public class Rice_Lake920iController : Controller
    {
        Thread readThread = new Thread(Read);
        public static bool _continue;
        public static SerialPort _serialPort;
        public static string transaccions;
        public static string warnings;
        public static string CommandReaded;

        // GET: Rice_Lake920i
        public ActionResult Index()
        {
            PuertoSerialViewModel PuertoserialViewModel = new PuertoSerialViewModel();
            //Evalua si el puerto esta abierto 
            if (_serialPort != null && _serialPort.IsOpen)
            {
                ViewBag.IsOpen = true;
                if (transaccions != null)
                {
                    ViewBag.trasacciones += transaccions;
                }
                if (warnings != null)
                {
                    ViewBag.Warning = warnings;
                }
                return View();
            }
            else
            {
                ViewBag.IsOpen = false;
                if (warnings != null)
                {
                    ViewBag.Warning = warnings;
                }
                // Create a new SerialPort object with default settings.
                _serialPort = new SerialPort();
                GetCatalogos();
                if (_serialPort.BaudRate != 0)
                    PuertoserialViewModel.BaudRate = _serialPort.BaudRate;
                if (_serialPort.DataBits != 0)
                    PuertoserialViewModel.DataBits = _serialPort.DataBits;
                if (_serialPort.Parity != 0)
                    PuertoserialViewModel.Parity = _serialPort.Parity;
                if (_serialPort.StopBits != 0)
                    PuertoserialViewModel.StopBits = _serialPort.StopBits;
                if (_serialPort.Handshake != 0)
                    PuertoserialViewModel.Handshake = _serialPort.Handshake;

                return View(PuertoserialViewModel);
            }
        }
        // Cambia la lógica de envío para que trabaje con ASCII en lugar de hexadecimal
        public ActionResult SendCommand(PuertoSerialViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Commant_Send))
                {
                    warnings = "Comando Null o vacío inválido";
                    return Redirect("Index");
                }

                // Enviar el comando directamente como una cadena ASCII
                string commandToSend = model.Commant_Send;
                _serialPort.Write(commandToSend);

                // Mostrar la transacción
                transaccions += "> " + commandToSend + "\n";
                warnings = "";
            }
            catch (Exception ex)
            {
                warnings = ex.Message;
            }

            return Redirect("Index");
        }

        // Cambia la lógica de lectura para manejar datos ASCII
        public ActionResult ReadCommandPort()
        {
            try
            {
                //Logs
                string path = Server.MapPath("~/Logs/RiceLake/");
                Log oLog = new Log(path);
                
                // Buffer para recibir los datos
                string result = _serialPort.ReadExisting(); // Leer los datos recibidos como cadena ASCII
                oLog.Add("Datos recibidos ReadCommandPort: \r\n" + result);
                if (!string.IsNullOrEmpty(result))
                {
                    // Almacenar las transacciones leídas
                    transaccions += "* " + result + "\n";
                    warnings = "";
                }
                else
                {
                    warnings = "No se recibió respuesta.";
                }
            }
            catch (Exception ex)
            {
                warnings = ex.Message;
            }

            return Redirect("Index");
        }


        /// <summary>
        /// Metodo que abre el puerto serial. 
        /// </summary>
        /// <param name="Port"></param>
        /// <returns></returns>
        public ActionResult OpenPort(PuertoSerialViewModel Port)
        {
            try
            {
                _serialPort.PortName = Port.Name;
                _serialPort.BaudRate = Port.BaudRate;
                _serialPort.DataBits = Port.DataBits;
                _serialPort.Parity = Port.Parity;
                _serialPort.StopBits = Port.StopBits;
                _serialPort.Handshake = Port.Handshake;
                // Set the read/write timeouts
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                _serialPort.Open();
                warnings = "";
                //_continue = true;
                //readThread.Start();
            }
            catch (Exception ex)
            {
                warnings = ex.Message;
            }
            return Redirect("Index");

        }

        /// <summary>
        /// Metodo que cierra el puerto serial.
        /// </summary>
        /// <returns></returns>
        public ActionResult ClosePort()
        {
            _serialPort.Close();
            _continue = false;
            transaccions = null;
            warnings = "";
            return Redirect("Index");
        }

        /// <summary>
        /// Hilo (No se usa actulamente)
        /// </summary>
        public static void Read()
        {
            while (_continue)
            {
                try
                {

                    //string message = _serialPort.ReadLine();
                    //Console.WriteLine("Leyendo ....");

                }
                catch (TimeoutException) { }
            }
        }

        /// <summary>
        /// Obtiene el Catalogo de puertos disponibles.
        /// </summary>
        private void GetCatalogos()
        {

            List<SelectListItem> ListCOMs = new List<SelectListItem>();

            foreach (string s in SerialPort.GetPortNames())
            {
                ListCOMs.Add(new SelectListItem
                {
                    Text = s,
                    Value = s
                });
            }
            if (ListCOMs.Count == 0)
            {
                ListCOMs.Add(new SelectListItem
                {
                    Text = "Not Available Ports COM",
                    Value = "00"
                });
            }
            ViewBag.ListCOMs = ListCOMs;

        }

        /// <summary>
        /// Convierte una cadena en formato hexadecimal a un arreglo de bytes 
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] FromHexStringToArrBytes(string hexString)
        {
            var bytes = new byte[(hexString.Length / 2)];
            for (var i = 0; i < bytes.Length; i++)
            {
                string substring = hexString.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(substring, 16);
            }

            return bytes;
        }
        /// <summary>
        /// Metodo que devuelve una vista con informacion correspondiente a la bascula.
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeerPeso()
        {
            PuertoSerialViewModel PuertoserialViewModel = new PuertoSerialViewModel();
            string path = Server.MapPath("~/Logs/RiceLake/");
            Log oLog = new Log(path);
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    ViewBag.IsOpen = true;
                  
                    _serialPort.DiscardInBuffer();
                    _serialPort.DiscardOutBuffer();
                   
                    // Leer el buffer
                    string respuesta = _serialPort.ReadLine();
                    
                    // Aquí procesas la respuesta para obtener el peso en el formato correcto
                    string peso1 = ProcesarRespuestaPeso(respuesta);
                    string peso2 = ProcesarRespuestaPeso2(respuesta);
                    oLog.Add("peso1: " + peso1);
                    oLog.Add("peso2: " + peso2);
                    if(((User)Session["User"]) != null && ((User)Session["User"]).nombre != null)
                            oLog.Add("Usuario : " + ((User)Session["User"]).nombre);

                    // Devuelves el peso a la vista
                    return Json(new { 
                        peso = peso1,
                        peso2 = peso2
                    }, JsonRequestBehavior.AllowGet);
                    
                }
                else
                {
                    ViewBag.IsOpen = false;
                    if (warnings != null)
                    {
                        ViewBag.Warning = warnings;
                    }
                    // Create a new SerialPort object with default settings.
                    _serialPort = new SerialPort();
                    GetCatalogos();
                    if (_serialPort.BaudRate != 0)
                        PuertoserialViewModel.BaudRate = _serialPort.BaudRate;
                    if (_serialPort.DataBits != 0)
                        PuertoserialViewModel.DataBits = _serialPort.DataBits;
                    if (_serialPort.Parity != 0)
                        PuertoserialViewModel.Parity = _serialPort.Parity;
                    if (_serialPort.StopBits != 0)
                        PuertoserialViewModel.StopBits = _serialPort.StopBits;
                    if (_serialPort.Handshake != 0)
                        PuertoserialViewModel.Handshake = _serialPort.Handshake;

                    return View("Index",PuertoserialViewModel);
                }
                
            }
            catch (Exception ex)
            {
                oLog.Add("excepcion encontrada:"  + ex.Message);
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private string ProcesarRespuestaPeso(string respuesta)
        {
            //string path = Server.MapPath("~/Logs/RiceLake/");
            //Log oLog = new Log(path);
            //oLog.Add("Procesar respuesta Peso: " + respuesta);
            var valor1 = respuesta.Split('*');
            if (valor1.Length >= 1)
            {
                respuesta = valor1[0];
            }

            return respuesta; 
        }

        private string ProcesarRespuestaPeso2(string respuesta)
        {
            var valor1 = respuesta.Split('*');
            if (valor1.Length >= 1)
            {
                respuesta = valor1[1];
            }


            return respuesta; 
        }
    }
}
