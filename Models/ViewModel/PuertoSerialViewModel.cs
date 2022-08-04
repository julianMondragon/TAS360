using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.IO.Ports;

namespace TAS360.Models.ViewModel
{
    /// <summary>
    /// Viewmodel del puerto serial.
    /// </summary>
    public class PuertoSerialViewModel
    {
        //PortName
        [Required]
        [Display(Name = "Nombre del Puerto")]
        public string Name { get; set; }
        //BaudRate
        [Required]
        [Display(Name = "BaudRate")]
        public int BaudRate { get; set; }
        //Parity
        [Required]
        [Display(Name = "Parity")]
        public Parity Parity { get; set; }
        //DataBits
        [Required]
        [Display(Name = "DataBits")]
        public int DataBits { get; set; }
        //StopBits
        [Required]
        [Display(Name = "StopBits")]
        public StopBits StopBits { get; set; }
        //Handshake
        [Required]
        [Display(Name = "Handshake")]
        public Handshake Handshake { get; set; }

        [Display(Name = "Comando a enviar")]
        public string Commant_Send { get; set; }
    }
}