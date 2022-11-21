using System.Collections.Generic;
using System.Web;

namespace databatdongsan.helper
{
    public class SessionHelper
    {
        public static T Get<T>(string key)
        {
            var valueFromSession = HttpContext.Current.Session[key];
            if (valueFromSession is T session)
            {
                return session;
            }
            return default(T);
        }

        public static T Get<T>(string key, T defaultValue)
        {
            var valueFromSession = HttpContext.Current.Session[key];
            if (valueFromSession == null)
            {
                HttpContext.Current.Session[key] = defaultValue;
            }

            return (T)HttpContext.Current.Session[key];
        }

        public static void Save<T>(string key, T entity)
        {
            HttpContext.Current.Session[key] = entity;
        }

        public static bool HasValue<T>(string key)
        {
            return !EqualityComparer<T>.Default.Equals(Get<T>(key), default(T));
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}
