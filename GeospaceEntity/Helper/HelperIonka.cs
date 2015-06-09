﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Helper
{
    public static class HelperIonka
    {
        public static string Normalize(string strIonka)
        {
            string[] delimiters = new string[] { " ","\r\n"};

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
                }
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
            res = Convert.ToInt32(strToken);
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
                return strIonka ;
            }
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

            return strIonka;
        }

        public static int Ionka_Group02_Station(string strIonka)
        {
            string[] arrayString = strIonka.Split(' ');
            string strStation = arrayString[1];
            int numberStation = ParseToken(strStation);
            return numberStation;
        }

        public static DateTime Ionka_Group03_DateCreate(string strIonka)
        {
            string[] arrayString = strIonka.Split(' ');
            string token = arrayString[2];
            int month = Convert.ToInt32(token.Substring(1, 2));
            int day = Convert.ToInt32(token.Substring(3, 2));
            int year = DateTime.Now.Year;
            DateTime dateCreate = new DateTime(year, month, day);
            return dateCreate;
        }

        public static int Ionka_Group04_Count(string strIonka)
        {
            string[] arrayString = strIonka.Split(' ');
            string token = arrayString[3];
            int sessionCount = Convert.ToInt32(token.Substring(2, 1));
            return sessionCount;
        }

        public static string Ionka_GroupData_Get(int sessionNumber, string strIonka)
        {
            string stringGroupData = strIonka.Substring(24 + 54 * sessionNumber, 53);
            return stringGroupData;
        }
        public static int Ionka_Group05_MM(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[0];
            token = token.Substring(1, 2);
            int MM = ParseToken(token);
            return MM;
        }

        public static int Ionka_Group05_DD(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[0];
            token = token.Substring(3, 2);
            int DD = ParseToken(token);
            return DD;
        }

        public static int Ionka_Group06_f0F2(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[1];
            token = token.Substring(0, 3);
            int f0F2 = ParseToken(token);
            
            return f0F2;
        }

        public static int Ionka_Group06_hF2(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[1];
            token = token.Substring(3, 2);
            int hF2 = ParseToken(token);
            
            return hF2;
        }

        public static int Ionka_Group07_M3000F2(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[2];
            token = token.Substring(0, 2);
            int M3000F2 = ParseToken(token);
            return M3000F2;
        }

        public static int Ionka_Group07_fmin(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[2];
            token = token.Substring(3, 2);
            int fmin = ParseToken(token);
            
            return fmin;
        }

        public static int Ionka_Group08_f0Es(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[3];
            token = token.Substring(0, 3);
            int f0Es = ParseToken(token);

            return f0Es;
        }

        public static int Ionka_Group08_hEs(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[3];
            token = token.Substring(3, 2);
            int hEs = ParseToken(token);
            
            return hEs;
        }

        public static int Ionka_Group09_f0F1(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[4];
            token = token.Substring(0, 3);
            int f0F1 = ParseToken(token);
            return f0F1;
        }

        public static int Ionka_Group09_hF1(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[4];
            token = token.Substring(3, 2);
            int hF1 = ParseToken(token);
            return hF1;
        }

        public static int Ionka_Group10_M3000F1(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[5];
            token = token.Substring(0, 2);
            
            int M3000F1 = ParseToken(token);
            return M3000F1;
        }

        public static int Ionka_Group10_hMF2(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[5];
            token = token.Substring(0, 2);
            int hMF2 = ParseToken(token);

            return hMF2;
        }

        public static int Ionka_Group11_f0E(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[6];
            token = token.Substring(0, 3);
            int f0E = ParseToken(token);
            return f0E;
        }

        public static int Ionka_Group11_hE(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[6];
            token = token.Substring(3, 2);
            int hE = ParseToken(token);
            return hE;
        }

        public static int Ionka_Group12_fbEs(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[7];
            token = token.Substring(0, 3);
            int fbEs = ParseToken(token) ;
            return fbEs;
        }



        public static int Ionka_Group12_Es(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[7];
            token = token.Substring(3, 1);
            int Es = ParseToken(token);
            
            return Es;
        }
        public static int Ionka_Group13_fx1(string strSession)
        {
            string[] arrayTokens = strSession.Split(' ');
            string token = arrayTokens[8];
            token = token.Substring(0, 3);
            int fx1=ParseToken(token);
            return fx1;
        }
    }
}