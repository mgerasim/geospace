using GeospaceEntity.Models;
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
        public GeospaceDecodeService()
        {
            InitializeComponent();
            logger = LogManager.GetLogger("log");
            error = LogManager.GetLogger("error");
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


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Timer");
            logger.Debug("timer1_Tick_1: Enter");
            logger.Debug("timer1_Tick_1: AppDir:" + AppDomain.CurrentDomain.BaseDirectory);

            AMS.Profile.Ini Ini = new AMS.Profile.Ini(AppDomain.CurrentDomain.BaseDirectory + "\\GeospaceDecodeService.ini");
            if (!Ini.HasSection("COMMON"))
            {
                error.Debug("Not HasSection COMMON");
                Ini.SetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");
            }

            string strFile = Ini.GetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");

            strFile = @"\\10.8.5.123\obmen\armgf1dan.txt";

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

                        foreach (var code in theCode.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (code.Length > 6)
                            {
                                if (code.Substring(0, 5).ToUpper() == "IONKA")
                                {
                                    logger.Debug(code);
                                    
                                    try
                                    {
                                        string code_source = code;
                                        code_source = GeospaceEntity.Helper.HelperIonka.Check(code);

                                        int StationCode = GeospaceEntity.Helper.HelperIonka.Ionka_Group02_Station(code_source);

                                        Station theStation = (new Station()).GetByCode(StationCode);
                                        if (theStation==null)
                                        {
                                            theStation = new Station();
                                            theStation.Code = StationCode;
                                            theStation.Save();
                                        }

                                        if (StationCode == 43501)
                                        {
                                            logger.Debug("timer1_Tick_1: StationCode: 43501: " + code_source);
                                            // Для Хабарвска код ИОНКА упращенный
                                            string[] arrayString = code_source.Split(' ');
                                            string token = arrayString[2];

                                            code_source = code_source.Replace(token, token + " 0/0/0");
                                            logger.Debug("timer1_Tick_1: StationCode: 43501: " + code_source);
                                        }

                                        DateTime Created_At = GeospaceEntity.Helper.HelperIonka.Ionka_Group03_DateCreate(code_source);
                                        int DD = Created_At.Day;
                                        int MM = Created_At.Month;
                                        int YYYY = Created_At.Year;
                                        int sessionCount = GeospaceEntity.Helper.HelperIonka.Ionka_Group04_Count(code_source);

                                        for (int i = 0; i < sessionCount; i++)
                                        {
                                            try
                                            {

                                                string strSession = GeospaceEntity.Helper.HelperIonka.Ionka_GroupData_Get(i, code_source);
                                                int HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(strSession);
                                                int MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(strSession);

                                                GeospaceEntity.Models.Codes.CodeIonka theCodeIonka = (new GeospaceEntity.Models.Codes.CodeIonka()).GetByDateUTC(theStation, YYYY, MM, DD, HH, MI);
                                                if (theCodeIonka == null)
                                                {
                                                    theCodeIonka = new GeospaceEntity.Models.Codes.CodeIonka(strSession);
                                                    theCodeIonka.DD = Created_At.Day;
                                                    theCodeIonka.MM = Created_At.Month;
                                                    theCodeIonka.YYYY = Created_At.Year;

                                                    theCodeIonka.Station = theStation;
                                                    theCodeIonka.Raw = theCode;

                                                    theCodeIonka.Save();
                                                }
                                            }
                                            catch(Exception err)
                                            {
                                                error.Error("Error:");
                                                error.Error(err.Message);
                                                error.Error(err.StackTrace);

                                                Error theErr;
                                                string Description = err.Message + err.StackTrace;
                                                theErr = (new Error()).GetByDescription(Description);
                                                if (theErr == null)
                                                {
                                                    theErr = new Error();
                                                    theErr.Description = Description;
                                                    theErr.Raw = String.Format("RawMessage\n {0}\n\nRawMessageNormalize\n {1}\n\nIonka\n {2}",
                                                        item, theCode, code);
                                                    theErr.Save();
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
                                        if (theErr == null)
                                        {
                                            theErr.Description = Description;
                                            theErr.Raw = String.Format("RawMessage\n\n {0}RawMessageNormalize\n\n {1}Ionka\n\n {3}", 
                                                item, theCode, code);
                                            theErr.Save();
                                        }

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
            }
        }
    }
}
