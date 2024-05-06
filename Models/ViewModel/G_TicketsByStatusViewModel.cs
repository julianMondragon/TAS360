using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAS360.Models.ViewModel
{
    public class G_TicketsByStatusViewModel
    {
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public DateTime Mas_Reciente { get; set; }
        public DateTime Mas_Antiguo { get; set; }
    }
}