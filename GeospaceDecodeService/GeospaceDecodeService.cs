using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        }
    }
}
