using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceCore;

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

           // Support06();
            Support07();
            Console.WriteLine("Ok");
            Console.ReadKey();
        }
        static void Support07()
        {

            string s1 = @"\\10.8.5.123\obmen\armgf1dan.txt";
            string s2 = "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\test.txt";
            ILogger theLogFile = new LoggerNLog();            
            IDecode theDecode = new Decode(theLogFile, s2);

            theDecode.Run();            
        }

        static void Support06()
        {
            string strFile = "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\All_Code_Ionka.txt";
            //strFile = @"\\10.8.5.123\obmen\armgf1dan.txt";
            GeospaceEntity.Common.NHibernateHelper.UpdateSchema();
            using (StreamReader sr = new StreamReader(strFile))
            {
                String line = sr.ReadToEnd();
                string[] delimiters = new string[] { "[ETX]" };
                foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                StringSplitOptions.RemoveEmptyEntries))
                {
                    string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                    foreach (var code in theCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string code_source = code;                        

                        int UmagfYYYY = 0;
                        int UmagfMM = 0;
                        Station UmagfStationFromIonka = new Station();

                        code_source = GeospaceEntity.Helper.HelperIonka.Check(code_source);
                        //GeospaceEntity.Helper.HelperIonka.Print_All_Code_Ionka(code_source, listLengthLines, "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\All_Code_Ionka.txt");

                        int StationCode = GeospaceEntity.Helper.HelperIonka.Ionka_Group02_Station(code_source);
                        Station theStation = Station.GetByCode(StationCode);
                        if (theStation != null)
                        {
                            //logger.Debug(code);
                            UmagfStationFromIonka = theStation;

                            DateTime Created_At = GeospaceEntity.Helper.HelperIonka.Ionka_Group03_DateCreate(code_source);
                            int DD = Created_At.Day;
                            int MM = Created_At.Month;
                            int YYYY = Created_At.Year;
                            UmagfYYYY = YYYY;
                            UmagfMM = MM;
                            string[] arrayGroups = code_source.Split(' ');
                            int sessionCount = 0;
                            int startGroup = 4;
                            if (GeospaceEntity.Helper.HelperIonka.FindSpecialGroup(arrayGroups[3])) //есть ли группа 4
                                sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(arrayGroups[3]);
                            else
                                startGroup = 3;
                            List<List<string>> Day = new List<List<string>>();
                            List<List<string>> PrevDay = new List<List<string>>();
                            GeospaceEntity.Helper.HelperIonka.Search_Time_Sessions(Day, PrevDay, arrayGroups, startGroup );
                        }
                    }
                }
            }
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
