using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public class Decode:IDecode
    {
        ILogger logger;
        string fileName;
        public Decode(ILogger logger, string fileName)
        {
            this.logger = logger;
            this.fileName = fileName;
        }
        void IDecode.Run()
        {
            try
            {
                logger.LogIonka("Run logger ionka - " + fileName);
                logger.LogUmagf("Run logger umagf");

                logger.LogIonka("timer1_Tick_1: FileName:" + fileName);
                try
                {
                    GeospaceEntity.Common.NHibernateHelper.UpdateSchema();
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        String line = sr.ReadToEnd();
                        string[] delimiters = new string[] { "[ETX]" };
                        foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                     StringSplitOptions.RemoveEmptyEntries))
                        {

                            string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                            int UmagfYYYY = 0;
                            int UmagfMM = 0;
                            int UmagfDD = 0;
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
                                            int posLastGroup_KIndex;

                                            if (arrayGroups.Length > 6) flagIonka = false;

                                            if (!flagIonka) //без ионки
                                            {
                                                numDate = 3;
                                                numIndex = 5;

                                                existStatFromBD = GeospaceEntity.Helper.HelperUmagf.Umagf_BigGroup1_NumStation(arrayGroups, 1, theCodeUmagf);
                                                GeospaceEntity.Helper.HelperUmagf.Umagf_BigGroup2_FullData(arrayGroups, 2, theCodeUmagf);
                                                //GeospaceEntity.Helper.HelperUmagf.Umagf_Group1_DateCreate(arrayGroups, numDate, theCodeUmagf);       
                                                GeospaceEntity.Helper.HelperUmagf.Umagf_Group1_DateCreate(arrayGroups, numDate + 1, theCodeUmagf, true);
                                            }
                                            else //если есть ионка
                                            {
                                                theCodeUmagf.Station = UmagfStationFromIonka;
                                                theCodeUmagf.YYYY = UmagfYYYY;
                                                theCodeUmagf.MM = UmagfMM;
                                                theCodeUmagf.DD = UmagfDD;
                                                GeospaceEntity.Helper.HelperUmagf.Umagf_Group1_DateCreate(arrayGroups, numDate, theCodeUmagf, true);

                                                flagIonka = false;
                                            }

                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Group2_AK(arrayGroups, numIndex, theCodeUmagf);
                                            posLastGroup_KIndex = GeospaceEntity.Helper.HelperUmagf.Umagf_Group3_K_index(arrayGroups, numIndex, theCodeUmagf);
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Events(arrayGroups, posLastGroup_KIndex, theCodeUmagf);
                                            GeospaceEntity.Helper.HelperUmagf.Umagf_Check(theCodeUmagf);
                                            //GeospaceEntity.Helper.HelperUmagf.Print_All_Code_Umagf(code_source, listLengthLines, listComb, "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\All_Code_Umagf.txt");

                                            theCodeUmagf.Raw = code_source;
                                            if (CodeUmagf.GetByDateUTC(theCodeUmagf.Station,
                                                        theCodeUmagf.YYYY,
                                                        theCodeUmagf.MM,
                                                        theCodeUmagf.DD,
                                                        theCodeUmagf.HH,
                                                        theCodeUmagf.MI) == null && existStatFromBD && theCodeUmagf.Station != null)
                                            {
                                                theCodeUmagf.Save();
                                                logger.LogUmagf("Save");
                                            }
                                            else logger.LogUmagf("Not Save");

                                            if (existStatFromBD)
                                                WriteUmagf(theCodeUmagf);
                                            else
                                                logger.LogUmagf("\nстанция не найдена в БД: " + code_source + "\n");
                                        }
                                        catch (System.Exception ex)
                                        {
                                            logger.LogError("\n\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                            logger.LogError(ex.Message);
                                            logger.LogError(ex.Source);
                                            logger.LogError("\ncode_source:");
                                            logger.LogError(code_source);
                                            logger.LogError(ex.StackTrace + "\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");

                                            Error umagfError = new Error();
                                            umagfError.Raw = item;
                                            umagfError.Description = ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace;

                                            if (umagfError.GetByRaw(item) == null) umagfError.Save();
                                        }
                                    }

                                    if (code.Substring(0, 5).ToUpper() == "IONKA")
                                    {
                                        flagIonka = true;

                                        try
                                        {
                                            code_source = GeospaceEntity.Helper.HelperIonka.Check(code_source);
                                            //GeospaceEntity.Helper.HelperIonka.Print_All_Code_Ionka(code_source, listLengthLines, "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\All_Code_Ionka.txt");

                                            int StationCode = GeospaceEntity.Helper.HelperIonka.Ionka_Group02_Station(code_source);
                                            Station theStation = Station.GetByCode(StationCode);
                                            if (theStation != null)
                                            {
                                                logger.LogIonka(code);
                                                UmagfStationFromIonka = theStation;
                                                //    int group_count = (arrayString.Count() - 3) / 6;
                                                //    logger.LogIonka("timer1_Tick_1: StationCode: 43501: " + code_source);
                                                //}
                                                DateTime Created_At = GeospaceEntity.Helper.HelperIonka.Ionka_Group03_DateCreate(code_source);
                                                int DD = Created_At.Day;
                                                int MM = Created_At.Month;
                                                int YYYY = Created_At.Year;
                                                UmagfYYYY = YYYY;
                                                UmagfMM = MM;
                                                UmagfDD = DD;
                                                string[] arrayGroups = code_source.Split(' ');
                                                int sessionCount = 0;
                                                int startGroup = 4;
                                                if (GeospaceEntity.Helper.HelperIonka.FindSpecialGroup(arrayGroups[3])) //есть ли группа 4
                                                    sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(arrayGroups[3]);
                                                else
                                                    startGroup = 3;
                                                List<List<string>> Day = new List<List<string>>();
                                                List<List<string>> PrevDay = new List<List<string>>();
                                                GeospaceEntity.Helper.HelperIonka.Search_Time_Sessions(Day, PrevDay, arrayGroups, startGroup);
                                                //обработка каждой временной группы
                                                foreach (var session in Day)
                                                {
                                                    try
                                                    {
                                                        //List<string> sessionGroup = GeospaceEntity.Helper.HelperIonka.SetListTimeSession(arrayGroups, addressStartSession, i);//создание новой под группы по времени
                                                        int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(session[0]);
                                                        int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(session[0]);
                                                        GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = GeospaceEntity.Models.Codes.CodeIonka.GetByDateUTC(theStation, YYYY, MM, DD, HH, MI);
                                                        if (theCodeIonka == null)
                                                        {
                                                            theCodeIonka = new CodeIonka();
                                                            theCodeIonka.DD = Created_At.Day;
                                                            theCodeIonka.MM = Created_At.Month;
                                                            theCodeIonka.YYYY = Created_At.Year;
                                                            theCodeIonka.Station = theStation;
                                                            theCodeIonka.Raw = code_source;
                                                            theCodeIonka.Decode(session);
                                                            //проверка на отказ записи в будущее))
                                                            DateTime DayNow = DateTime.Now;
                                                            if (DayNow.Hour >= HH || theCodeIonka.DD < DayNow.Day || theCodeIonka.MM < DayNow.Day || theCodeIonka.YYYY < DayNow.Year)
                                                                theCodeIonka.Save();
                                                            else
                                                            {
                                                                //Проверка на существование данных за пердыдущий день
                                                                DateTime IonkaDay = new DateTime(YYYY, MM, DD);
                                                                IonkaDay = IonkaDay.AddDays(-1);
                                                                CodeIonka theCodeIonkaOld = CodeIonka.GetByDateUTC(theStation, IonkaDay.Year, IonkaDay.Month, IonkaDay.Day, HH, MI);
                                                                if (theCodeIonka == null)
                                                                {
                                                                    //Запись за предыдущий день
                                                                    theCodeIonka.DD = IonkaDay.Day;
                                                                    theCodeIonka.MM = IonkaDay.Month;
                                                                    theCodeIonka.YYYY = IonkaDay.Year;
                                                                    theCodeIonka.Station = theStation;
                                                                    theCodeIonka.Raw = code_source;
                                                                    theCodeIonka.Decode(session);
                                                                    theCodeIonka.Save();
                                                                }
                                                                else
                                                                {
                                                                    //Запись в ошибки
                                                                    Error IonkaError = new Error();
                                                                    IonkaError.Description = "Ошибка в дате";
                                                                    IonkaError.Raw = item;
                                                                    if (IonkaError.GetByRaw(item) == null)
                                                                        IonkaError.Save();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (theCodeIonka.Raw != code_source)
                                                            {
                                                                Error IonkaError = new Error();
                                                                IonkaError.Description = "Повторная телеграмма";
                                                                IonkaError.Raw = item;
                                                                if (IonkaError.GetByRaw(item) == null)
                                                                    IonkaError.Save();
                                                            }
                                                        }
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        logger.LogError(code);
                                                        logger.LogError("Error:");
                                                        logger.LogError(err.Message);
                                                        logger.LogError(err.StackTrace);
                                                        if (err.InnerException != null)
                                                        {
                                                            logger.LogError(err.InnerException.Message);
                                                            logger.LogError("Raw: " + code_source);
                                                        }
                                                        else
                                                        {
                                                            logger.LogError("InnerException is null");
                                                        }
                                                        Error theErr = new Error();
                                                        string Description = err.Message + err.StackTrace;
                                                        theErr.Description = Description;
                                                        if (theErr.GetByRaw(item) == null)
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
                                                                logger.LogError("Not save to Error obj");
                                                            }
                                                        }
                                                        Error IonkaError = new Error();
                                                        IonkaError.Description = err.Message + "\n" + err.InnerException + "\n" + err.Source + "\n" + err.StackTrace;
                                                        IonkaError.Raw = theCode;
                                                        if (IonkaError.GetByRaw(theCode) == null)
                                                            IonkaError.Save();

                                                    }
                                                }
                                                foreach (var session in PrevDay)
                                                {
                                                    try
                                                    {
                                                        //List<string> sessionGroup = GeospaceEntity.Helper.HelperIonka.SetListTimeSession(arrayGroups, addressStartSession, i);//создание новой под группы по времени
                                                        int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(session[0]);
                                                        int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(session[0]);
                                                        DateTime PrevDay_At = Created_At.AddDays(-1);
                                                        GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = GeospaceEntity.Models.Codes.CodeIonka.GetByDateUTC(theStation, PrevDay_At.Year, PrevDay_At.Month, PrevDay_At.Day, HH, MI);
                                                        if (theCodeIonka == null)
                                                        {
                                                            theCodeIonka = new CodeIonka();
                                                            theCodeIonka.DD = PrevDay_At.Day;
                                                            theCodeIonka.MM = PrevDay_At.Month;
                                                            theCodeIonka.YYYY = PrevDay_At.Year;
                                                            theCodeIonka.Station = theStation;
                                                            theCodeIonka.Raw = code_source;
                                                            theCodeIonka.Decode(session);
                                                            theCodeIonka.Save();
                                                        }
                                                        else
                                                        {
                                                            if (theCodeIonka.Raw != code_source)
                                                            {
                                                                Error IonkaError = new Error();
                                                                IonkaError.Description = "Повторная телеграмма";
                                                                IonkaError.Raw = item;
                                                                if (IonkaError.GetByRaw(item) == null)
                                                                    IonkaError.Save();
                                                            }
                                                        }
                                                    }
                                                    catch (Exception err)
                                                    {
                                                        logger.LogError(code);
                                                        logger.LogError("Error:");
                                                        logger.LogError(err.Message);
                                                        logger.LogError(err.StackTrace);
                                                        if (err.InnerException != null)
                                                        {
                                                            logger.LogError(err.InnerException.Message);
                                                            logger.LogError("Raw: " + code_source);
                                                        }
                                                        else
                                                        {
                                                            logger.LogError("InnerException is null");
                                                        }
                                                        Error theErr = new Error();
                                                        string Description = err.Message + err.StackTrace;
                                                        theErr.Description = Description;
                                                        if (theErr.GetByDescription() == null)
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
                                                                logger.LogError("Not save to Error obj");
                                                            }
                                                        }
                                                        Error IonkaError = new Error();
                                                        IonkaError.Description = err.Message + "\n" + err.InnerException + "\n" + err.Source + "\n" + err.StackTrace;
                                                        IonkaError.Raw = theCode;
                                                        if (IonkaError.Description == null)
                                                            IonkaError.Save();

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.LogError("item:");
                                            logger.LogError(item);
                                            logger.LogError("item normalize");
                                            logger.LogError(theCode);
                                            logger.LogError(code);
                                            logger.LogError(ex.Message);
                                            logger.LogError(ex.Source);
                                            logger.LogError(ex.StackTrace);
                                            Error theErr = new Error();
                                            string Description = ex.Message + ex.StackTrace;
                                            if (theErr.GetByDescription() != null)
                                            {
                                                theErr.Description = Description;
                                                theErr.Raw = String.Format("RawMessage\n\n {0}RawMessageNormalize\n\n {1}Ionka\n\n {3}",
                                                    item, theCode, code);
                                                theErr.Save();
                                            }
                                            Error IonkaError = new Error();
                                            IonkaError. Description = ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace;
                                            IonkaError.Raw = theCode;
                                            if (IonkaError.Description == null)
                                                IonkaError.Save();
                                        }
                                    }
                                }

                            }// foreach (var code in theCode.Split

                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Global Error:");
                    logger.LogError(ex.Message);
                    logger.LogError(ex.StackTrace);
                }
                
            }
            catch
            {
                logger.LogError("Error Global");
            }
        }

        public void WriteUmagf(GeospaceEntity.Models.Codes.CodeUmagf umagf)
        {
            logger.LogUmagf(umagf.Raw);
            logger.LogUmagf("SS " + umagf.Station.Code.ToString() + " - " + umagf.Station.Name);
            logger.LogUmagf(umagf.HH.ToString() + ":" + umagf.MI.ToString() + "  " + umagf.DD.ToString() + "." + umagf.MM.ToString() + "." + umagf.YYYY.ToString());
            logger.LogUmagf("AK" + " = " + umagf.ak);
            logger.LogUmagf("k1 = " + umagf.k1.ToString());
            logger.LogUmagf("k2 = " + umagf.k2.ToString());
            logger.LogUmagf("k3 = " + umagf.k3.ToString());
            logger.LogUmagf("k4 = " + umagf.k4.ToString());
            logger.LogUmagf("k5 = " + umagf.k5.ToString());
            logger.LogUmagf("k6 = " + umagf.k6.ToString());
            logger.LogUmagf("k7 = " + umagf.k7.ToString());
            logger.LogUmagf("k8 = " + umagf.k8.ToString());
            logger.LogUmagf("events = " + umagf.events);
            logger.LogUmagf("+++++++++++++++++++++++++++++++++++");
        }
    }
}
