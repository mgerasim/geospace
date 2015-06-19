using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceTest
{
    class Program
    {
        static void Main(string[] args)
        {
           // Support01();
           // Support02();
            Support03();
           // Support04();
           // Support05(); 
            Console.WriteLine("Ok");
            Console.ReadKey();
        }
        static void Support05()
        {
            int StationCode = 43501;
            DateTime Start = DateTime.Now.AddDays(-5);
            int limit = 5;
            List<GeospaceEntity.Models.Codes.CodeIonka> theIonkaValues = (List<GeospaceEntity.Models.Codes.CodeIonka>)(new GeospaceEntity.Models.Codes.CodeIonka()).GetByPeriod((new GeospaceEntity.Models.Station()).GetByCode(StationCode),
                Start.Year, Start.Month, Start.Day,
                Start.AddDays(limit).Year, Start.AddDays(limit).Month, Start.AddDays(limit).Day);

            int Count = theIonkaValues.Count;
        }
        static void Support04()
        {
            string strFile = Environment.CurrentDirectory;
            strFile = "D:\\мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";

            if (File.Exists(strFile))
            {
                Console.WriteLine("Файл существует:");
                Console.WriteLine(strFile);
            }
            else
            {
                Console.WriteLine("Файл не существует:");
                Console.WriteLine(strFile);
            }

            try
            {
                using (StreamReader sr = new StreamReader(strFile))
                {
                    String line = sr.ReadToEnd();
                    string[] delimiters = new string[] { "[ETX]" };
                    foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                 StringSplitOptions.RemoveEmptyEntries))
                    {

                        if (item.IndexOf("ionka 37701 50606 7/1/8/") > -1)
                        {
                            string sss = "";
                            sss = "D";
                        }

                        string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);


                        foreach (var code in theCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (code.Length > 6)
                            {
                                if (code.Substring(0, 5).ToUpper() == "IONKA")
                                {
                                    try
                                    {
                                        string code_source = code;
                                        code_source = GeospaceEntity.Helper.HelperIonka.Check(code);

                                        int StationCode = GeospaceEntity.Helper.HelperIonka.Ionka_Group02_Station(code_source);
                                        Station theStation = (new Station()).GetByCode(StationCode);
                                        if (theStation == null)
                                        {
                                            theStation = new Station();
                                            theStation.Code = StationCode;
                                            theStation.Save();
                                        }

                                        if (StationCode == 43501)
                                        {
                                            // Для Хабаровска код ИОНКА упращенный
                                            string[] arrayString = code_source.Split(' ');
                                            string token = arrayString[2];

                                            code_source = code_source.Replace(token, token + " 0/0/0");

                                        }
                                        Console.WriteLine(code_source);

                                        DateTime Created_At = GeospaceEntity.Helper.HelperIonka.Ionka_Group03_DateCreate(code_source);
                                        int DD = Created_At.Day;
                                        int MM = Created_At.Month;
                                        int YYYY = Created_At.Year;
                                        int sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(code_source);
                                        int hour = Convert.ToInt32(code_source.Split(' ')[4].Substring(1, 2));

                                        int minute = Convert.ToInt32(code_source.Split(' ')[4].Substring(3, 2));

                                        GeospaceEntity.Models.Codes.CodeIonka theTemp = (new GeospaceEntity.Models.Codes.CodeIonka()).GetByDateUTC(theStation, YYYY, MM, DD, hour, minute);
                                        if (theTemp == null)
                                        {
                                            theTemp = new GeospaceEntity.Models.Codes.CodeIonka();
                                            theTemp.DD = Created_At.Day;
                                            theTemp.MM = Created_At.Month;
                                            theTemp.YYYY = Created_At.Year;
                                            theTemp.HH = hour;
                                            theTemp.MI = minute;

                                            theTemp.Station = theStation;

                                            theTemp.Save();
                                        }

                                        for (int i = 0; i < sessionCount; i++)
                                        {
                                            string strSession = GeospaceEntity.Helper.HelperIonka.Ionka_GroupData_Get(i, code_source);
                                            int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(strSession);
                                            int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(strSession);

                                            GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = (new GeospaceEntity.Models.Codes.CodeIonka()).GetByDateUTC(theStation, YYYY, MM, DD, HH, MI);
                                            if (theCodeIonka == null)
                                            {
                                                try
                                                {
                                                    theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka(strSession);
                                                    theCodeIonka.DD = Created_At.Day;
                                                    theCodeIonka.MM = Created_At.Month;
                                                    theCodeIonka.YYYY = Created_At.Year;

                                                    theCodeIonka.Station = theStation;

                                                    theCodeIonka.Save();
                                                }
                                                catch(Exception db)
                                                {
                                                    
                                                    theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka();
                                                    theCodeIonka.ErrorMessage = db.InnerException.Message;
                                                    theCodeIonka.Raw = code_source;
                                                    theCodeIonka.Save();
                                                }
                                                
                                            }
                                            else
                                            {
                                                Console.WriteLine("Ddddddddddddddddddddddddddd");
                                            }
                                            /*
                                            GeospaceEntity.Models.Codes.CodeIonka theIonka = new GeospaceEntity.Models.Codes.CodeIonka(strSession);
                                            theIonka.DD = Created_At.Day;
                                            theIonka.MM = Created_At.Month;
                                            theIonka.YYYY = Created_At.Year;

                                            theIonka.Station = this;

                                            this._IonkaValues.Add(theIonka);*/
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("E R R O R");
                                        Console.WriteLine(code);
                                        Console.WriteLine(ex.Message);
                                        Console.WriteLine(ex.Source);
                                        Console.WriteLine(ex.StackTrace);

                                    }

                                }

                            }

                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        static void Support03()
        {
            GeospaceEntity.Common.NHibernateHelper.UpdateSchema();

        }
        static void Support02()
        {
            string strFile = Environment.CurrentDirectory;
            strFile = "D:\\мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";

            if (File.Exists(strFile))
            {
                Console.WriteLine("Файл существует:");
                Console.WriteLine(strFile);
            }
            else 
            {
                Console.WriteLine("Файл не существует:");
                Console.WriteLine(strFile);
            }

            try
            {
                using (StreamReader sr = new StreamReader(strFile))
                {
                    String line = sr.ReadToEnd();
                    string[] delimiters = new string[] { "[ETX]" };
                    foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                 StringSplitOptions.RemoveEmptyEntries)) {
                                    
                        Console.WriteLine("item");
                        Console.WriteLine(item);
                        string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                        Console.WriteLine(theCode);

                        foreach (var code in theCode.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (code.Length > 6)
                            {
                                if (code.Substring(0, 5).ToUpper() == "IONKA")
                                {
                                    Console.WriteLine("theCode");
                                    Console.WriteLine(code);
                                    Station theStation = new Station();
                                    try 
                                    {
                                        theStation.TryParser(code);
                                        
                                    }
                                    catch (Exception ex)
                                    {   
                                        Console.WriteLine("E R R O R");
                                        Console.WriteLine(code);
                                        Console.WriteLine(ex.Message);
                                        Console.WriteLine(ex.Source);
                                        Console.WriteLine(ex.StackTrace);
                                        
                                    }
                                    
                                }
                           
                            }

                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            
        }
        static void Support01()
        {
            string strIonka = "\"IONKA 46501 50331 7/3/7 /0000 01025 32/19 04225 04520 38284 //100 0343/ //7// /0100 01024 32319 04217 //722 /7285 //102 0383/ //7// /0200 09824 32319 04010 //720 /7290 //100 0373/ //7// \"";
            try
            {
                GeospaceEntity.Models.Station theStation = new GeospaceEntity.Models.Station();
                theStation.TryParser(strIonka);
                theStation.PrintToConsole();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }
    }
}
