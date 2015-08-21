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
        public ViewDisturbanceList()
        {
            theStationList = new List<Station>();
            theDisturbanceList = new List<ViewDisturbance>();
        }
        public int GetCountHH(int StationCode, int YYYY, int MM, int DD)
        {
            return theDisturbanceList.Count(x => x.StationCode == StationCode && x.YYYY==YYYY && x.MM==MM && x.DD==DD);
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