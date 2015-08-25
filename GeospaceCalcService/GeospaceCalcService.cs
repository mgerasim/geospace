using GeospaceCore;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
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


namespace GeospaceCalcService
{
    public partial class GeospaceCalcService : ServiceBase
    {
        ILogger theLoggerAverage;
        ILogger theLoggerMediana;

        ICalculation theCalcAverage;
        ICalculation theCalcMediana;

        public GeospaceCalcService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            theLoggerAverage = new LoggerCalc("logAverage", "errorAverage");
            theCalcAverage = new Calculation(theLoggerAverage);
            theCalcAverage.AverageCalc_Run();

            theLoggerMediana = new LoggerCalc("logMediana", "errorMediana");
            theCalcMediana = new Calculation(theLoggerMediana);
            theCalcMediana.MedianaCalc_Run();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //Начало работы
            theCalcAverage.AverageCalc_Run();
            theCalcMediana.MedianaCalc_Run();
            theCalcMediana.CharacterizationCalc();
            theCalcMediana.DisturbanceCalc();
        }
        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

        protected override void OnStop()
        {
            timer1.Stop();
        }
    }
}
