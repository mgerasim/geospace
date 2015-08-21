using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Common
{
    public class DateTimeKhabarovsk
    {
        private static TimeZoneInfo _vladZone = null;

        public static DateTime Now
        {
            get
            {
                if(_vladZone == null)
                {
                    _vladZone = TimeZoneInfo.FindSystemTimeZoneById("Vladivostok Standard Time");
                }
                
                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, _vladZone);
            }
            
        }
    }
}