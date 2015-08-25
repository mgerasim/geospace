using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class ViewDisturbanceList
    {
        public List<Station> theStationList;
        public List<ViewDisturbance> theDisturbanceList;
        protected int YYYY;
        protected int MM;
        public ViewDisturbanceList(int YYYY, int MM)
        {
            theStationList = new List<Station>();
            theDisturbanceList = new List<ViewDisturbance>();
            this.YYYY = YYYY;
            this.MM = MM;
        }
        public int GetCountHH(int StationCode, int YYYY, int MM, int DD)
        {
            return theDisturbanceList.Count(x => x.StationCode == StationCode && x.YYYY==YYYY && x.MM==MM && x.DD==DD);
        }
        public string Display(int StationCode, int YYYY, int MM, int DD)
        {
            string res="";
                        
            foreach (var item in this.theDisturbanceList.Where(x => x.StationCode == StationCode && x.YYYY == YYYY && x.MM == MM && x.DD == DD).OrderBy(x => x.HH))
            {
                string time = String.Format("{0:D2}:00-{1:D2}:00; ", item.HH, item.HH + 1);
                res += time;
            }
            return res;
        }

        public string Title {
            get {
                DateTime currDate = new DateTime(YYYY, MM, 1);
                return "Таблица нарушения радиосвязи за " + currDate.ToString("MMMM yyyy");
            }
        }

        public DateTime CurrDate
        {
            get
            {
                return new DateTime(this.YYYY, this.MM, 1);
            }
        }

        
            
    }
    public class ViewDisturbance
    {
        public int YYYY;
        public int MM;
        public int DD;
        public int HH;
        public int MI;
        public int StationCode;
        public string StationName;
        public ViewDisturbance(Disturbance theDisturbance) 
        {
            this.YYYY = theDisturbance.YYYY;
            this.MM = theDisturbance.MM;
            this.DD = theDisturbance.DD;
            this.HH = theDisturbance.HH;
            this.MI = theDisturbance.MI;
            this.StationCode = theDisturbance.Station.Code;
            this.StationName = theDisturbance.Station.Name;
        }

    }
}