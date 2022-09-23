using System;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;

namespace TAS360.Models.ViewModel
{
    /// <summary>
    /// Viewmodel del Socket
    /// </summary>
    public class SocketViewModel
    {
        //Info del Host 
        [Required]
        [Display(Name = "Host")]
        public string ipHostInfo { get; set; }
        //Ip adresss
        [Required]
        [Display(Name = "Dirección IP")]
        public string ipAddress { get; set; }
        //Port
        [Required]
        [Display(Name = "Puerto")]
        public int Port { get; set;  }

        [Display(Name = "Comando a enviar en hexadecimal")]
        public string Commant_Send { get; set; }

    }
}