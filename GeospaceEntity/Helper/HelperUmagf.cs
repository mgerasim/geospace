using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Helper
{
    public static class HelperUmagf
    {
        //получить из Umagf день, часы, минуты
        public static void Umagf_Group1_DateCreate( string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf, bool flag=false)
        {
            arrayGroups[num] = arrayGroups[num].Replace( "/", "" );
            int len = arrayGroups[num].Length;

            if (flag) //из коротокой строки Umagf
            {
                DateTime dt = new DateTime(theCodeUmagf.YYYY,
                    theCodeUmagf.MM,
                    Convert.ToInt32(arrayGroups[num].Substring(0, 2)));
                int DayZ = Convert.ToInt32(arrayGroups[num].Substring(0, 2));
                if (Math.Abs(DayZ - theCodeUmagf.DD) > 15)
                {
                    dt = dt.AddMonths(-1);
                    theCodeUmagf.MM = dt.Month;
                    theCodeUmagf.YYYY = dt.Year;
                }

                theCodeUmagf.DD = DayZ;
                int pos = 2;
                if (arrayGroups[num].Length == 5) pos = 3;
                theCodeUmagf.HH = Convert.ToInt32(arrayGroups[num].Substring(len - pos, 2));
            }
            else  //из длиной строки Umagf
            {
                theCodeUmagf.HH = Convert.ToInt32(arrayGroups[num].Substring(0, 2));
                theCodeUmagf.MI = Convert.ToInt32(arrayGroups[num].Substring(len - 2, 2));
            }               
        }

        //получает индекс станции, если в БД такой станции НЕ СОЗДОВАТЬ эту станцию в БД
        public static bool Umagf_BigGroup1_NumStation(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int number = Convert.ToInt32(arrayGroups[num]);
            Station theStation = (new Station().GetByCode(number));
            if (theStation == null)
            {
                return false;
                /*theStation = new Station();
                theStation.Code = number;
                theStation.Save();*/
            }
            theCodeUmagf.Station = theStation;
            return true;
        }

        //получить год/месяц/день из длиной строки Umagf
        public static void Umagf_BigGroup2_FullData(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            arrayGroups[num] = arrayGroups[num].Replace("/", "");
            if (arrayGroups[num].Length >= 5 && arrayGroups.Length > num)
            {
                DateTime dt = new DateTime(DateTime.Now.Year,
                    Convert.ToInt32(arrayGroups[num].Substring(1, 2)),
                    Convert.ToInt32(arrayGroups[num].Substring(3, 2)));
                
                //dt = dt.AddDays(-1);

                theCodeUmagf.YYYY = dt.Year;
                theCodeUmagf.MM = dt.Month;
                theCodeUmagf.DD = dt.Day;
            }
        }

        //получить из Umagf AK
        public static void Umagf_Group2_AK(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            int len = arrayGroups[num].Length;           
            
            if (arrayGroups[num][0] == '1' )
                if (Char.IsDigit(arrayGroups[num][len - 2]) || Char.IsDigit(arrayGroups[num][len - 1]))
                {
                    theCodeUmagf.ak = Convert.ToInt32(arrayGroups[num].Substring(len - 2, 2));
                }
        }

        //получить из Umagf K-индексы
        public static int Umagf_Group3_K_index(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf )
        {
            int group2 = -1, group3 = -1;
            for (int i = num + 1; i < arrayGroups.Length; i++)
            {
                if (arrayGroups[i].Length > 0)
                {
                    if (arrayGroups[i][0] == '2') group2 = i;
                    if (arrayGroups[i][0] == '3') group3 = i;
                }
            }

            if (group2 > 0)
            {
                int len = arrayGroups[group2].Length;
                if (Char.IsDigit(arrayGroups[group2][len - 4]))
                    theCodeUmagf.k1 = Convert.ToInt32(arrayGroups[group2].Substring(len - 4, 1));
                if (Char.IsDigit(arrayGroups[group2][len - 3]))
                    theCodeUmagf.k2 = Convert.ToInt32(arrayGroups[group2].Substring(len - 3, 1));
                if (Char.IsDigit(arrayGroups[group2][len - 2]))
                    theCodeUmagf.k3 = Convert.ToInt32(arrayGroups[group2].Substring(len - 2, 1));
                if (Char.IsDigit(arrayGroups[group2][len - 1]))
                    theCodeUmagf.k4 = Convert.ToInt32(arrayGroups[group2].Substring(len - 1, 1));
            }            
            if (group3 > 0)
            {
                int len = arrayGroups[group3].Length;
                if (Char.IsDigit(arrayGroups[group3][len - 4]))
                    theCodeUmagf.k5 = Convert.ToInt32(arrayGroups[group3].Substring(len - 4, 1));
                if (Char.IsDigit(arrayGroups[group3][len - 3]))
                    theCodeUmagf.k6 = Convert.ToInt32(arrayGroups[group3].Substring(len - 3, 1));
                if (Char.IsDigit(arrayGroups[group3][len - 2]))
                    theCodeUmagf.k7 = Convert.ToInt32(arrayGroups[group3].Substring(len - 2, 1));
                if (Char.IsDigit(arrayGroups[group3][len - 1]))
                    theCodeUmagf.k8 = Convert.ToInt32(arrayGroups[group3].Substring(len - 1, 1));
            }
            return group3;
        }

        //получает строку с явлениями вида: явление1.ЧЧ:ММ, явление2.ЧЧ:ММ, ...
        public static void Umagf_Events( string[] arrayGroups, int posLastGroup_KIndex, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf )
        {
            if (posLastGroup_KIndex < 0 && arrayGroups.Length < posLastGroup_KIndex + 1) return;

            for (int i = posLastGroup_KIndex + 1; i < arrayGroups.Length && arrayGroups[i].Length > 0; i++)
            {
                if (arrayGroups[i].Substring(0, 1) == "5")
                {
                    i++;
                    continue;
                }
                if (theCodeUmagf.events.Length != 0) theCodeUmagf.events += ", ";
                theCodeUmagf.events += arrayGroups[i].Substring(0, 1) + "." +
                                          arrayGroups[i].Substring(1, 2) + ":" + arrayGroups[i].Substring(3, 2);
            }
        }

        //печатает все возможные комбинации кода Umagf
        public static void Print_All_Code_Umagf(string strUmagf, List<int> listLengthLines, List<string> listComb, string pathFile)
        {
            int count = 0;
            foreach (int len in listLengthLines)
            {
                if (strUmagf.Length == len) count++;
                if (count == 10) return;
            }

            listLengthLines.Add(strUmagf.Length);
            listComb.Add(strUmagf);

            StreamWriter sw = new StreamWriter(pathFile);
            foreach( string s in listComb)
                sw.WriteLine(s);
            sw.Close();
        }
    }
}
