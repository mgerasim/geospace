using System;
using GeospaseCore;
using NLog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCalculation
{
    public partial class GeospaceCalculation : ServiceBase
    {
        ICalculation theCalc;
        public GeospaceCalculation()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("GeospaceDecodeService: Start");
        }

        protected override void OnStop()
        {
            timer1.Stop();
            eventLog1.WriteEntry("GeospaceCalculationService: Stop");
        }
        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {

           // theDecode.Run();

        }
    }
}
