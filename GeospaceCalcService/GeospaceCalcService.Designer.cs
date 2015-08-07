namespace GeospaceCalcService
{
    partial class GeospaceCalcService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.timer1 = new System.Timers.Timer();
            this.timer1.Interval = 1000 * 60 * 30;            // 30 мин
            this.timer1.Start();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            // 
            // eventLog1
            // 
            this.eventLog1.EnableRaisingEvents = true;
            this.eventLog1.Log = "Application";
            this.eventLog1.Source = "GeospaceCalcService";
            this.eventLog1.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog1_EntryWritten);
            // 
            // timer1
            // 
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick_1);
            // 
            // GeospaceDecodeService
            // 
            this.ServiceName = "GeospaceCalcService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();

        }

        #endregion

        private System.Timers.Timer timer1;
        private System.Diagnostics.EventLog eventLog1;

    }
}
