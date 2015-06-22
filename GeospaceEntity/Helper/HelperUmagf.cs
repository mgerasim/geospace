using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Helper
{
    public static class HelperUmagf
    {
        //получить из Umagf день, часы, минуты
        public static void Umagf_Group1_DateCreate( string strUmagf, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf )
        {
            int len = strUmagf.Length;
            theCodeUmagf.DD = Convert.ToInt32(strUmagf.Substring(0, 2));
            theCodeUmagf.HH = Convert.ToInt32(strUmagf.Substring( len-2, 2) );     
        }

        //получить из Umagf AK
        public static void Umagf_Group2_AK(string strUmagf, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = strUmagf.Length;
            theCodeUmagf.ak = Convert.ToInt32(strUmagf.Substring(len - 2, 2));
        }

        //получить из Umagf K-индексы
        public static void Umagf_Group3_K_index(string[] arrayGroups, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[3].Length;
            theCodeUmagf.k1 = Convert.ToInt32(arrayGroups[3].Substring(len - 4, 1));
            theCodeUmagf.k2 = Convert.ToInt32(arrayGroups[3].Substring(len - 3, 1));
            theCodeUmagf.k3 = Convert.ToInt32(arrayGroups[3].Substring(len - 2, 1));
            theCodeUmagf.k4 = Convert.ToInt32(arrayGroups[3].Substring(len - 1, 1));

            len = arrayGroups[4].Length;
            theCodeUmagf.k5 = Convert.ToInt32(arrayGroups[4].Substring(len - 4, 1));
            theCodeUmagf.k6 = Convert.ToInt32(arrayGroups[4].Substring(len - 3, 1));
            theCodeUmagf.k7 = Convert.ToInt32(arrayGroups[4].Substring(len - 2, 1));
            theCodeUmagf.k8 = Convert.ToInt32(arrayGroups[4].Substring(len - 1, 1));
        }
    }
}
