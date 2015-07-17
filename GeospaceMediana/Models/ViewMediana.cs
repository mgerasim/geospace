using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class ViewMediana
    {
        protected IList<GeospaceEntity.Models.Mediana> medianaValues;

        public ViewMediana(IList<GeospaceEntity.Models.Mediana> medianaValues)
        {
            this.medianaValues = medianaValues;
        }

        public Mediana GetValue(int hour, int numberRange)
        {
            try
            {
                return medianaValues.Where(x => x.HH == hour && x.RangeNumber == numberRange).Single();
            }
            catch (Exception)
            {
                return new Mediana();
            }
        }

    }
}