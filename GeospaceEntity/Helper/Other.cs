using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
