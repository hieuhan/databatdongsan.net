using System;
using System.Configuration;

namespace databatdongsan.helper
{
    public class ConstantHelper
    {
        public static string Domain = ConfigurationManager.AppSettings["Domain"] ?? "";
        public static string RootPath = ConfigurationManager.AppSettings["RootPath"] ?? "/";
        public static string PageTitle = ConfigurationManager.AppSettings["PageTitle"] ?? "";
        public static string MediaPath = ConfigurationManager.AppSettings["MediaPath"] ?? "Uploaded/";
        public static string NoImageUrl = ConfigurationManager.AppSettings["NoImageUrl"] ?? "~/assets/images/avatar/default.png";
        public static string CommonConstr = ConfigurationManager.AppSettings["CommonConstr"] ?? string.Empty;
        public static string ExcelConstr = ConfigurationManager.AppSettings["ExcelConstr"] ?? string.Empty;
        public static string UserSession = ConfigurationManager.AppSettings["UserSession"] ?? "databatdongsan.net";
        public static string UserCookie = ConfigurationManager.AppSettings["UserCookie"] ?? "_databatdongsan.net_";
        public static string ActionStatusSuccess = ConfigurationManager.AppSettings["ActionStatusSuccess"] ?? "OK";
        public static byte StatusIdActivated = Convert.ToByte(ConfigurationManager.AppSettings["StatusIdActivated"] ?? "1");
        public static byte StatusIdUnapproved = Convert.ToByte(ConfigurationManager.AppSettings["StatusIdUnapproved"] ?? "3");
        public static byte UserStatusIdApproved = Convert.ToByte(ConfigurationManager.AppSettings["ActionStatusSuccess"] ?? "1");
        public static byte UserStatusIdUnapproved = Convert.ToByte(ConfigurationManager.AppSettings["UserStatusIdUnapproved"] ?? "2");
        public static string FileAllowedUpload = ConfigurationManager.AppSettings["FileAllowedUpload"] ?? "jpeg,jpg,png,gif";
        public static int MediaWidth = Convert.ToInt32(ConfigurationManager.AppSettings["MediaWidth"] ?? "0");
        public static int MediaHeight = Convert.ToInt32(ConfigurationManager.AppSettings["MediaHeight"] ?? "0");
        public static int MediaMobileWidth = Convert.ToInt32(ConfigurationManager.AppSettings["MediaMobileWidth"] ?? "0");
        public static int MediaMobileHeight = Convert.ToInt32(ConfigurationManager.AppSettings["MediaMobileHeight"] ?? "0");
        public static int MediaThumnailWidth = Convert.ToInt32(ConfigurationManager.AppSettings["MediaThumnailWidth"] ?? "0");
        public static int MediaThumnailHeight = Convert.ToInt32(ConfigurationManager.AppSettings["MediaThumnailHeight"] ?? "0");
        public static int MediaIconWidth = Convert.ToInt32(ConfigurationManager.AppSettings["MediaIconWidth"] ?? "0");
        public static int MediaIconHeight = Convert.ToInt32(ConfigurationManager.AppSettings["MediaIconHeight"] ?? "0");
        public static int SiteId = Convert.ToInt32(ConfigurationManager.AppSettings["SiteId"] ?? "1000");
        public static int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"] ?? "50");
    }
}
