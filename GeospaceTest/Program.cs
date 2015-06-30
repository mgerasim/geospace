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
            //Support03();
            Support04();
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
            
            
        }
    

           
        
        
        static void Support03()
        {
            GeospaceEntity.Common.NHibernateHelper.UpdateSchema();

        }
       /* static void Support02()
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
            
        }*/
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
