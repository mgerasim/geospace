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
            try
            {
                arrayGroups[num] = arrayGroups[num].Replace("/", "");
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
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема при декодировании времени", ex);
            }
        }

        //получает индекс станции, если в БД такой станции НЕ СОЗДОВАТЬ эту станцию в БД
        public static bool Umagf_BigGroup1_NumStation(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            try
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
            }
            catch( System.Exception ex)
            {
                throw new System.Exception("Ошибка в индексе станции: " + arrayGroups[num].ToString(), ex );
            }
            return true;
        }

        //получить год/месяц/день из длиной строки Umagf
        public static void Umagf_BigGroup2_FullData(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            try
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
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема в декодировании даты", ex);
            }
        }

        //получить из Umagf AK
        public static void Umagf_Group2_AK(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf)
        {
            try
            {
                int len = arrayGroups[num].Length;

                if (arrayGroups[num][0] == '1')
                    if (Char.IsDigit(arrayGroups[num][len - 2]) || Char.IsDigit(arrayGroups[num][len - 1]))
                    {
                        theCodeUmagf.ak = Convert.ToInt32(arrayGroups[num].Substring(len - 2, 2));
                    }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема при декодировании АК", ex);
            }
        }

        //получить из Umagf K-индексы
        public static int Umagf_Group3_K_index(string[] arrayGroups, int num, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf )
        {
            int group2 = -1, group3 = -1;
            try
            {               
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
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема при декодировании К-индексов", ex);
            }
            return group3;
        }

        //получает строку с явлениями вида: явление1.ЧЧ:ММ, явление2.ЧЧ:ММ, ...
        public static void Umagf_Events( string[] arrayGroups, int posLastGroup_KIndex, GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf )
        {
            try
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
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема при декодировании явлений", ex);
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
        
        public static void Umagf_Check(Models.Codes.CodeUmagf theCodeUmagf)
        {
            try
            {
                int n = 0;
                int[] array = { 0, 3, 7, 15, 27, 48, 80, 140, 240, 400 };
                double An = 0.0;
                if (theCodeUmagf.k1 != 1000) { n++; An += array[theCodeUmagf.k1]; }
                if (theCodeUmagf.k2 != 1000) { n++; An += array[theCodeUmagf.k2]; }
                if (theCodeUmagf.k3 != 1000) { n++; An += array[theCodeUmagf.k3]; }
                if (theCodeUmagf.k4 != 1000) { n++; An += array[theCodeUmagf.k4]; }
                if (theCodeUmagf.k5 != 1000) { n++; An += array[theCodeUmagf.k5]; }
                if (theCodeUmagf.k6 != 1000) { n++; An += array[theCodeUmagf.k6]; }
                if (theCodeUmagf.k7 != 1000) { n++; An += array[theCodeUmagf.k7]; }
                if (theCodeUmagf.k8 != 1000) { n++; An += array[theCodeUmagf.k8]; }
                if (n > 0)
                {
                    An = Math.Round(An / n);
                    if (GeospaceEntity.Helper.HelperUmagf.Metround(An) * 2 == theCodeUmagf.ak)
                        theCodeUmagf.ak = GeospaceEntity.Helper.HelperUmagf.Metround(An);
                    if (theCodeUmagf.ak == 1000)
                        theCodeUmagf.ak = GeospaceEntity.Helper.HelperUmagf.Metround(An);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Проблема при проверки АК", ex);
            }
        }

        private static int Metround(double p)
        {
            int x = Convert.ToInt32(p*10);
            if (x % 10 > 5) return x / 10 + 1;
            if (x % 10 < 5) return x / 10;
            if (x % 10 == 5) return ((x - 5) % 2 == 0) ? x / 10 : x / 10 + 1;
            return Convert.ToInt32(p);
        }
    }
}
