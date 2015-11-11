using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeospaceEntity.Models;

namespace GeospaceMediana.Models
{
    public class ViewHeight
    {
        public int StationCode;
        public DateTime Start;
        public int Limit;
        public int Step;

        public List<GeospaceEntity.Models.Codes.CodeIonka> theIonkaValues;

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

        public ViewHeight(int station, string start, int limit, int step)
        {
            StationCode = station;
            Start = DateTime.ParseExact(start, "yyyyMMdd",
                                       System.Globalization.CultureInfo.InvariantCulture);
            Limit = limit;
            Step = step;

            theIonkaValues = (List<GeospaceEntity.Models.Codes.CodeIonka>)GeospaceEntity.Models.Codes.CodeIonka.GetByPeriod(GeospaceEntity.Models.Station.GetByCode(StationCode),
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
            theList.Add("hE");
            theList.Add("hEs");
            theList.Add("hF1");
            theList.Add("hF2");
            theList.Add("hMF2");
            return theList;
        }

        public GeospaceEntity.Models.Codes.CodeIonka GetValue(int YYYY, int MM, int DD, int HH)
        {
            List<GeospaceEntity.Models.Codes.CodeIonka> result = theIonkaValues.Where(x => x.YYYY==YYYY && x.MM==MM && x.DD==DD && x.HH==HH)
                .OrderBy(x=>x.MI)
                .ToList();

            if (result.Count!=0)
            {
                return result[0];
            }
            return new GeospaceEntity.Models.Codes.CodeIonka();
        }
    }
}