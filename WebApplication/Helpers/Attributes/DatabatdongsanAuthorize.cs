using databatdongsan.helper;
using databatdongsan.library;
using Microsoft.Ajax.Utilities;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebApplication.Helpers.Sercurity;
using WebApplication.Models;

namespace WebApplication.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class DatabatdongsanAuthorize : AuthorizeAttribute
    {
        protected virtual MyPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as MyPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var user = httpContext.User as MyPrincipal;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            var routeData = httpContext.Request.RequestContext.RouteData;
            string currentAction = routeData.GetRequiredString("action"),
            currentController = routeData.GetRequiredString("controller"),
            currentArea = string.Empty;

            if (routeData.DataTokens.TryGetValue("area", out object area))
            {
                currentArea = area.ToString();
            }

            string path = string.Format("{0}/{1}/{2}", currentArea, currentController, currentAction).ToLower();


            if (user.Identity.IsAuthenticated)
            {
                if (databatdongsan.library.Users.Static_GetHasPriv(user.UserName, user.UserId, string.Format("{0}/{1}", currentController, currentAction)) != 1)
                {
                    return false;
                }
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var url = new UrlHelper(filterContext.RequestContext);
            var accessDeniedUrl = url.Action("AccessDenied", "Error");

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Completed = false,
                            Message = "Bạn không có quyền truy cập nội dung này.",
                            ReturnUrl = accessDeniedUrl
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    controller = "Error",
                                    action = "AccessDenied"
                                }));
                }
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Completed = false,
                            ReturnUrl = filterContext.HttpContext.Request.UrlReferrer != null && filterContext.HttpContext.Request.UrlReferrer.PathAndQuery.IndexOf("login.html", StringComparison.Ordinal) < 0 ? string.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, filterContext.HttpContext.Request.UrlReferrer.PathAndQuery) : string.Concat(FormsAuthentication.LoginUrl, "?ReturnUrl=/")
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Login",
                                returnUrl = request.Url?.GetComponents(UriComponents.PathAndQuery,
                                    UriFormat.SafeUnescaped)
                            }));
                }
            }
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //    {
        //        // Don't check for authorization as AllowAnonymous filter is applied to the action or controller  
        //        return;
        //    }
        //    var request = filterContext.HttpContext.Request;
        //    var url = new UrlHelper(filterContext.RequestContext);
        //    var accessDeniedUrl = url.Action("AccessDenied", "Error");
        //    string actionName = filterContext.RouteData.Values["action"].ToString(),
        //        controllerName = filterContext.RouteData.Values["controller"].ToString();
        //    if (SessionHelper.HasValue<UserSessionVM>(ConstantHelper.UserSession))
        //    {
        //        UserSessionVM currentUser = SessionHelper.Get<UserSessionVM>(ConstantHelper.UserSession);

        //        if (databatdongsan.library.Users.Static_GetHasPriv(currentUser.UserName, currentUser.UserId, $"{controllerName}/{actionName}") != 1)
        //        {
        //            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        //            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        //            if (request.IsAjaxRequest())
        //            {
        //                filterContext.Result = new JsonResult
        //                {
        //                    Data = new
        //                    {
        //                        error = true,
        //                        signinerror = true,
        //                        message = "Quý khách không có quyền truy cập chức năng này!",
        //                        url = accessDeniedUrl
        //                    },
        //                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //                };
        //            }
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(new
        //                                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        //        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        //        if (request.Url != null)
        //            if (request.IsAjaxRequest())
        //            {
        //                filterContext.Result = new JsonResult
        //                {
        //                    Data = new
        //                    {
        //                        error = true,
        //                        signinerror = true,
        //                        url = request.UrlReferrer != null && !string.IsNullOrWhiteSpace(request.UrlReferrer.ToString()) ? $"/login.html?returnUrl={request.UrlReferrer.PathAndQuery}" : "/login.html"
        //                    },
        //                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //                };
        //            } 
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(
        //                    new RouteValueDictionary(
        //                        new
        //                        {
        //                            controller = "Account",
        //                            action = "Login",
        //                            returnUrl = request.Url?.GetComponents(UriComponents.PathAndQuery,
        //                                UriFormat.SafeUnescaped)
        //                        }));
        //            }      
        //    }
        //}
    }
}