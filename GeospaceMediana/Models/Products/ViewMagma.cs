using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class ViewMagma
    {
        public List<int> values = new List<int>();
        public int error;
        public ViewMagma(int stationCode, DateTime date)
        {
            Station theStation = Station.GetByCode(stationCode);
            if (theStation == null)
            {
                error = 1;
                return;
            }
            DateTime startDate = date.AddDays(-2);
            List<CodeMagma> theCodes = (List<CodeMagma>)CodeMagma.GetByPeriod(theStation, startDate.Year, startDate.Month, startDate.Day, date.Year, date.Month, date.Day);
            theCodes.Reverse();
            List<ViewMagma> theViews = new List<ViewMagma>();
            for (int i = 0; i < 24; i++)
            {
                if (i < theCodes.Count)
                {
                    values.Add(theCodes[i].value);
                }
                else
                {
                    values.Add(0);
                }
            }
        }
    }
}