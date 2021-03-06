﻿using GeospaceEntity.Models.Codes;
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


        public static string Normalize(string strIonka, List<string> listBegin)
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

            //if (strIonka.IndexOf("7/1/ ") > -1)
            //{
            //    strIonka = strIonka.Replace("7/1/ ", "7/1/");
            //}
            string[] delimiters = new string[] { " ", "\r\n" };

            // strIonka = strIonka.Replace("///", "/ //");
            string[] strCodes = strIonka.Split(delimiters,
                                 StringSplitOptions.None);
            if (strCodes.Count() == 1)
            {
                return "";
            }
            string strIonkaNormalize = "";
            string prevItem = "";
            string prevPrevItem = "";
            bool firstTime = false, secondTime = false, words = false, thirdTime = false;
            string message = "";
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
                //определяет сообщения о сбоях (нет эл.) после нахождения времени (/ЧЧМИ)
                if (!thirdTime && !words)
                    message = "";

                if (words)
                    if (Find_Time(ss) || ss.Substring(0).ToUpper().IndexOf("UMAGF") > -1)
                        thirdTime = true;
                    else message += ss + " ";

                if ((secondTime || firstTime) && !words)
                {
                    //words = true;
                    for (int k = 0; k < ss.Length; k++)
                    {
                        if (ss[k] > 61)
                            words = true;
                    }

                    if (ss.Substring(0).ToUpper().IndexOf("UMAGF") > -1) thirdTime = true;
                    else
                        if (words)
                            message += ss + " ";

                    if (!words && firstTime && secondTime)
                    {
                        firstTime = false;
                        secondTime = false;
                    }
                    //else message += ss + "|";  

                }

                if (firstTime && !secondTime && !thirdTime)
                {
                    if (Find_Time(ss))
                    {
                        secondTime = true;
                        prevItem = ss;
                    }
                    else
                        if (!words) firstTime = false;

                }

                if (Find_Time(ss) && !firstTime)
                {
                    firstTime = true;
                    prevPrevItem = ss;
                }

                if (ss.Length > 5)
                {
                    //string sss = ss.Replace(" ", "");
                    int pos = ss.IndexOf("-");
                    if (pos > 0)
                    {
                        string s1 = ss.Substring(0, pos);
                        string s2 = ss.Substring(pos + 1);

                        if (s1.Length == 5 && s2.Length == 5)
                        {
                            firstTime = true;
                            secondTime = true;
                            prevItem = s2;
                            prevPrevItem = s1;
                            strIonkaNormalize += prevPrevItem + " " + prevItem + " ";
                        }
                    }
                }

                if (firstTime && words && thirdTime)
                {
                    int m = 6;
                    if (secondTime) m += 6;
                    strIonkaNormalize = strIonkaNormalize.Remove(strIonkaNormalize.Length - m);
                    Time startTime = new Time();
                    Time endTime = new Time();

                    Find_Time(prevPrevItem, startTime);
                    Find_Time(prevItem, endTime);
                    
                    if (secondTime)
                        strIonkaNormalize += Set_All_Value(startTime, endTime, message, strIonkaNormalize.Length - 3);
                    else
                        strIonkaNormalize += Set_All_Value(startTime, startTime, message, strIonkaNormalize.Length + 3);

                    if (ss.Substring(0).ToUpper().IndexOf("UMAGF") > -1) firstTime = false;
                    else prevPrevItem = ss;
                    secondTime = false;
                    words = false;
                    thirdTime = false;
                }
                //Сдвоенные группы
                 if (ss.Length == 10)
                 {
                     bool prov = true;
                     for (int k = 0; k < ss.Length; k++)
                     {
                         if (ss[k] > 61)
                             prov = false;
                     }
                     if (prov)
                     {
                         strIonkaNormalize += ss.Substring(0, 5);
                         strIonkaNormalize += " ";
                         strIonkaNormalize += ss.Substring(5, 5);
                         strIonkaNormalize += " ";
                     }
                 }

                if (ss.Length == 5)
                {
                    bool final = false;
                    foreach( string begin in listBegin)
                    {
                        if( begin == ss.ToUpper() )
                        {
                            final = true;
                            break;
                        }
                    }
                    if (final) strIonkaNormalize += "\r\n" + ss + " ";
                    
                    bool notWords = true;
                    for (int k = 0; k < ss.Length; k++)
                    {
                        if (ss[k] > 61)
                            notWords = false;
                    }
                    if (notWords)
                    {
                        strIonkaNormalize += ss;
                        strIonkaNormalize += " ";
                    }
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

        public static string Set_All_Value( Time start, Time end, string message, int startPos )
        {
            string s = "";

            int k = start.HH.val;
            int finish;
            if (start.HH.val <= end.HH.val) finish = end.HH.val - start.HH.val;
            else finish = 24 - start.HH.val + end.HH.val;

            for (int h = 0; h <= finish; h++)
            {
                string ss = k.ToString();
                if (k < 10) ss = '0' + k.ToString();
                s += "/" + ss + "00 " + "//3/3 /3//3 //3/3 //3/3 /3//3 //3/3 //3// //3/3 " + message + " "; // ;

                k++;
                if( k == 24 ) k = 0;
            }

            string posSrart = (startPos - 5).ToString();

            string hours = "/";
            if (start.HH.val < 10) hours += '0' + start.HH.val.ToString();
            else hours += start.HH.val.ToString();
            hours += "00 ";

            if( start != end )
            { 
                hours += "- /";
                if (end.HH.val < 10) hours += '0' + end.HH.val.ToString();
                else hours += end.HH.val.ToString();
                hours += "00 ";
            }

            string lenghtMessage = (hours.Length + message.Length).ToString();

            string posEnd = ((s + "\0 alert 00000 00000 00000 " + hours + message + " \0").Length).ToString();

            while (lenghtMessage.Length != 5)
                lenghtMessage = "0" + lenghtMessage;

            while (posSrart.Length != 5)
                posSrart = "0" + posSrart;

            while (posEnd.Length != 5)
                posEnd = "0" + posEnd;

            string special = "\0 alert " + posSrart + " " + posEnd + " " + lenghtMessage + " " + hours + message + " \0";

            return s;// +special;
        }

        public static string Gen_Error_Message( CodeIonka ci )
        {
            return ci.Station.Code.ToString() + " "
                    + ci.Station.Name + " "
                    + ci.DD + "." + ci.MM + "." + ci.YYYY + " "
                    + ci.HH + ":" + ci.MI + ". ";
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
                throw new System.Exception( "Ошибка в дате(Время минуты): " + strSession, ex);
            }
            return -1;
            
        }
        public static int Ionka_Group06_f0F2(string strSession, CodeIonka ci)
        {
            try
            {
                string str = strSession.Substring(0, 3);
                int f0F2 = ParseToken(str);
                return f0F2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в f0F2 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group06_hF2(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hF2 = ParseToken(token);
                return hF2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  hF2 : " + strSession, ex);
            }
            return -1;
            
        }

        public static int Ionka_Group07_M3000F2(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int M3000F2 = ParseToken(token);
                return M3000F2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  M3000F2 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group07_fmin(string strSession, CodeIonka ci)
        {
            int fmin = 0;
            try
            {
                string token = strSession.Substring(3, 2);
                fmin = ParseToken(token);
                
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  fmin : " + strSession, ex);
            }
            return fmin;
        }

        public static int Ionka_Group07_diffusio(string strSession, CodeIonka ci )
        {
            int diffusio = 0;
            try
            {
                string token = strSession.Substring(2, 1);
                diffusio = ParseToken(token);

                if (diffusio == 1000) diffusio = 0;

                //if (diffusio == 6 && ci.f0F2 == 1006 && ci.M3000F2 == 1006) diffusio = 3;
                //if (diffusio == 6 && ci.f0F2 != 1006 && ci.M3000F2 != 1006) diffusio = 1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception( Gen_Error_Message(ci) + "Ошибка в  diffusio : " + strSession, ex);
            }
            return diffusio;
        }

        public static int Ionka_Group08_f0Es(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0Es = ParseToken(token);
                return f0Es;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  f0Es : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group08_hEs(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hEs = ParseToken(token);
                return hEs;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  hEs : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group09_f0F1(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0F1 = ParseToken(token);
                return f0F1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  f0F1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group09_hF1(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hF1 = ParseToken(token);
                return hF1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  hF1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group10_M3000F1(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int M3000F1 = ParseToken(token);
                return M3000F1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  M3000F1 : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group10_hMF2(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 2);
                int hMF2 = ParseToken(token);
                return hMF2;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  hMF2 : " + strSession, ex);
            }
            return -1;
            
        }

        public static int Ionka_Group11_f0E(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int f0E = ParseToken(token);
                return f0E;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  f0E : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group11_hE(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(3, 2);
                int hE = ParseToken(token);
                return hE;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  hE : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group12_fbEs(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int fbEs = ParseToken(token);
                return fbEs;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  fbEs : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group12_Es(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(3, 1);
                int Es = ParseToken(token);
                return Es;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  Es : " + strSession, ex);
            }
            return -1;
        }

        public static int Ionka_Group13_fx1(string strSession, CodeIonka ci)
        {
            try
            {
                string token = strSession.Substring(0, 3);
                int fx1 = ParseToken(token);
                return fx1;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(Gen_Error_Message(ci) + "Ошибка в  fx1 : " + strSession, ex);
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

        //проверка на группу /ЧЧММ и возврат объекта типа Time
        public static bool Find_Time(string strTime, Time time = null)
        {
            bool a = true;
            try
            {
                if (strTime.Length == 0) a = false;
                int res;
                if (strTime[0] == '/' && Int32.TryParse(strTime.Substring(1), out res) && strTime.Length == 5)
                {
                    if (time != null)
                    {
                        time.init(Convert.ToInt32(strTime.Substring(1, 2)), Convert.ToInt32(strTime.Substring(3, 2)));
                        if (!time.Check_Format()) a = false;

                        if (time.MI.val % 15 != 0) a = false;
                    }
                }
                else a = false;
            }
            catch (System.Exception ex)
            {
                //throw new System.Exception("Ошибка в поиске временных отрезков /HHMM : " + strTime, ex);
            }
            return a;
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

            if (maxTimeDiff >= avgTime)
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
