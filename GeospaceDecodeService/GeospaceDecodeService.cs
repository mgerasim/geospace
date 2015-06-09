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
        public GeospaceDecodeService()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Start");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Stop");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Timer");
          //  System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();

            string strFile = "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            if (File.Exists(strFile))
            {
                eventLog1.WriteEntry("Файл существует:");
                eventLog1.WriteEntry(strFile);
            }
            else
            {
                eventLog1.WriteEntry("Файл не существует:");
                eventLog1.WriteEntry(strFile);
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

                        string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                        if (theCode != "")
                        {
                            eventLog1.WriteEntry(theCode);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
            }
            eventLog1.WriteEntry(strFile);
        }
    }
}
