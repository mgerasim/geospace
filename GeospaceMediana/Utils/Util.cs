﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Utils
{
    public class Util
    {
        static public bool IsLocal()
        {
            bool key = true;
            try
            {
                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                string ip = GetIP();
                string addr = HttpContext.Current.Request.Params["REMOTE_ADDR"];
                //object forwarded = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                //Тестирование убран интерфейс отслеживания рабочих адрессов
                if (ip == "10.8.3.192")
                {
                    key = false;
                }
                //if (ip.Substring(0, 3) == "127")
                //{
                //    return true;
                //}
                if (ip != "::1" && ip != "127.0.0.1")
                {
                    GeospaceEntity.Models.Request RequestUser = new GeospaceEntity.Models.Request(ip, controller, action);
                    RequestUser.Save();
                }
            }
            catch (Exception ex)
            {
                GeospaceEntity.Models.Error theCodeError = new GeospaceEntity.Models.Error();
                theCodeError.Raw = "IP";
                theCodeError.Description = ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace;
                theCodeError.Save();
            }
            return key;//Дозможность редактирования доступна всем!
        }
        public static string GetIP()
        {
            

            string ip = HttpContext.Current.Request.UserHostAddress;
            return ip;
        }
    }
}