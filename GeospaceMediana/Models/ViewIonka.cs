using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class ViewIonka
    {
        public int StationCode;
        public string StationName;
        public DateTime Start;
        public int Limit;
        public int Step;

        public ViewIonka(int station, string start, int limit, int step)
        {
            StationCode = station;
            StationName = "Хабаровск";
            Start = DateTime.Now.AddDays(-1);
            Limit = limit;
            Step = step;
        }

        public List<String> GetHeader()
        {
            List<String> theList = new List<string>();
            for (int i = 0; i < this.Limit; i++)
            {
                string Header = this.Start.AddDays(i).ToString("dd MMMM");
                theList.Add(Header);
            }
            return theList;
        }

        public List<String> GetHour()
        {
            List<String> theList = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                string Hour = i.ToString("D2");
                theList.Add(Hour);
            }
            return theList;
        }
    }
}