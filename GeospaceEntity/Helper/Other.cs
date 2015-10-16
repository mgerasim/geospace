using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceEntity.Models;
using System.IO;
using System.Net.Mail;

namespace GeospaceEntity.Helper
{

    public static class Other
    {
        //дописать q пробелов с обоих сторон
        public static string Add_Spaces( string s, int q )
        {
            string final = s.Trim();
            int len = final.Length, start, end;


            if( len < 0 ) return "";

            if( q % 2 == 0 )
            {
                start = q / 2;
                end = start;
            }
            else
            {
                start = q / 2 + 1;
                end = start - 1;
            }

            final = "";
            for( int i = 0; i < start; i++)
                final += ' ';
            
            final += s.Trim();
            for (int i = 0; i < end; i++)
                final += ' ';

            return final;
        }
        public static void SendToAspd(string subject, string msg)
        {
            Settings theSetting = new Settings();
            theSetting = theSetting.GetAll()[0];
            MailMessage mail = new MailMessage();
            string host = theSetting.SNMP_host;
            int port = theSetting.SNMP_port;
            string msg_from = theSetting.Email_ASPD_From;
            SmtpClient SmtpServer = new SmtpClient(host);
            mail.From = new MailAddress(msg_from);
            foreach (var line in theSetting.Email_ASPD_To.Split(new string[] { ";" }, StringSplitOptions.None))
            {
                mail.To.Add(line);
            }
            mail.Subject = subject;
            mail.IsBodyHtml = false;
            mail.BodyEncoding = Encoding.GetEncoding("koi8-r");

            Encoding koi8 = Encoding.GetEncoding("KOI8-R");
            Encoding utf8 = Encoding.UTF8;
            string Message = msg;
            byte[] utfBytes = utf8.GetBytes(Message);
            byte[] isoBytes = Encoding.Convert(utf8, koi8, utfBytes);
            Message = koi8.GetString(isoBytes);

            mail.Body = Message;
            SmtpServer.Port = port;


            SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);



        }
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
