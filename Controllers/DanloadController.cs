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

namespace TAS360.Controllers
{
    public class DanloadController : Controller
    {
        Thread readThread = new Thread(Read);
        public static bool _continue;
        public static SerialPort _serialPort;
        public static string transaccions;
        public static string warnings;
        public static string CommandReaded;
        

        /// <summary>
        /// Metodo principal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PuertoSerialViewModel PuertoserialViewModel = new PuertoSerialViewModel();
            //Evalua si el puerto esta abierto 
            if (_serialPort != null && _serialPort.IsOpen)
            {
                ViewBag.IsOpen = true;
                if(transaccions != null)
                {
                    ViewBag.trasacciones += transaccions;
                }
                if(warnings != null)
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

        /// <summary>
        /// Enviar comando
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SendCommand(PuertoSerialViewModel model)
        {
            try
            {
                if (model.Commant_Send == null)
                {
                    warnings = "Comando Null invalido";
                    return Redirect("Index");
                }                
                byte[] Bytes = FromHexStringToArrBytes(model.Commant_Send);
                _serialPort.Write(Bytes, 0, Bytes.Length);
                string result = System.Text.Encoding.UTF8.GetString(Bytes);
                transaccions += "> " + model.Commant_Send + "\n" + "> " + result;
                warnings = "";
            }
            catch (Exception ex)
            {
                warnings = ex.Message;
            }
            
            return Redirect("Index");
        }

        /// <summary>
        /// Metodo que lee el puerto serial.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadCommandPort()
        {
            try
            {
                byte[] bytes = new byte[64];
                StringBuilder sb = new StringBuilder();
                string result = "";
                //Read
                var longitud = _serialPort.Read(bytes, 0, 64);
                string respuesta = BitConverter.ToString(bytes, 0, longitud);
                respuesta = respuesta.Replace("-", "");
                result = System.Text.Encoding.UTF8.GetString(bytes, 0 ,longitud);

                transaccions += "* " + respuesta + "\n" + "* " + result + "\n";
                warnings = "";

            }
            catch(Exception ex)
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
            _continue=false;
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
            if(ListCOMs.Count == 0)
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
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FromHexToString(string hex)
        {
            string cadena = "";



            return cadena;
        }
    }
}