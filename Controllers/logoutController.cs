using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homexcellence.Controllers
{
    public class logoutController : Controller
    {
        public ActionResult logout()
        {
            Session["User"] = null;
            return RedirectToAction("index", "Access");
        }
    }
}