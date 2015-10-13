using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceCore;
using GeospaceEntity.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace GeospaceTest
{

    class Program
    {
        static void Main(string[] args)
        {
           // Support01();

           //Support02();
           Support03();
           //Support04();
           // Support05(); 

           //Support06();
            //Support07();
           //Support08();

           //Support09();
           //Support10();
            //Support11();
           //Support11();
            //Support12();

            Console.WriteLine("Ok");
            Console.ReadKey();
        }
        static void SupportWord()
        {
           // Microsoft.Office.Interop.Word.Application wApp = null;
           // object oMissing = System.Reflection.Missing.Value;

           // wApp = new Microsoft.Office.Interop.Word.Application();
           // if (wApp == null)
           // {
           //     Console.WriteLine("wApp is null\r\n");
           //     Console.ReadKey();
           //    // ViewBag.Error += "wApp is null\r\n";
           // }
           // wApp.Visible = false;
           // var wDocument = wApp.Documents.Add(ref oMissing,
           //     ref oMissing,
           //     ref oMissing, ref oMissing);
           // if (wDocument == null)
           // {
           //     Console.WriteLine("wDocument is null\r\n");
           //     Console.ReadKey();
           //    // ViewBag.Error += "wDocument is null\r\n";
           // }
           //// ViewBag.Error += "2\r\n";
           // foreach (Microsoft.Office.Interop.Word.Section section in wDocument.Sections)
           // {
           //     Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
           //     headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
           //     headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
           //     headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
           //     headerRange.Font.Size = 14;
           //     headerRange.Text = "Таблица";
           // }

           // Console.WriteLine("3\r\n");
           // Console.ReadKey();
           //// ViewBag.Error += "3\r\n";
           // wDocument.Content.SetRange(0, 0);

           // var paragraphTable = wDocument.Paragraphs.Add();
           // //ViewBag.Error += "4\r\n";
           // paragraphTable.Range.InsertParagraphAfter();
           // //ViewBag.Error += "5\r\n";

           // //ViewBag.Error += "7\r\n";
           // Table firstTable = wDocument.Tables.Add(paragraphTable.Range, DateTime.DaysInMonth(2015, 9) + 1, 20 /*for Day Columnt*/);
           // firstTable.Borders.Enable = 1;
           // firstTable.Columns[1].PreferredWidth = 40f;
           // IList<ConsolidatedTable> tableView = ConsolidatedTable.GetByDateMM(2015, 9);
           // foreach (Row row in firstTable.Rows)
           // {
           //     if (row.IsFirst)
           //     {
           //         foreach (Cell cell in row.Cells)
           //         {
           //             if (cell.ColumnIndex == 1)
           //             {
           //                 cell.Range.Text = "День";
           //             }
           //             else
           //             {
           //                 cell.Range.Text = tableView[cell.ColumnIndex - 2].DD.ToString();
           //             }

           //         }
           //     }
           //     else
           //     {

           //         foreach (Cell cell in row.Cells)
           //         {
           //             if (cell.ColumnIndex == 1)
           //             {
           //                 cell.Range.Text = (cell.RowIndex - 1).ToString("D2");
           //             }
           //             else
           //             {
           //                 cell.Range.Text = theViewData.DisplaySafe(theViewData.theStationList[cell.ColumnIndex - 2].Code,
           //                     YYYY,
           //                     MM,
           //                     cell.RowIndex - 1);
           //             }

           //         }
           //     }

           // }


           // string nameDoc = HttpContext.Server.MapPath("~/App_Data/");
           // string fileName = theViewData.Title + ".doc";
           // string fileNameTemp = string.Format(@"{0}.doc", Guid.NewGuid());
           // nameDoc += fileNameTemp;

           // wApp.ActiveDocument.SaveAs2(nameDoc);
           // wApp.ActiveDocument.Close(true);

           // byte[] fileBytes = System.IO.File.ReadAllBytes(nameDoc);
           // System.IO.File.Delete(nameDoc);
           // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        static void Support12()
        {
            ILogger theLog = new LoggerConsole();
            ICalculation theCalc = new Calculation(theLog);
            theCalc.ConsolidatedTableCalc();
        }
        static void Support11()
        {
            ILogger theLog = new LoggerConsole();
            ICalculation theCalc = new Calculation(theLog);
            theCalc.DisturbanceCalc(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(1));
        }
        static void Support10()
        {
            ILogger theLog = new LoggerConsole();
            ICalculation  theCalc = new Calculation(theLog);
            theCalc.CharacterizationCalc(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(1));
        }
        static void Support09()
        {
            Post a = new Post();
            Post b = new Post();

            a.Longitude = 135;
            a.Latitude = 48.5;

            b.Longitude = 137;
            b.Latitude = 50.6;

            List<Post> listPost = new List<Post>();
            double angle = 0.0, lenght = 0.0;

            GeospaceEntity.Helper.HelperTrack.Calc_Track(a, b, listPost, ref lenght, ref angle);

            Console.WriteLine("a = ({0}, {1}) - {2} -  b = ({3}, {4})\nlenght = {5}, angle = {6}", a.Longitude, a.Latitude, listPost.Count,
                b.Longitude, b.Latitude,
                lenght, angle );

            foreach (Post item in listPost)
                Console.WriteLine("({0}, {1})", item.Longitude, item.Latitude);
        }

        static void Support08()
        {
            CodeMagma theMagma = new CodeMagma();
            theMagma.Station = Station.GetByCode(38701);
            theMagma.YYYY = DateTime.Now.Year;
            theMagma.MM = DateTime.Now.Month;
            theMagma.DD = DateTime.Now.Day;
            theMagma.HH = DateTime.Now.Hour;
            theMagma.Raw = "test";
            theMagma.Save();
        }
        static void Support07()
        {
            //Begin.Save_From_File("C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\All_Begin_Telegramm.txt");
            string s1 = @"\\192.168.72.123\obmen\armgf1dan.txt";
            string s2 = "C:\\Users\\distomin\\Projects\\GeoSpace\\documents\\test.txt";
            string s3 =  "D:\\Мои документы\\visual studio 2013\\Projects\\GeoSpace\\documents\\armgf1dan.txt";
            ILogger theLogFile = new LoggerNLog();
            ILogger theConsoleLog = new LoggerConsole();
            IDecode theDecode = new Decode(theLogFile, s1);

            theDecode.Run();            
        }

        static void Support06()
        {
            ILogger theLogA = new LoggerCalc("logAverage", "errorAverage");
            ICalculation theCalcA = new Calculation(theLogA);
            //theCalcA.AverageCalc_Run();

            ILogger theLogM = new LoggerCalc("logMediana", "errorMediana");
            ICalculation theCalcM = new Calculation(theLogM);
            theCalcM.MedianaCalc_Run();
        }

        static void Support04()
        {
            ILogger theLoggerAverage = new LoggerCalc("logAverage", "errorAverage");

            ICalculation theCalcAverage = new Calculation(theLoggerAverage);
            //theCalcAverage.AverageCalc_Run();
            theCalcAverage.AverageCalc_Run(new DateTime(2015,6,1,0,0,0), new DateTime(2015, 10, 7, 23, 0, 0));
        }   
        
        
        static void Support03()
        {
            try
            {
                GeospaceEntity.Common.NHibernateHelper.UpdateSchema();
            }
            catch( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Support02()
        {
            try
            {
                List<string> output = new List<string>();
                List<string> log = new List<string>();
                log.Add("");

                output.Add("");   //MUF
                output.Add("");   //OPF
                output.Add("");   //параметры: D
                int W = 60; 

                Track track = Track.GetById(1);
                string param = track.PointA.Longitude + " "
                    + track.PointA.Latitude + " "
                    + track.PointB.Longitude + " "
                    + track.PointB.Latitude + " "
                    + W.ToString() + " "
                    + DateTime.Now.AddMonths(-1).Month.ToString(); 

               // GeospaceEntity.Helper.HelperTrack.Start(log, output, param, true, true );
                Console.WriteLine(log[0]);
            }
            catch( System.Exception ex)
            {
                Console.WriteLine(ex.Message);
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
