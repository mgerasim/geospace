using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public struct HeaderDay
    {
        public string Name;
        public int YYYY;
        public int MM;
        public int DD;

    }
    public struct HeaderHour
    {
        public string Name;
        public int HH;
    }

    public class ViewIonka
    {
        public int StationCode;
        public DateTime Start;
        public int Limit;
        public int Step;

        protected List<GeospaceEntity.Models.Codes.CodeIonka> theIonkaValues;
        protected List<GeospaceEntity.Models.Codes.CodeUmagf> theUmagfValues;

        public List<GeospaceEntity.Models.Station> Stations
        {
            get
            {
                return GeospaceEntity.Models.Station.GetAll();
            }
        }
        public String StationName
        {
            get
            {
                return GeospaceEntity.Models.Station.GetByCode(this.StationCode).Name;
            }
        }
        public DateTime End
        {
            get 
            { 
                return Start.AddDays(Limit);
            }

        }
        public String DateToString(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
        public ViewIonka(int station, string start, int limit, int step)
        {
            StationCode = station;
            Start = DateTime.ParseExact(start, "yyyyMMdd",
                                       System.Globalization.CultureInfo.InvariantCulture);
            Limit = limit;
            Step = step;

            theIonkaValues = (List<GeospaceEntity.Models.Codes.CodeIonka>)GeospaceEntity.Models.Codes.CodeIonka.GetByPeriod(GeospaceEntity.Models.Station.GetByCode(StationCode),
                Start.Year, Start.Month, Start.Day,
                Start.AddDays(limit).Year, Start.AddDays(limit).Month, Start.AddDays(limit).Day);

            theUmagfValues = (List<GeospaceEntity.Models.Codes.CodeUmagf>)GeospaceEntity.Models.Codes.CodeUmagf.GetByPeriod(GeospaceEntity.Models.Station.GetByCode(StationCode),
                Start.Year, Start.Month, Start.Day,
                Start.AddDays(limit).Year, Start.AddDays(limit).Month, Start.AddDays(limit).Day);
        }

        public List<HeaderDay> GetHeader()
        {
            List<HeaderDay> theList = new List<HeaderDay>();
            for (int i = 0; i < this.Limit; i++)
            {
                HeaderDay Header = new HeaderDay();
                Header.Name = this.Start.AddDays(i).ToString("dd MMMM");
                Header.YYYY = this.Start.AddDays(i).Year;
                Header.MM = this.Start.AddDays(i).Month;
                Header.DD = this.Start.AddDays(i).Day;

                theList.Add(Header);
            }
            return theList;
        }

        public List<HeaderHour> GetHour()
        {
            List<HeaderHour> theList = new List<HeaderHour>();
            for (int i = 0; i < 24; i++)
            {
                HeaderHour Hour = new HeaderHour();
                Hour.Name = i.ToString("D2");
                Hour.HH = i;
                theList.Add(Hour);
            }
            return theList;
        }
        public List<String> GetHeaderValue()
        {
            List<string> theList = new List<string>();
            theList.Add("f0 F2");
            theList.Add("M3000 F2");
            theList.Add("f0 F1");
            theList.Add("M3000 F1");
            theList.Add("f0 Es");
            theList.Add("D");
            theList.Add("f min");
            return theList;
        }

        public GeospaceEntity.Models.Codes.CodeIonka GetValue(int YYYY, int MM, int DD, int HH)
        {
            List<GeospaceEntity.Models.Codes.CodeIonka> result = theIonkaValues.Where(x => x.YYYY==YYYY && x.MM==MM && x.DD==DD && x.HH==HH).ToList();
            if (result.Count!=0)
            {
                return result[0];
            }
            return new GeospaceEntity.Models.Codes.CodeIonka();
        }
        
        public GeospaceEntity.Models.Codes.CodeUmagf GetValueUmagf(int YYYY, int MM, int DD)
        {
            List<GeospaceEntity.Models.Codes.CodeUmagf> result = theUmagfValues.Where(x => x.YYYY == YYYY && x.MM == MM && x.DD == DD).ToList();
            if (result.Count != 0)
            {
                return result[0];
            }
            return new GeospaceEntity.Models.Codes.CodeUmagf();
        }
    }
}