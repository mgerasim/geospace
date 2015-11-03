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
            if (GetIP().Substring(0,10) =="192.168.72")
            {
                return true;
            }
            if (GetIP().Substring(0, 10) == "192.0.0.1")
            {
                return true;
            }

            return false;
        }
        public static string GetIP()
        {

            string ip = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
            return ip;
        }
    }
}