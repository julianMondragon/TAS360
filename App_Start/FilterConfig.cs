using System.Web;
using System.Web.Mvc;

namespace TAS360
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new Filters.VerificaSession());
        }
    }
}
