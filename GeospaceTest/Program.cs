using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Support01();
            Support02(); 
            Console.ReadKey();
        }
        static void Support02()
        {
            string strFile = Environment.CurrentDirectory;
            strFile = Path.GetDirectoryName(strFile);
            strFile = Path.GetDirectoryName(strFile);
            strFile = Path.GetDirectoryName(strFile);
            strFile += "\\documents\\armgf1dan.txt";

            if (File.Exists(strFile))
            {
                Console.WriteLine("Файл существует:");
                Console.WriteLine(strFile);
            }
            else 
            {
                Console.WriteLine("Файл не существует:");
                Console.WriteLine(strFile);
            }

            try
            {
                using (StreamReader sr = new StreamReader(strFile))
                {
                    String line = sr.ReadToEnd();
                    string[] delimiters = new string[] { "[ETX]" };
                    foreach (var item in line.Split(new char[] { '\u0002', '\u0003' },
                                 StringSplitOptions.RemoveEmptyEntries)) {

                        string theCode = GeospaceEntity.Helper.HelperIonka.Normalize(item);

                        if (theCode != "")
                        {
                             Console.WriteLine(theCode);
                            Console.ReadKey();
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            
        }
        static void Support01()
        {
            string strIonka = "\"IONKA 46501 50331 7/3/7 /0000 01025 32/19 04225 04520 38284 //100 0343/ //7// /0100 01024 32319 04217 //722 /7285 //102 0383/ //7// /0200 09824 32319 04010 //720 /7290 //100 0373/ //7// \"";
            try
            {
                GeospaceEntity.Models.Station theStation = new GeospaceEntity.Models.Station();
                theStation.TryParser(strIonka);
                theStation.PrintToConsole();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }
    }
}
