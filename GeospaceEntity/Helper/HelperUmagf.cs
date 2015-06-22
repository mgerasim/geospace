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
        public static void Umagf_Group1_DateCreate( string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[num].Length;
            theCodeUmagf.DD = Convert.ToInt32(arrayGroups[num].Substring(0, 2));
            theCodeUmagf.HH = Convert.ToInt32(arrayGroups[num].Substring(len - 2, 2));     
        }

        public static void Umagf_BigGroup1_NumStation(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            theCodeUmagf.Station.Code = Convert.ToInt32(arrayGroups[num]);
        }
        public static void Umagf_BigGroup2_FullData(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[num].Length;
            theCodeUmagf.YYYY = DateTime.Now.Year;
            theCodeUmagf.MM = Convert.ToInt32(arrayGroups[num].Substring(1, 2));
            theCodeUmagf.DD = Convert.ToInt32(arrayGroups[num].Substring(3, 2));
        }
            //получить из Umagf AK
        public static void Umagf_Group2_AK(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[num].Length;
            if (arrayGroups[num][0] == '1' && Char.IsDigit( arrayGroups[num][len - 2])
                                           && Char.IsDigit( arrayGroups[num][len - 1]))
                theCodeUmagf.ak = Convert.ToInt32(arrayGroups[num].Substring(len - 2, 2));
        }

        //получить из Umagf K-индексы
        public static void Umagf_Group3_K_index(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[num+1].Length;
            if (arrayGroups[num + 1][0] == '2')
            {
                if( Char.IsDigit(arrayGroups[num + 1][len - 4]) )
                    theCodeUmagf.k1 = Convert.ToInt32(arrayGroups[num + 1].Substring(len - 4, 1));
                if (Char.IsDigit(arrayGroups[num + 1][len - 3]))
                    theCodeUmagf.k2 = Convert.ToInt32(arrayGroups[num + 1].Substring(len - 3, 1));
                if (Char.IsDigit(arrayGroups[num + 1][len - 2]))
                    theCodeUmagf.k3 = Convert.ToInt32(arrayGroups[num + 1].Substring(len - 2, 1));
                if (Char.IsDigit(arrayGroups[num + 1][len - 1]))
                    theCodeUmagf.k4 = Convert.ToInt32(arrayGroups[num + 1].Substring(len - 1, 1));
            }
            len = arrayGroups[num + 2].Length;
            if (arrayGroups[num + 2][0] == '3')
            {
                if (Char.IsDigit(arrayGroups[num + 2][len - 4]))
                    theCodeUmagf.k5 = Convert.ToInt32(arrayGroups[num + 2].Substring(len - 4, 1));
                if (Char.IsDigit(arrayGroups[num + 2][len - 3]))
                theCodeUmagf.k6 = Convert.ToInt32(arrayGroups[num + 2].Substring(len - 3, 1));
                if (Char.IsDigit(arrayGroups[num + 2][len - 2]))
                    theCodeUmagf.k7 = Convert.ToInt32(arrayGroups[num + 2].Substring(len - 2, 1));
                if (Char.IsDigit(arrayGroups[num + 2][len - 1]))
                    theCodeUmagf.k8 = Convert.ToInt32(arrayGroups[num + 2].Substring(len - 1, 1));
            }
        }
    }
}
