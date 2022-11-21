using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication.Helpers.Attributes
{
    public class DatabatdongsanHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Completed = false,
                        Message = "Quý khách vui lòng thử lại sau!",
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Error",
                    action = "NotFound"
                }));
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}