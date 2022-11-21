using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace databatdongsan.helper
{
    public static class StringHelper
    {
        public static string CleanStr(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                text = text.Replace("'", "&#39;");
            return text;
        }

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        public static string ToUrlSlug(this string value)
        {
            string retVal = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                retVal = value.ToLowerInvariant();
                retVal = RemoveSign4VietnameseString(retVal);
                retVal = Regex.Replace(retVal, @"\s", "-", RegexOptions.Compiled);
                retVal = Regex.Replace(retVal, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);
                retVal = retVal.Trim('-', '_');
                retVal = Regex.Replace(retVal, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            }
            return retVal;
        }

        public static string InjectionString(this string str)
        {
            string retVal = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                string[] sqlCheckList =
                {
                    "/*",
                    "*/",
                    "char",
                    "nchar",
                    "varchar",
                    "nvarchar",
                    "alter",
                    "begin",
                    "cast",
                    "create",
                    "cursor",
                    "declare",
                    "delete",
                    "drop",
                    "end",
                    "exec",
                    "execute",
                    "fetch",
                    "insert",
                    "kill",
                    "select",
                    "sys",
                    "sysobjects",
                    "syscolumns",
                    "table",
                    "update",
                    "xp_"
                };
                retVal = str.Replace("'", "''");
                while (retVal.Contains("--"))
                {
                    retVal = retVal.Replace("--", "-");
                }
                foreach (var item in sqlCheckList)
                    retVal = retVal.Replace(item, string.Empty);
            }

            return retVal;
        }

        public static string RedirectUrl(string key, string defaultUrl = "")
        {
            string retVal = ConstantHelper.RootPath;
            if (SessionHelper.HasValue<string>(key))
            {
                retVal = SessionHelper.Get<string>(key);
            }
            else if (!string.IsNullOrEmpty(defaultUrl))
                retVal = defaultUrl;
            return retVal;
        }

        public static string StatusBootstrap(this byte statusId)
        {
            string retVal = "warning";
            if (statusId == ConstantHelper.StatusIdActivated)
                retVal = "success";
            else if (statusId == ConstantHelper.StatusIdUnapproved)
                retVal = "danger";
            return retVal;
        }

        public static string UserStatusBootstrap(this byte statusId)
        {
            string retVal = "bg-warning";
            if (statusId == ConstantHelper.UserStatusIdApproved)
                retVal = "bg-success";
            else if (statusId == ConstantHelper.UserStatusIdUnapproved)
                retVal = "bg-danger";
            return retVal;
        }

        public static string TrimmedOrDefault(this string str, string strDefault)
        {
            return string.IsNullOrEmpty(str) ? strDefault : str.Trim();
        }

        public static string BuildQueryStringUrl(this string url, NameValueCollection parameters)
        {
            string urlWithoutQuery = url.IndexOf('?') >= 0
                ? url.Substring(0, url.IndexOf('?'))
                : url;

            string queryString = url.IndexOf('?') >= 0
                ? url.Substring(url.IndexOf('?'))
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);

            foreach (string key in parameters.AllKeys)
            {
                string value = parameters[key] ?? string.Empty;
                if (queryParamList[key] != null)
                {
                    if (value == "")
                        queryParamList.Remove(key);
                    else queryParamList[key] = value;
                }
                else
                {
                    if (value == "")
                        queryParamList.Remove(key);
                    else queryParamList.Add(key, value);
                }
            }
            return $"{urlWithoutQuery}{(queryParamList.Count > 0 ? "?" : string.Empty)}{queryParamList}";
        }
        public static string BuildQueryStringUrl(this string url, NameValueCollection parameters, IList<string> searchFilter)
        {
            string urlWithoutQuery = url.IndexOf('?') >= 0
                ? url.Substring(0, url.IndexOf('?'))
                : url;

            string queryString = url.IndexOf('?') >= 0
                ? url.Substring(url.IndexOf('?'))
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);
            queryParamList.Remove("layout");
            var temp = queryParamList;
            if (temp.Count > 0 && searchFilter.IsAny())
            {
                for (var index = 0; index < temp.Count; index++)
                {
                    string key = temp.GetKey(index);
                    if (!searchFilter.Any(o => string.Equals(key, o, StringComparison.OrdinalIgnoreCase)))
                    {
                        queryParamList.Remove(key);
                    }
                }
            }
            foreach (string key in parameters.AllKeys)
            {
                string value = parameters[key] ?? string.Empty;
                if (queryParamList[key] != null)
                {
                    if (value == "")
                        queryParamList.Remove(key);
                    else queryParamList[key] = value;
                }
                else
                {
                    if (value == "")
                        queryParamList.Remove(key);
                    else queryParamList.Add(key, value);
                }
            }
            return $"{urlWithoutQuery}{(queryParamList.Count > 0 ? "?" : string.Empty)}{queryParamList}";
        }
        public static string BuildCanonicalUrl(this string url, IList<string> searchFilter)
        {
            string urlWithoutQuery = url.IndexOf('?') >= 0
                ? url.Substring(0, url.IndexOf('?'))
                : url;

            string queryString = url.IndexOf('?') >= 0
                ? url.Substring(url.IndexOf('?'))
                : null;

            var queryParamList = queryString != null
                ? HttpUtility.ParseQueryString(queryString)
                : HttpUtility.ParseQueryString(string.Empty);
            var temp = queryParamList;
            if (temp.Count > 0 && searchFilter.IsAny())
            {
                for (var index = 0; index < temp.Count; index++)
                {
                    string key = temp.GetKey(index);
                    if (!searchFilter.Any(o => string.Equals(key, o, StringComparison.OrdinalIgnoreCase)))
                    {
                        queryParamList.Remove(key);
                    }
                }
            }
            return $"{urlWithoutQuery}{(queryParamList.Count > 0 ? "?" : string.Empty)}{queryParamList}";
        }

        public static string GetUrl(this string url)
        {
            string retVal = url;
            if (string.IsNullOrEmpty(retVal))
            {
                return string.Empty;
            }

            if (!retVal.Contains("://"))
            {
                while (retVal.StartsWith("/"))
                {
                    retVal = retVal.Substring(1);
                }

                retVal = string.Concat(ConstantHelper.RootPath, retVal);
            }

            return retVal;
        }

        public static string GetImageUrl(this string filePath)
        {
            string retVal = filePath;
            if (string.IsNullOrEmpty(retVal))
            {
                return ConstantHelper.NoImageUrl;
            }

            if (!retVal.Contains("://"))
            {
                while (retVal.StartsWith("/"))
                {
                    retVal = retVal.Substring(1);
                }

                retVal = string.Concat(ConstantHelper.RootPath, retVal);
            }
            return retVal;
        }

        public static string GetImageUrl_Mobile(this string filePath)
        {
            return filePath.GetImageUrl().Replace("/Original/", "/Mobile/");
        }

        public static string GetImageUrl_Icon(this string filePath)
        {
            return filePath.GetImageUrl().Replace("/Original/", "/Icon/");
        }

        public static string StripTags(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char[] arr = new char[value.Length];
                int arrIndex = 0;
                bool check = false;
                foreach (var item in value)
                {
                    if (item == '<')
                    {
                        check = true;
                        continue;
                    }
                    if (item == '>')
                    {
                        check = false;
                        continue;
                    }
                    if (!check)
                    {
                        arr[arrIndex] = item;
                        arrIndex++;
                    }
                }
                return new string(arr, 0, arrIndex);
            }

            return value;
        }

        public static string Truncate(this string value, int mLengthRemain)
        {
            string text = value;
            if (value.Length > mLengthRemain)
            {
                text = value.Substring(0, mLengthRemain);
                string temp = value.Substring(mLengthRemain, 1);
                if (temp != " ")
                {
                    text = text.Substring(0, text.LastIndexOf(" ", StringComparison.Ordinal));
                }
                text += " ...";
            }
            value = text;
            return value;
        }
    }
}
