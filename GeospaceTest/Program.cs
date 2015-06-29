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
            string strFile = Environment.CurrentDirectory;
            //strFile = "D:\\мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            strFile = @"\\10.8.5.123\obmen\armgf1dan.txt";
            //strFile = "C:\\Users\\azyryanov\\Desktop\\1\\documents\\armgf1dan.txt";
            //strFile = "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            if (!File.Exists(strFile))
            {
                /*
                eventLog1.WriteEntry("Файл не существует:");
                eventLog1.WriteEntry(strFile);
                error.Error("Указанный файл: " + strFile + " не существует");
                return;*/
            }
            //logger.Debug("timer1_Tick_1: FileName:" + strFile);
            try
            {
                GeospaceEntity.Common.NHibernateHelper.UpdateSchema();
                using (StreamReader sr = new StreamReader(strFile))
                {
                    String line = sr.ReadToEnd();
                    string[] delimiters = new string[] { "[ETX]" };
                    foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                 StringSplitOptions.RemoveEmptyEntries))
                    {

                        string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                        int UmagfYYYY = 0;
                        int UmagfMM = 0;
                        Station UmagfStationFromIonka = new Station();
                        bool flagIonka = false;                    //если есть ионка, то брать данные( станция, месяц, год) из ионки, если нет, то из умагф

                        foreach (var code in theCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string code_source = code;
                            int numDate = 1;
                            int numIndex = 2;
                            bool existStatFromBD = true;
                            if (code.Length > 6)
                            {
                                if (code.Substring(0).ToUpper().IndexOf("UMAGF") >= 0)
                                {
                                    try
                                    {
                                        GeospaceEntity.Models.Codes.CodeUmagf theCodeUmagf = new GeospaceEntity.Models.Codes.CodeUmagf();

                                        string[] arrayGroups = code_source.Split(' ');

                                        if (arrayGroups.Length > 6) flagIonka = false;

                                        if (!flagIonka )
                                        {
                                            //без ионки

                                            numDate = 3;
                                            numIndex = 5;
                                            //logumagf.Debug( "1" );
                                            existStatFromBD = GeospaceEntity.Helper.HelperUmagf.Umagf_BigGroup1_NumStation(arrayGroups, 1, theCodeUmagf);
                                            //logumagf.Debug("2");
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_BigGroup2_FullData(arrayGroups, 2, theCodeUmagf);
                                            //logumagf.Debug("3");
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group1_DateCreate(arrayGroups, numDate, theCodeUmagf);
                                            //logumagf.Debug("4");
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group2_AK(arrayGroups, numIndex, theCodeUmagf);
                                            //logumagf.Debug("5");
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group3_K_index(arrayGroups, numIndex, theCodeUmagf);
                                            //logumagf.Debug("6");
                                        }
                                        else //если есть ионка
                                        {
                                            theCodeUmagf.Station = UmagfStationFromIonka;
                                            theCodeUmagf.YYYY = UmagfYYYY;
                                            theCodeUmagf.MM = UmagfMM;
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group1_DateCreate(arrayGroups, numDate, theCodeUmagf, true);
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group2_AK(arrayGroups, numIndex, theCodeUmagf);
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group3_K_index(arrayGroups, numIndex, theCodeUmagf);

                                            flagIonka = false;
                                        }

                                        theCodeUmagf.Raw = code_source;
                                        if (theCodeUmagf.GetByDateUTC() == null && existStatFromBD && theCodeUmagf.Station != null)
                                        {
                                            //theCodeUmagf.Save();
                                            //logumagf.Debug("Save");
                                        }/*
                                        else logumagf.Debug("Not Save");
                                        
                                        if (existStatFromBD)
                                            WriteUmagf(theCodeUmagf);
                                        else
                                            logumagf.Error("\nстанция не найдена в БД: " +
                                                code_source + "\n");*/
                                    }
                                    catch (Exception ex)
                                    {/*
                                        logumagf.Error("\n\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                        logumagf.Error(ex.Message);
                                        logumagf.Error(ex.Source);
                                        logumagf.Error("\ncode_source:");
                                        logumagf.Error(code_source);
                                        logumagf.Error(ex.StackTrace + "\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");*/
                                    }
                                }

                                if (code.Substring(0, 5).ToUpper() == "IONKA")
                                {
                                    flagIonka = true;
                                    //logger.Debug(code);

                                    try
                                    {
                                        code_source = GeospaceEntity.Helper.HelperIonka.Check(code_source);

                                        //GeospaceEntity.Helper.HelperIonka.Print_All_Code_Ionka(code_source, listLengthLines);

                                        int StationCode = GeospaceEntity.Helper.HelperIonka.Ionka_Group02_Station(code_source);

                                        Station theStation = (new Station()).GetByCode(StationCode);
                                        if (theStation == null)
                                        {
                                            theStation = new Station();
                                            theStation.Code = StationCode;
                                            theStation.Save();
                                        }

                                        UmagfStationFromIonka = theStation;

                                        //    // Для Хабарвска код ИОНКА упращенный
                                        //    string[] arrayString = code_source.Split(' ');
                                        //    string token = arrayString[2];

                                        //    int group_count = (arrayString.Count() - 3) / 6;

                                        //    code_source = code_source.Replace(token, token + " 0/" + group_count.ToString()+"/0");
                                        //    logger.Debug("timer1_Tick_1: StationCode: 43501: " + code_source);
                                        //}

                                        DateTime Created_At = GeospaceEntity.Helper.HelperIonka.Ionka_Group03_DateCreate(code_source);
                                        int DD = Created_At.Day;
                                        int MM = Created_At.Month;
                                        int YYYY = Created_At.Year;
                                        string[] arrayGroups = code_source.Split(' ');
                                        int sessionCount = 0;
                                        int startGroup = 4;
                                        if (GeospaceEntity.Helper.HelperIonka.FindSpecialGroup(arrayGroups[3])) //есть ли группа 4
                                            sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(code_source);
                                        else
                                        {
                                            startGroup = 3;
                                            if (GeospaceEntity.Helper.HelperIonka.FindTimePeriod(arrayGroups[3]))
                                            {

                                            }
                                        }

                                        sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(code_source);

                                        for (int i = 0; i < sessionCount; i++)
                                        {
                                            try
                                            {
                                                string strSession;
                                                if (StationCode != 43501)
                                                {
                                                    strSession = GeospaceEntity.Helper.HelperIonka.Ionka_GroupData_Get(i, code_source);
                                                }
                                                else
                                                {
                                                    strSession = GeospaceEntity.Helper.HelperIonka.Ionka_GroupData_Get_Khabarovsk(i, code_source);
                                                }
                                                int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(strSession);
                                                int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(strSession);

                                                GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = (new GeospaceEntity.Models.Codes.CodeIonka()).GetByDateUTC(theStation, YYYY, MM, DD, HH, MI);
                                                if (theCodeIonka == null)
                                                {
                                                    if (StationCode != 43501)
                                                    {
                                                        theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka(strSession);
                                                    }
                                                    else
                                                    {
                                                        theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka();
                                                        theCodeIonka.Parse_Khabarovsk(strSession);
                                                    }
                                                    theCodeIonka.DD = Created_At.Day;
                                                    theCodeIonka.MM = Created_At.Month;
                                                    theCodeIonka.YYYY = Created_At.Year;

                                                    theCodeIonka.Station = theStation;
                                                    theCodeIonka.Raw = code_source;


                                                    theCodeIonka.Save();
                                                }

                                                UmagfYYYY = theCodeIonka.YYYY;
                                                UmagfMM = theCodeIonka.MM;
                                            }
                                            catch (Exception err)
                                            {/*
                                                error.Error("Error:");
                                                error.Error(err.Message);
                                                error.Error(err.StackTrace);*/
                                                if (err.InnerException != null)
                                                {
                                                   // error.Error(err.InnerException.Message);
                                                   // error.Error("Raw: " + code_source);

                                                }
                                                else
                                                {
                                                    //error.Error("InnerException is null");
                                                }

                                                Error theErr;
                                                string Description = err.Message + err.StackTrace;
                                                theErr = (new Error()).GetByDescription(Description);
                                                if (theErr == null)
                                                {
                                                    theErr = new Error();
                                                    theErr.Description = Description;
                                                    theErr.Raw = String.Format("RawMessage\n {0}\n\nRawMessageNormalize\n {1}\n\nIonka\n {2}",
                                                        item.ToString(), code_source, code);
                                                    try
                                                    {
                                                        theErr.Save();

                                                    }
                                                    catch
                                                    {
                                                        //error.Error("Not save to Error obj");
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {/*
                                        error.Error("item:");
                                        error.Error(item);
                                        error.Error("item normalize");
                                        error.Error(theCode);
                                        error.Error(code);
                                        error.Error(ex.Message);
                                        error.Error(ex.Source);
                                        error.Error(ex.StackTrace);

                                        Error theErr;
                                        string Description = ex.Message + ex.StackTrace;
                                        theErr = (new Error()).GetByDescription(Description);
                                        if (theErr != null)
                                        {
                                            theErr.Description = Description;
                                            theErr.Raw = String.Format("RawMessage\n\n {0}RawMessageNormalize\n\n {1}Ionka\n\n {3}",
                                                item, theCode, code);
                                            theErr.Save();
                                        }*/

                                    }

                                }
                            }

                        }// foreach (var code in theCode.Split
                    }

                }
            }
            catch (Exception ex)
            {
                /*
                error.Error("Global Error:");
                error.Error(ex.Message);
                error.Error(ex.StackTrace);*/
            }
        
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
