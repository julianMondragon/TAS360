using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.IO;
using TAS360.Models.ViewModel;

namespace TAS360.Controllers
{
    public class WebAPIPemexController : Controller
    {
        // GET: WebAPIPemex
        [HttpGet]
        public ActionResult Index()
        {
            WebAPIPemexViewModel model = new WebAPIPemexViewModel();
            return View(model);
        }

        
        [HttpPost]
        public ActionResult Index(WebAPIPemexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.warning = "Formato no valido enviado en el campo fecha";
                return View("Index", model);
            }
            string path = Server.MapPath("~/ArchivosASA/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var url = $"https://api.pemex.com/API_TriAsa/api/Archivosasa/?f_sol={model.date}";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("X-ApiTriASA22-ApiKey","T2sMH$VQpy9h6&LaW#n0");
            //request.ContentType = "application/json";
            request.Accept = "*/*";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    // configurar el buffer a 2 KBytes
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;

                    using (FileStream fs = new FileStream(path + "ASA_"+model.date +".tar" , FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        //HttpWebResponse response = (HttpWebResponse)httpReq.GetResponse();

                        // obtener el stream retornado por el servidor
                        Stream stream = response.GetResponseStream();

                        // Leer del buffer 2kb cada vez
                        contentLen = stream.Read(buff, 0, buffLength);

                        // mientras existan datos en el buffer
                        while (contentLen != 0)
                        {
                            // escribir el contenido en el stream
                            fs.Write(buff, 0, contentLen);
                            contentLen = stream.Read(buff, 0, buffLength);
                        }
                        #region codigo
                        //    using (Stream strReader = response.GetResponseStream())
                        //{

                        //   
                        //    //if (strReader == null)
                        //    //    ViewBag.Isnull = true;
                        //    //ViewBag.Info = "En breve inicara la descarga ...";
                        //    //return View(model);
                        //    //using (StreamReader objReader = new StreamReader(strReader))
                        //    //{
                        //    //    string responseBody = objReader.ReadToEnd();
                        //    //    // Do something with responseBody

                        //    //}
                        //    
                        //}
                        #endregion
                    }
                    ViewBag.Info = "El archivo fue descargado exitosamente en: " + path;
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("Error en el servidor remoto: (404) No se encontró."))
                {
                    ViewBag.warning = "No existe archivo con esa fecha solicitada, rectifique la fecha. Verifique que este la fecha en el formato indicado y sin signos de separación de fechas.";
                    return View(model);
                }
                ViewBag.warning = ex.Message;
                // Handle error

            }
            return View(model);
        }
    }
}