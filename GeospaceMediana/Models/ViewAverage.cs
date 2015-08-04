using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeospaceEntity.Models;

namespace GeospaceMediana.Models
{

    public class ViewAverage
    {
        public int StationCode;
        public List<Average> theAverageValues;

        public ViewAverage(int station, int year, int month, int day)
        {
            StationCode = station;

            theAverageValues = Average.GetByDate(Station.GetByCode(StationCode), year, month, day);
        }
               
    }
}