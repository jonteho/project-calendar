using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvcApplication1.Models
{
    public class Week
    {
        public Events Events { get; set; }
        public int DayThisWeek { get; set; }
        public int MonthThisWeek { get; set; }
        public int YearThisWeek { get; set; }
        public DateTime TodaysDate { get; set; }
        public DateTime MondayDateThisWeek { get; set; }
        public int ThisWeek { get; set; }
        public int NextWeek { get; set; }
        public int PreWeek { get; set; }

        public List<string> Listweekdays { get; set; }

        public Week()
        {
        }

        public Week(int year, int month, int day)
        {
            YearThisWeek = year;
            MonthThisWeek = month;
            DayThisWeek = day;
            ChangeDate();
        }
        // Villkor för ändring av månadsbyte och årsbyte
        public void ChangeDate()
        {
            // Hämtar veckodagarna och söndag placeras sist i listan
            string[] weekdays = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
            Listweekdays = new List<string>(weekdays);
            Listweekdays.Add(Listweekdays[0]);

            const int monVerify = 1;
            const int tueVerify = 2;
            const int wedVerify = 3;
            const int thuVerify = 4;

            // Anpassa kalendern efter format och den gregorianska kalendern
            var dateformat = DateTimeFormatInfo.CurrentInfo;
            var gregorianCalendar = dateformat.Calendar;

            // Veckans datum som är en måndag
            TodaysDate = new DateTime(YearThisWeek, MonthThisWeek, DayThisWeek);
            MondayDateThisWeek = TodaysDate.AddDays(-1 * ((int)TodaysDate.DayOfWeek - 1));

            // Hittar aktuell vecka, föregående vecka och nästkommande vecka.
            // Obs! GetWeekOfYear ger ej 100% tillförlitligt resultat av vecka, kontroll sker senare.
            ThisWeek = gregorianCalendar.GetWeekOfYear(TodaysDate, dateformat.CalendarWeekRule, dateformat.FirstDayOfWeek);
            NextWeek = gregorianCalendar.GetWeekOfYear(TodaysDate.AddDays(7), dateformat.CalendarWeekRule,
                                                       dateformat.FirstDayOfWeek);
            PreWeek = gregorianCalendar.GetWeekOfYear(TodaysDate.AddDays(-7), dateformat.CalendarWeekRule,
                                                       dateformat.FirstDayOfWeek);

            // ger dec31 veckodagsnummer för veckan
            var dec31 = new DateTime(YearThisWeek, 12, 31);
            var enumWeekdayDec31 = dec31.DayOfWeek;
            var indexWeekdayDec31 = (int)enumWeekdayDec31;

            // ger jan1 veckodagsnummer för veckan
            var jan1 = new DateTime(YearThisWeek, 1, 1);
            var enumWeekdayJan1 = jan1.DayOfWeek;
            var indexWeekdayJan1 = (int)enumWeekdayJan1;

            // ger dec31 veckodagsnummer för veckan föregående år
            var dec31PrevYear = new DateTime(YearThisWeek - 1, 12, 31);
            var enumWeekdayDec31PrevYear = dec31PrevYear.DayOfWeek;
            var indexWeekdayDec31PrevYear = (int)enumWeekdayDec31PrevYear;

            // ger jan1 veckodagsnummer för veckan föregående år
            var jan1PrevYear = new DateTime(YearThisWeek - 1, 1, 1);
            var enumWeekdayJan1PrevYear = jan1PrevYear.DayOfWeek;
            var indexWeekdayJan1PrevYear = (int)enumWeekdayJan1PrevYear;

            // Korrigerar så att veckonummer blir rätt vid årsbyte
            if (ThisWeek == 52)
            {
                if ((indexWeekdayDec31 == monVerify) || (indexWeekdayDec31 == tueVerify) || (indexWeekdayDec31 == wedVerify))   // Om vecka 52 består av mån, tis eller ons den 31:a dec, då är denna vecka 1
                {
                    NextWeek = 1;
                }
            }
            if (ThisWeek == 53)
            {
                if ((indexWeekdayDec31 == monVerify) || (indexWeekdayDec31 == tueVerify) || (indexWeekdayDec31 == wedVerify)) // Om mån, tis eller ons 31/12 då är vi inne i vecka 1, den sista veckan är 52 
                {
                    ThisWeek = 1;
                    NextWeek = 2;
                }
                if ((indexWeekdayJan1 == thuVerify) && (indexWeekdayDec31 == thuVerify))  // Om 1/1 och 31/12 är tors, då stämmer det att månaden har 53 veckor
                {
                    ThisWeek = 53;
                    NextWeek = 1;
                }
            }
            // Föregående år
            if (ThisWeek == 2)
            {
                if ((indexWeekdayJan1PrevYear == thuVerify) && (indexWeekdayDec31PrevYear == thuVerify))
                // Om 1/1 och 31/12 är tors, då stämmer det att månaden har 53 veckor
                {
                    PreWeek = 53;
                }
                else
                {
                    PreWeek = 1;
                    YearThisWeek = MondayDateThisWeek.AddDays(-7).Year;
                }
            }
        }
    }
}