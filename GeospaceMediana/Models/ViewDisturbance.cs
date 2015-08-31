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
            List<int> arrTimeHH = new List<int>();          
            foreach (var item in this.theDisturbanceList.Where(x => x.StationCode == StationCode && x.YYYY == YYYY && x.MM == MM && x.DD == DD).OrderBy(x => x.HH))
            {
                arrTimeHH.Add(item.HH);
                arrTimeHH.Add(item.HH+1);
            }
            List<int> TimeHH = new List<int>();
            for (int i = 0; i < arrTimeHH.Count; i++)
            {
                if (i + 1 < arrTimeHH.Count)
                {
                    if (arrTimeHH[i] != arrTimeHH[i + 1])
                    {
                        TimeHH.Add(arrTimeHH[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                else TimeHH.Add(arrTimeHH[i]);

            }
            int countLine = 22;
            int line = 0;
            for (int i = 0; i < TimeHH.Count; i++)
            {
                string time = "";
                if (TimeHH[i] + 1 != TimeHH[i + 1])
                {
                    time = String.Format("{0:D2}00-{1:D2}00; ", TimeHH[i], TimeHH[i + 1]);
                }
                else
                {
                    time = String.Format("{0:D2}00; ", TimeHH[i]);
                }
                if (time.Length + (res.Length - (countLine + 4) * line) > countLine)
                {
                    line++;
                    res += "<br>";
                }
                res += time;
                i++;

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