using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvcApplication1.Models
{
    public class Month
    {
        public int Dates { get; set; }
        public DateTime TodaysDate { get; set; }
        public int FirstDateInMonth { get; set; } // Elementnummer första veckodagen
        public int LastDateInMonth { get; set; }  // Elementnummer sista veckodagen
        public int DatesLastMonth { get; set; }
        public int WeekNow { get; set; }

        public int Day { get; set; }
        public DateTime NextMonth { get; set; }
        public DateTime PrevMonth { get; set; }
        public DateTime NextWeek { get; set; }
        public DateTime PrevWeek { get; set; }
        public List<string> Listweekdays { get; set; }

        public Events Events { get; set; }
        public List<Events> EventsList = new List<Events>();
        public string Token { get; set; }

        public Month()
        {
        }

        public Month(int year, int month, int dayWeek)
        {
            //NextWeek = new DateTime(year, month, dayWeek);
            Dates = DateTime.DaysInMonth(year, month);

            if (dayWeek >= Dates)
            {
                NextWeek = new DateTime(year, month + 1, dayWeek);
            }
            else
            {
                NextWeek = new DateTime(year, month, dayWeek);
            }

            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;

            // hittar veckan just nu...
            var date1 = new DateTime(year, month, dayWeek);
            WeekNow = cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public Month(int year, int month, List<Events> listByEmail)
        {
            var day = DateTime.Now.Day;
            TodaysDate = new DateTime(year, month, day);
            Dates = DateTime.DaysInMonth(year, month);

            NextMonth = new DateTime(year, month, day);
            const int dayFirstMonth = 1;
            var dateFirstMonth = new DateTime(year, month, dayFirstMonth);   // datum första månadsdagen
            var dateLastMonth = new DateTime(year, month, Dates);            // datum sista månadsdagen

            // Hämtar veckodagarna och söndag placeras sist i listan
            string[] weekdays = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
            Listweekdays = new List<string>(weekdays);
            Listweekdays.Add(Listweekdays[0]);

            // Lägger till events i listan utifrån den som är inloggad
            foreach (var events in listByEmail)
            {
                EventsList.Add(events);
            }
            // Ser till att bli rätt år och månad på variablerna, när aktuell månad är jan eller dec
            if (month == 12)
            {
                NextMonth = new DateTime(year + 1, month - 11, day);
            }
            else
            {
                NextMonth = new DateTime(year, month + 1, day);
            }
            if (month == 1)
            {
                PrevMonth = new DateTime(year - 1, month + 11, day);
            }
            else
            {
                PrevMonth = new DateTime(year, month - 1, day);
            }

            // Skapar aktuell månadsvy vid start
            if (((int)(DayOfWeek)Enum.ToObject(typeof(DayOfWeek), (int)dateFirstMonth.DayOfWeek)) == 0)
            {
                FirstDateInMonth = 7;  // söndagen får elementnummer 7
            }
            else
            {
                FirstDateInMonth = (int)(DayOfWeek)Enum.ToObject(typeof(DayOfWeek), (int)dateFirstMonth.DayOfWeek);
            }
            if (((int)(DayOfWeek)Enum.ToObject(typeof(DayOfWeek), (int)dateLastMonth.DayOfWeek)) == 0)
            {
                LastDateInMonth = 7;  // söndagen får elementnummer 7
            }
            else
            {
                LastDateInMonth = (int)(DayOfWeek)Enum.ToObject(typeof(DayOfWeek), (int)dateLastMonth.DayOfWeek);
            }
            DatesLastMonth = DateTime.DaysInMonth(year, month);
        }
    }
}