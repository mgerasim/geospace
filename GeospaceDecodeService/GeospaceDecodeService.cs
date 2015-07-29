using GeospaceCore;
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
        

        IDecode theDecode;
        GeospaceCore.ILogger theLogger;
        public GeospaceDecodeService()
        {
            InitializeComponent();
            //logger = LogManager.GetLogger("log");
           // error = LogManager.GetLogger("error");
           // logumagf = LogManager.GetLogger("logumagf");
            //logger.Debug("InitializeComponent");
        }


        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Start");
            //logger.Debug("GeospaceDecodeService: Start");

            AMS.Profile.Ini Ini = new AMS.Profile.Ini(AppDomain.CurrentDomain.BaseDirectory + "\\GeospaceDecodeService.ini");
            if (!Ini.HasSection("COMMON"))
            {
                //  error.Debug("Not HasSection COMMON");
                Ini.SetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");                
            }

            string strFile = Ini.GetValue("COMMON", "FileName", "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt");

            bool bDebug = Ini.GetValue("COMMON", "DEBUG", false);

            strFile = @"\\10.8.5.123\obmen\armgf1dan.txt";
            //strFile = "C:\\Users\\azyryanov\\Desktop\\1\\documents\\armgf1dan.txt";
            //strFile = "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            if (!File.Exists(strFile))
            {
                //eventLog1.WriteEntry("Файл не существует:");
               // eventLog1.WriteEntry(strFile);
               // error.Error("Указанный файл: " + strFile + " не существует");
                return;
            }

            theLogger = new LoggerNLog();
            theDecode = new Decode(theLogger, strFile);

            theDecode.Run();

        }

        protected override void OnStop()
        {
            timer1.Stop();
            eventLog1.WriteEntry("GeospaceDecodeService: Stop");
           // logger.Debug("GeospaceDecodeService: Stop");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

            theDecode.Run();

        }
    }
}
