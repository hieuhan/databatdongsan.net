using System.Web;
using WebApplication.Helpers.Sercurity;

namespace WebApplication.Helpers.Extensions
{
    public static class AppExtensions
    {
        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.User != null &&
                       HttpContext.Current.User.Identity != null &&
                       HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public static MyPrincipal GetCurrentUser()
        {
            return IsAuthenticated ? ((MyPrincipal)HttpContext.Current.User) : null;
        }

        public static string DefaultIfEmpty(this string str, string defaultValue = "", bool considerWhiteSpaceIsEmpty = false)
        {
            return (considerWhiteSpaceIsEmpty ? string.IsNullOrWhiteSpace(str) : string.IsNullOrEmpty(str)) ? defaultValue : str;
        }
    }
}