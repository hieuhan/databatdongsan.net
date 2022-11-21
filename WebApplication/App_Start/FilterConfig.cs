using System.Web.Mvc;
using WebApplication.Helpers.Attributes;

namespace WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new DatabatdongsanHandleErrorAttribute());
            filters.Add(new MyAuthTicketAttribute());
        }
    }
}
