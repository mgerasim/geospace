using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Helper
{
    public static class HelperIonka
    {
        public static bool FindSpecialGroup(string str) // поиск специальной группы
        {

            if (str[1] == '/' || str[3] == '/')
                return true;
            else return false;
        }
        public static int FindTimePeriod(string str) // поиск временого периода группы
        {
            if ((str[0] == '/'))
            {
                if ((Char.IsDigit(str[1]) && Char.IsDigit(str[2]) && Char.IsDigit(str[3]) && Char.IsDigit(str[4])))
                {
                    return Convert.ToInt32(str.Substring(1));
                }
                else
                    return -1; 
            }
            else
                return -1;
        }
        public static List<string> SetListTimeSession(string[] arrayGroups, List<int> addressStartSession, int i)
        {
            List<string> list = new List<string>();
            for(int j = addressStartSession[i]; j < addressStartSession[i+1] && j < arrayGroups.Length ; j++)
                list.Add(arrayGroups[j]);
            return list;
        }
        public static string Normalize(string strIonka)
        {

            if (strIonka.IndexOf("37701 50406 01706") > -1)
            {
                strIonka = strIonka.Replace("37701 50406 01706", "37701 50617");
            }
            if (strIonka.IndexOf("37701 50406 1706 8") > -1)
            {
                strIonka = strIonka.Replace("37701 50406 1706 8", "37701 50617");
            }
            if (strIonka.IndexOf("37701 50406 01706 /") > -1)
            {
                strIonka = strIonka.Replace("37701 50406 01706 /", "37701 50617 7/1/1 /");
            }
            if (strIonka.IndexOf("7/1/ 2") > -1)
            {
                strIonka = strIonka.Replace("7/1/ 2", "7/1/2");
            }

            if (strIonka.IndexOf("7/1/ ") > -1)
            {
                strIonka = strIonka.Replace("7/1/ ", "7/1/");
            }
            string[] delimiters = new string[] { " ", "\r\n" };

            // strIonka = strIonka.Replace("///", "/ //");
            string[] strCodes = strIonka.Split(delimiters,
                                 StringSplitOptions.None);
            if (strCodes.Count() == 1)
            {
                return "";
            }
            string strIonkaNormalize = "";
            foreach (var item in strCodes)
            {
                string ss = item;

                if ((ss.Length == 10 || ss.Length == 11) && ss.IndexOf("///") > -1)
                {
                    ss = ss.Replace("///", "/ //");
                    strIonkaNormalize += ss;
                    strIonkaNormalize += " ";
                }

                //  03300//70/ 
                if (ss.Length == 10 && ss.IndexOf("00//") > -1)
                {
                    ss = ss.Replace("00//", "00 //");

                    strIonkaNormalize += ss;
                    strIonkaNormalize += " ";
                }
                if (ss.IndexOf("//7///=") > -1)
                {
                    ss = ss.Replace("//7///=", "//7//");
                    ss = ss.Replace("//7///", "//7//");
                }
                if (ss.Length == 6)
                {
                    ss = ss.Replace("=", "");
                    ss = ss.Replace("-", "");
                    if (ss.Substring(0, 2) == "00")
                    {
                        ss = ss.Replace("00", "0");
                    }
                    if (ss.Length == 6 && ss.Substring(0, 1) == "0")
                    {
                        ss = ss.Substring(1);
                    }
                }

                /*/////////////////////////////////////////////////////////////////////
                if (ss.Length == 6)
                {
                    bool flag = true;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (!Char.IsDigit(ss[i]))
                        {
                            flag = false;
                           
                        }
                    }
                    ss = ss.Replace(ss[ss.Length - 1].ToString(), "");
                    
                }
                *//////////////////////////////////////////////////////////////////////
                if (ss.Length == 4)
                {
                    ss = ss.Replace("/73/", "//73/");
                    ss = ss.Replace("//77", "//7/7");
                    ss = ss.Replace("//11", "//1/1");
                    if (ss.Length == 4)
                    {
                        ss = "0" + ss;
                    }
                }
                if (ss.Length == 5)
                {
                    if (Char.IsLetter(ss[0]))
                    {
                        ss = "\r\n" + ss;
                    }
                    strIonkaNormalize += ss;
                    strIonkaNormalize += " ";
                }
            }

            /*
            if( strIonkaNormalize != strIonka )
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\1.txt", true);

                sw.WriteLine(strIonka);
                sw.WriteLine(strIonkaNormalize);
                sw.WriteLine("\n");

                sw.Close();
            }
            */
            return strIonkaNormalize;
        }
        public static int ParseToken(string strToken)
        {
            int res = 0;
            if (strToken.IndexOf("/") > -1)
            {
                strToken = strToken.Replace("/", "");
                if (strToken == "")
                {
                    strToken = "0";
                }
                res += 1000;
            }
            res += Convert.ToInt32(strToken);
            return res;
        }
        public static string Check(string strIonka)
        {
            strIonka = strIonka.Replace("\"", "");
            strIonka = strIonka.Replace("///", "/ //");

            string[] arrayString = strIonka.Split(' ');
            if (arrayString[0] != "ionka" && arrayString[0] != "IONKA")
            {
                throw new Exception("Не явлейтсе строкой с кодом Ionka");
            }
            string tokenGroup02 = arrayString[1];
            int numberStation = Convert.ToInt32(tokenGroup02);
            if (numberStation == 43501)
            {
                // Для станции с кодом 43501 Хабаровск код IONKA упращенный
                return strIonka;
            }
            /*
            string tokenGroup04 = arrayString[3];
            int numberControl = Convert.ToInt32(tokenGroup04.Substring(0, 1));
            if (numberControl != 7)
            {
                throw new Exception(String.Format("В коде {0} служебная группа {1} не имеет служебную цифру = 7",
                    strIonka,
                    tokenGroup04));
            }

            if (tokenGroup04.Substring(1, 1) != "/")
            {
                throw new Exception(String.Format("В коде {0} служебная группа {1} не соответствует формату Н/М/К",
                    strIonka,
                    tokenGroup04));
            }


            if (tokenGroup04.Substring(3, 1) != "/")
            {
                new Exception(String.Format("В коде {0} служебная группа {1} не соответствует формату Н/М/К",
                    strIonka,
                    tokenGroup04));
            }
            */
            return strIonka;
        }

        public static int Ionka_Group02_Station(string strIonka)
        {
            string[] arrayString = strIonka.Split(' ');
            string strStation = arrayString[1];
            try
            {
                int numberStation = ParseToken(strStation);
                return numberStation;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в индексе станции: " + strStation, ex);
            }
            return -1;
        }

        public static DateTime Ionka_Group03_DateCreate(string strIonka)
        {
            DateTime dateCreate;
            string[] arrayString = strIonka.Split(' ');
            string token = arrayString[2];
            try
            {
                int month = Convert.ToInt32(token.Substring(1, 2));
                int day = Convert.ToInt32(token.Substring(3, 2));
                int year = DateTime.Now.Year;
                dateCreate = new DateTime(year, month, day);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в дате: " + token, ex);
            }
            return dateCreate;
        }


        public static int Ionka_Group04_Count(string strIonka)
        {
            try
            {
                string token = strIonka;
                int sessionCount = Convert.ToInt32(token.Substring(2, 1));
                return sessionCount;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка вслужебной группе , в количестве сеансов зондирования: " + strIonka, ex);
            }
            return -1;
        }

        public static string Ionka_GroupData_Get(int sessionNumber, string strIonka)
        {
            string stringGroupData = strIonka.Substring(24 + 54 * sessionNumber, 53);
            return stringGroupData;
        }

        public static string Ionka_GroupData_Get_Khabarovsk(int sessionNumber, string strIonka)
        {
            string stringGroupData = strIonka.Substring(24 + 36 * sessionNumber, 35);
            return stringGroupData;
        }

        public static int Ionka_Group05_HH(string strSession)
        {
            try
            {
                string str = strSession.Substring(1, 2);
                int MM = ParseToken(str);
                return MM;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в дате(Время - часы): " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group05_MI(string strSession)
        {
            try
            {
                string str = strSession.Substring(3, 2);
                int DD = ParseToken(str);
                return DD;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в дате(Время минуты): " + strSession, ex);
            }
            return -1;
            
        }
        public static int Ionka_Group06_f0F2(string strSession)
        {
            try
            {
                string str = strSession.Substring(0, 3);
                int f0F2 = ParseToken(str);
                return f0F2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в f0F2 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group06_hF2(string strSession)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hF2 = ParseToken(token);
                return hF2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  hF2 : " + strSession, ex);
            }
            return -1;
            
        }

        public static int Ionka_Group07_M3000F2(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int M3000F2 = ParseToken(token);
                return M3000F2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  M3000F2 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group07_fmin(string strSession)
        {
            string token = strSession.Substring(3, 2);
            int fmin = ParseToken(token);
            return fmin;
        }

        public static int Ionka_Group08_f0Es(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0Es = ParseToken(token);
                return f0Es;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  f0Es : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group08_hEs(string strSession)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hEs = ParseToken(token);
                return hEs;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  hEs : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group09_f0F1(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0F1 = ParseToken(token);
                return f0F1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  f0F1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group09_hF1(string strSession)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hF1 = ParseToken(token);
                return hF1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  hF1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group10_M3000F1(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int M3000F1 = ParseToken(token);
                return M3000F1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  M3000F1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group10_hMF2(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int hMF2 = ParseToken(token);
                return hMF2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  hMF2 : " + strSession, ex);
            }
            return -1;
            
        }

        public static int Ionka_Group11_f0E(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0E = ParseToken(token);
                return f0E;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  f0E : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group11_hE(string strSession)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hE = ParseToken(token);
                return hE;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  hE : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group12_fbEs(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int fbEs = ParseToken(token);
                return fbEs;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  fbEs : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group12_Es(string strSession)
        {
            try
            {
                string token = strSession.Substring(3, 1);
                int Es = ParseToken(token);
                return Es;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  Es : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group13_fx1(string strSession)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int fx1 = ParseToken(token);
                return fx1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка в  fx1 : " + strSession, ex);
            }
            return -1;
        }

        //печатает все возможные комбинации кода ионка
        public static void Print_All_Code_Ionka(string strIonka, List<int> listLengthLines, string pathFile )
        {
            foreach (int len in listLengthLines)
                if (strIonka.Length == len) return;

            listLengthLines.Add(strIonka.Length);

            StreamWriter sw = new StreamWriter(pathFile, true);
            sw.WriteLine(strIonka);
            sw.Close();
        }

        //проверка на группу /ЧЧММ и возврат объекта типа DateTime
        public static bool Find_Time(string strTime, Time time)
        {
            try
            {
                int res;
                if (strTime[0] == '/' && Int32.TryParse(strTime.Substring(1), out res))
                {
                    time.init(Convert.ToInt32(strTime.Substring(1, 2)), Convert.ToInt32(strTime.Substring(3, 2)));
                    if (!time.Check_Format()) return false;
                }
                else return false;

                return true;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Ошибка впоиске временных отрезков /HHMM : " + strTime, ex);
            }
            return true;
        }

        //среднее значения для класса Time
        public static Time Avg_Time( List<Time> listDiff )
        {
            int hh = 0, mi = 0; 
            for( int i = 0; i < listDiff.Count(); i++ )
            {
                hh += listDiff[i].HH.val;
                mi += listDiff[i].MI.val;
            }
            return new Time(hh / listDiff.Count(), mi / listDiff.Count());
        }

        //Алгоритм поиска сеансов зондирования.
        //Если средние значение больше максимального - удалить максимальное значение
        public static void Search_Time_Sess_With_Avg(List<Time> listTimes, List<int> listPositions)
        {
            List<Time> listDiff = new List<Time>();
            for (int i = 0; i < listTimes.Count() - 1; i+=2)
                listDiff.Add(listTimes[i + 1] - listTimes[i]);

            Time avgTime = Avg_Time(listDiff);
            if (avgTime < new Time(3, 0)) avgTime = 2 * avgTime;

            Time maxTimeDiff = new Time();
            int index = 0;

            Search_Max_Time(listDiff, ref maxTimeDiff, ref index);

            if (maxTimeDiff > avgTime)
            {
                listTimes.RemoveAt(index);
                listPositions.RemoveAt(index);
            }
        }

        //Алгоритм поиска сеансов зондирования.
        //Проверяет растояниие между сессиями, если 2 сеанса находяться рядом друг с другом - удалить первый
        public static void Search_Time_Sess_With_Dist(List<Time> listTimes, List<int> listPositions)
        {
            for (int i = 0; i < listPositions.Count() - 1; i++)
            {
                if( listPositions[i+1] - listPositions[i] == 1 )
                {
                    listTimes.RemoveAt(i);
                    listPositions.RemoveAt(i);
                }
            }

            
        }
        /// <summary>
        /// Формирует массивы сеансов зандирония за текущий и пердыдущий день(Опционально)
        /// </summary>
        /// <param name="PrevDay"> Данные за пердыдущий день</param>
        /// <param name="Day"> Данные за текущий день</param>
        /// <param name="listTime"> Время сеансов зондирования</param>
        /// <param name="arrayGroups"> Массив кодов телеграммы</param>
        /// <param name="listPositions"> Позиций кодов времени в массиве телеграммы</param>
 
        public static void List_Time_Session(List<List<string> > PrevDay, List<List<string> > Day, List<Time> listTime, string[] arrayGroups, List<int> listPositions)
        {
            listPositions.Add(arrayGroups.Length-1);
            for (int i = 0; i < listTime.Count(); i++ )
            {
                if (i > 0)
                {
                    if(listTime[i] < listTime[i-1])
                    {
                        foreach(var index in Day)
                        {
                            PrevDay.Add(new List<string>(index));
                        }
                        Day.Clear();
                    }
                }
                List<string> lineListGroup = new List<string>();
                for(int k = listPositions[i]; k < listPositions[i+1]; k++)
                {
                    lineListGroup.Add(arrayGroups[k]);
                }
                Day.Add(lineListGroup);
            }
        }
        public static void Search_Max_Time(List<Time> list, ref Time time, ref int index)
        {
            time.init(list[0]);
            for( int i = 0; i < list.Count(); i++ )
            {
                if( time < list[i] )
                {
                    time = list[i];
                    index = i + 1;
                }
            }
            if (index == 0) index++;
        }
        
        /// <summary>
        ///  Поиск сеансов зондирования
        /// </summary>
        /// <param name="Day">Данные за текущий день</param>
        /// <param name="PrevDay">Данные за предыдущий день</param>
        /// <param name="arrayGroups">Массив кодов телеграммы</param>
        /// <param name="startGroup">Начальное положение временной группы</param>
        public static void Search_Time_Sessions( List<List<string>> Day, List<List<string>> PrevDay, string[] arrayGroups, int startGroup)
        {
            List<Time> listTimes = new List<Time>();
            List<int> listPositions = new List<int>();
            List<string []> listTimeSessions = new List<string []>();
            
            Time currTime = new Time();

            //создает массивы из времени сеансов зондирования и положения этих сеансов в arrayGroups
            for (int i = startGroup; i < arrayGroups.Length - 1; i++)
            {
                if (Find_Time(arrayGroups[i], currTime))
                {
                    listTimes.Add(new Time( currTime ));
                    listPositions.Add(i);
                }
            }
            if (listTimes.Count() > 1)
            {
                //Алгоритм поиска сеансов зондирования.
                //Проверяет растояниие между сессиями, если 2 сеанса находяться рядом друг с другом - удалить первый
                Search_Time_Sess_With_Dist(listTimes, listPositions);

                //Алгоритм поиска сеансов зондирования.
                //Если средние значение больше максимального - удалить максимальное значение 
                Search_Time_Sess_With_Avg(listTimes, listPositions);
            }
            List_Time_Session(PrevDay,Day,listTimes,arrayGroups,listPositions);            
        }
    }
}
