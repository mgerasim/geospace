using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class ViewMagma
    {
        public int station = 0;
        public int YYYY = 0;
        public int MM = 0;
        public int DD = 0;
        public int HH = 0;
        public int MI = 0;
        public int value = 0;
        public string Raw = "";
        public ViewMagma(CodeMagma theCodeMagma)
        {
            if (theCodeMagma != null)
            {
                station = theCodeMagma.Station.Code;
                YYYY = theCodeMagma.YYYY;
                MM = theCodeMagma.MM;
                DD = theCodeMagma.DD;
                HH = theCodeMagma.HH;
                MI = theCodeMagma.MI;
                value = theCodeMagma.value;
                Raw = theCodeMagma.Raw;
            }
        }

    }
}