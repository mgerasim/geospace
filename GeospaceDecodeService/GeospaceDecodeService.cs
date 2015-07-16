using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceDecodeService
{
    public partial class GeospaceDecodeService : ServiceBase
    {
        Logger logger;
        Logger error;
        Logger logumagf;
        public GeospaceDecodeService()
        {
            InitializeComponent();
            logger = LogManager.GetLogger("log");
            error = LogManager.GetLogger("error");
            logumagf = LogManager.GetLogger("logumagf");
            logger.Debug("InitializeComponent");
        }


        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Start");
            logger.Debug("GeospaceDecodeService: Start");
            timer1_Tick_1(null, null);
        }

        protected override void OnStop()
        {
            timer1.Stop();
            eventLog1.WriteEntry("GeospaceDecodeService: Stop");
            logger.Debug("GeospaceDecodeService: Stop");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
        public void WriteUmagf(GeospaceEntity.Models.Codes.CodeUmagf umagf)
        {
            logumagf.Debug(umagf.Raw);
            logumagf.Debug("SS " + umagf.Station.Code.ToString() + " - " + umagf.Station.Name);
            logumagf.Debug(umagf.HH.ToString() + ":" + umagf.MI.ToString() + "  " + umagf.DD.ToString() + "." + umagf.MM.ToString() + "." + umagf.YYYY.ToString());
            logumagf.Debug("AK" + " = " + umagf.ak);
            logumagf.Debug("k1 = " + umagf.k1.ToString());
            logumagf.Debug("k2 = " + umagf.k2.ToString());
            logumagf.Debug("k3 = " + umagf.k3.ToString());
            logumagf.Debug("k4 = " + umagf.k4.ToString());
            logumagf.Debug("k5 = " + umagf.k5.ToString());
            logumagf.Debug("k6 = " + umagf.k6.ToString());
            logumagf.Debug("k7 = " + umagf.k7.ToString());
            logumagf.Debug("k8 = " + umagf.k8.ToString());
            logumagf.Debug("events = " + umagf.events);
            logumagf.Debug("+++++++++++++++++++++++++++++++++++");
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Timer");
            logger.Debug("timer1_Tick_1: Enter");
            logger.Debug("timer1_Tick_1: AppDir:" + AppDomain.CurrentDomain.BaseDirectory);

            List<int> listLengthLines = new List<int>();
            List<string> listComb = new List<string>();

            AMS.Profile.Ini Ini = new AMS.Profile.Ini(AppDomain.CurrentDomain.BaseDirectory + "\\GeospaceDecodeService.ini");
            if (!Ini.HasSection("COMMON"))
            {
                error.Debug("Not HasSection COMMON");
                Ini.SetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");
            }

            string strFile = Ini.GetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");

            strFile = @"\\10.8.5.123\obmen\armgf1dan.txt";
            //strFile = "C:\\Users\\azyryanov\\Desktop\\1\\documents\\armgf1dan.txt";
            //strFile = "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            if (!File.Exists(strFile))
            {
                eventLog1.WriteEntry("Файл не существует:");
                eventLog1.WriteEntry(strFile);
                error.Error("Указанный файл: " + strFile + " не существует");
                return;
            }
            logger.Debug("timer1_Tick_1: FileName:" + strFile);
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

                                        theCodeUmagf.Raw = item;
                                        if (CodeUmagf.GetByDateUTC( theCodeUmagf.Station, theCodeUmagf.YYYY, theCodeUmagf.MM,
                                            theCodeUmagf.DD, theCodeUmagf.HH, theCodeUmagf.MI) == null && existStatFromBD && theCodeUmagf.Station != null)
                                        {
                                            theCodeUmagf.Save();
                                            logumagf.Debug("Save");
                                        }
                                        else logumagf.Debug("Not Save");
                                        
                                        if (existStatFromBD)
                                            WriteUmagf(theCodeUmagf);
                                        else
                                            logumagf.Debug("\nстанция не найдена в БД: " + code_source + "\n");
                                    }
                                    catch ( System.Exception ex)
                                    {
                                        logumagf.Error("\n\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                        logumagf.Error(ex.Message);
                                        logumagf.Error(ex.Source);
                                        logumagf.Error("\ncode_source:");
                                        logumagf.Error(code_source);
                                        logumagf.Error(ex.StackTrace + "\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");

                                        CodeUmagfError umagfError = new CodeUmagfError();
                                        umagfError.Raw = item;
                                        umagfError.ErrorMessage = ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace;

                                        if (umagfError.GetByRaw() == null) umagfError.Save();
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
                                            logger.Debug(code);
                                            UmagfStationFromIonka = theStation;
                                        //    int group_count = (arrayString.Count() - 3) / 6;
                                        //    logger.Debug("timer1_Tick_1: StationCode: 43501: " + code_source);
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
                                                    GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = CodeIonka.GetByDateUTC(theStation, YYYY, MM, DD, HH, MI);
                                                    if (theCodeIonka == null)
                                                    {
                                                        theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka(session);
                                                        theCodeIonka.DD = Created_At.Day;
                                                        theCodeIonka.MM = Created_At.Month;
                                                        theCodeIonka.YYYY = Created_At.Year;
                                                        theCodeIonka.Station = theStation;
                                                        theCodeIonka.Raw = code_source;
                                                        theCodeIonka.Save();
                                                    }
                                                }
                                                catch (Exception err)
                                                {
                                                    error.Error(code);
                                                    error.Error("Error:");
                                                    error.Error(err.Message);
                                                    error.Error(err.StackTrace);
                                                    if (err.InnerException != null)
                                                    {
                                                        error.Error(err.InnerException.Message);
                                                        error.Error("Raw: " + code_source);
                                                    }
                                                    else
                                                    {
                                                        error.Error("InnerException is null");
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
                                                            error.Error("Not save to Error obj");
                                                        }
                                                    }
                                                    CodeIonkaError IonkaError = new CodeIonkaError();
                                                    IonkaError.ErrorMessage = err.Message + "\n" + err.InnerException + "\n" + err.Source + "\n" + err.StackTrace;
                                                    IonkaError.Raw = theCode;
                                                    if (IonkaError.GetByRaw() == null)
                                                        IonkaError.Save();

                                                }
                                            }
                                            foreach(var session in PrevDay)
                                            {
                                                try
                                                {
                                                    //List<string> sessionGroup = GeospaceEntity.Helper.HelperIonka.SetListTimeSession(arrayGroups, addressStartSession, i);//создание новой под группы по времени
                                                    int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(session[0]);
                                                    int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(session[0]);
                                                    DateTime PrevDay_At = Created_At.AddDays(-1);
                                                    GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = CodeIonka.GetByDateUTC(theStation, PrevDay_At.Year, PrevDay_At.Month, PrevDay_At.Day, HH, MI);
                                                    if (theCodeIonka == null)
                                                    {
                                                        theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka(session);
                                                        theCodeIonka.DD = PrevDay_At.Day;
                                                        theCodeIonka.MM = PrevDay_At.Month;
                                                        theCodeIonka.YYYY = PrevDay_At.Year;
                                                        theCodeIonka.Station = theStation;
                                                        theCodeIonka.Raw = code_source;
                                                        theCodeIonka.Save();
                                                    }
                                                }
                                                catch (Exception err)
                                                {
                                                    error.Error(code);
                                                    error.Error("Error:");
                                                    error.Error(err.Message);
                                                    error.Error(err.StackTrace);
                                                    if (err.InnerException != null)
                                                    {
                                                        error.Error(err.InnerException.Message);
                                                        error.Error("Raw: " + code_source);
                                                    }
                                                    else
                                                    {
                                                        error.Error("InnerException is null");
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
                                                            error.Error("Not save to Error obj");
                                                        }
                                                    }
                                                    CodeIonkaError IonkaError = new CodeIonkaError();
                                                    IonkaError.ErrorMessage = err.Message + "\n" + err.InnerException + "\n" + err.Source + "\n" + err.StackTrace;
                                                    IonkaError.Raw = theCode;
                                                    if (IonkaError.GetByRaw() == null)
                                                        IonkaError.Save();

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
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
                                        }
                                        CodeIonkaError IonkaError = new CodeIonkaError();
                                        IonkaError.ErrorMessage = ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace;
                                        IonkaError.Raw = theCode;
                                        if(IonkaError.GetByRaw() == null)
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
                error.Error("Global Error:");
                error.Error(ex.Message);
                error.Error(ex.StackTrace);
            }
        }
    }
}
