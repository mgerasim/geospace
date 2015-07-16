using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceEntity.Models;
using System.IO;

namespace GeospaceEntity.Helper
{
    public static class Other
    {
        //печатает индексы всех станций
        public static void Print_All_Stations( int statIndex, List<int> listIndex, string pathFile)
        {
            foreach (int index in listIndex)
            {
                if (index == statIndex) return;
            }

            listIndex.Add(statIndex);

            StreamWriter sw = new StreamWriter(pathFile, true);
            sw.WriteLine(statIndex);
            sw.Close();
        }
        public static void Print_Code_Day(String dirName)
        {
            IList<Station> StationList = Station.GetAll();          //Список станций
            DateTime dataPrev = DateTime.Now.AddDays(-1);           //предыдущий день
            DateTime dataNow = DateTime.Now;
            StreamWriter swIonka = new StreamWriter(dirName + "Ionka\\" + dataPrev.ToString("ddMMyy") + ".txt");
            StreamWriter swUmagf = new StreamWriter(dirName + "Umagf\\" + dataPrev.ToString("ddMMyy") + ".txt");
            foreach (var stat in StationList)
            {
                //Вывод Ionka
                IList<CodeIonka> IListIonka = CodeIonka.GetByPeriod(stat, dataPrev.Year, dataPrev.Month, dataPrev.Day,
                    dataNow.Year, dataNow.Month, dataNow.Day);
                foreach(var ionka in IListIonka)
                    ionka.PrintToFile(swIonka);
                //Вывод Umagf
                IList<CodeUmagf> IListUmagf = CodeUmagf.GetByPeriod(stat, dataPrev.Year, dataPrev.Month, dataPrev.Day,
                    dataNow.Year, dataNow.Month, dataNow.Day);
                foreach (var umagf in IListUmagf)
                    umagf.PrintToFile(swUmagf);
            }
            swIonka.Close();
            swUmagf.Close();
        }
    }
}
