using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Utils
{
    public class Util
    {
        static public bool IsLocal()
        {
            string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if (GetIP().Substring(0,3) =="192")
            {
                return true;
            }
            if (GetIP().Substring(0, 3) == "127")
            {
                return true;
            }
            if (GetIP().Substring(0, 3) == "::1")
            {
                return true;
            }

            return false;
        }
        public static string GetIP()
        {

            string ip = HttpContext.Current.Request.UserHostAddress;
            return ip;
        }
    }
}