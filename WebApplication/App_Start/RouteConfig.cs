using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
               "Login",
               "login.html",
               new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
               "Profile",
               "profile.html",
               new { controller = "Account", action = "EditProfile" }
            );

            routes.MapRoute(
               "ChangePassword",
               "change-password.html",
               new { controller = "Account", action = "ChangePassword" }
            );

            routes.MapRoute(
               "AddNewCustomer",
               "api/customer/add.html",
               new { controller = "Ajax", action = "AddNewCustomer" }
            );

            routes.MapRoute(
               "Users",
               "user/list.html",
               new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
               "AddNewUser",
               "user/add.html",
               new { controller = "User", action = "Insert" }
            );

            routes.MapRoute(
               "UserUpdate",
               "user/{UserId}/edit.html",
               new { controller = "User", action = "Update", UserId = UrlParameter.Optional },
               new { UserId = @"\d+" }
            );

            routes.MapRoute(
               "DeleteUser",
               "user/{UserId}/delete.html",
               new { controller = "User", action = "Delete", UserId = UrlParameter.Optional },
               new { UserId = @"\d+" }
            );

            routes.MapRoute(
               "GetUsers",
               "api/user/get.html",
               new { controller = "Ajax", action = "GetUsers" }
            );

            routes.MapRoute(
               "RemoveCustomer",
               "api/customer/remove.html",
               new { controller = "Ajax", action = "RemoveCustomer" }
            );

            routes.MapRoute(
               "EditCustomerRow",
               "ui/customer/edit.html",
               new { controller = "Ajax", action = "EditCustomerRow" }
            );

            routes.MapRoute(
               "ViewCustomerRow",
               "ui/customer/view.html",
               new { controller = "Ajax", action = "ViewCustomerRow" }
            );

            routes.MapRoute(
               "ImportFromExcel",
               "import-from-excel.html",
               new { controller = "Home", action = "ImportFromExcel" }
            );

            routes.MapRoute(
               "Charts",
               "charts.html",
               new { controller = "Home", action = "Charts" }
            );

            routes.MapRoute(
               "GetCharts",
               "api/charts/get.html",
               new { controller = "Ajax", action = "GetCharts" }
            );

            routes.MapRoute(
               "FAQ",
               "faq.html",
               new { controller = "Home", action = "FAQ" }
            );

            routes.MapRoute(
               "Logout",
               "logout.html",
               new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
              "BadRequest",
              "400.html",
              new { controller = "Error", action = "BadRequest" }
            );

            routes.MapRoute(
              "NotFound",
              "404.html",
              new { controller = "Error", action = "NotFound" }
            );

            routes.MapRoute(
               "AccessDenied",
               "access-denied.html",
               new { controller = "Error", action = "AccessDenied" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
