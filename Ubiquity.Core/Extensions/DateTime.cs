using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubiquity.Core.Extensions
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Given a date, it returns the next (specified) day of week 
        /// </summary>
        /// <param name="date">Date to process</param>
        /// <param name="day">Day of week to find on calendar</param>
        /// <returns>Future date</returns>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek day = DayOfWeek.Monday)
        {
            while (true)
            {
                if (date.DayOfWeek == day)
                    return date;
                date = date.AddDays(1);
            }
        }

        /// <summary>
        /// Given a date, it returns the next (specified) day of week
        /// </summary>
        /// <param name="date">Date to process</param>
        /// <param name="timezoneFromUtc">Hours of the timezone from UTC</param>
        /// <returns>Future date</returns>
        public static DateTime LocalTimeFromUtc(this DateTime date, int timezoneFromUtc)
        {
            return date.ToUniversalTime().AddHours(timezoneFromUtc);
        }

        public static DateTime Midnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime RightBeforeMidnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }


        public static int GetWeekNumber(this DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
            return weekNum;
        }

        public static string ConvertDate(this DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue)
            {
                if (excludeHoursAndMinutes)
                    return datetTime.ToString("yyyy-MM-dd");
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return null;
        }

        public static string ToOracleString(this DateTime date)
        {
            return $" TO_DATE('{ date.ToString("dd-mmm-yyyy hh:nn:ss")}','DD-MON-YYYY HH24:MI:SS') ";
        }


        public static DateTime GetFirstDayOfWeek(this int weekNumber, int year, CultureInfo culture)
        {
            DateTime _now = DateTime.Now;
            //if we are wrpping into the new year we need to add to the year.
            if (weekNumber < _now.GetWeekNumber())
            {
                year++;
            }

            Calendar calendar = culture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber);
            DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(-1);
            }

            return targetDay;
        }

        public static DateTime PreviousWeek(this DateTime date, int weeksBack = 1)
        {
            return date.AddDays(-7 * weeksBack);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static List<DateTime> GetWeek(this DateTime date)
        {
            List<DateTime> results = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                results.Add(date.AddDays(i));
            }
            return results;
        }

        public static List<DateTime> Weeks(this DateTime date, int numberOfWeeks)
        {
            List<DateTime> results = new List<DateTime>();
            int daysOut = numberOfWeeks * 7;
            for (int i = 0; i < daysOut; i++)
            {
                results.Add(date.AddDays(i));
            }
            return results;
        }

        public static string ShortDay(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Sun.";
                case DayOfWeek.Monday:
                    return "Mon.";
                case DayOfWeek.Tuesday:
                    return "Tue.";
                case DayOfWeek.Wednesday:
                    return "Wed.";
                case DayOfWeek.Thursday:
                    return "Thu.";
                case DayOfWeek.Friday:
                    return "Fri.";
                case DayOfWeek.Saturday:
                    return "Sat.";
                default:
                    return "";
            }
        }

        public static DateTime Random(this DateTime date)
        {
            return new RandomDateTime().Next();
        }

        public static DateTime GetNextDayOfWeek(this DateTime date, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntil = ((int)day - (int)date.DayOfWeek + 7) % 7;
            DateTime next = date.Date.AddDays(daysUntil);
            return next;
        }

        public static DateTime GetNextDayOfWeek(this DateTime date, DayOfWeek day, TimeSpan timeSpan)
        {
            DateTime next = date.GetNextDayOfWeek(day).AddTicks(timeSpan.Ticks);
            return next;
        }

        public static DateTime GetNextDayOfWeek(this DateTime date, DayOfWeek day, int hour = 0, int minute = 0, int second = 0)
        {
            DateTime next = date.GetNextDayOfWeek(day);
            return new DateTime(next.Year, next.Month, next.Day, hour, minute, second);
        }

        public static DateTime ToTime(this DateTime date, int hour = 0, int minute = 0, int second = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
        }

        public static bool IsInSameWeek(this DateTime d, DateTime date)
        {            
            int _week = d.GetWeekNumber();
            int _otherDateWeek = date.GetWeekNumber();
            return _week == _otherDateWeek;
        }
    }

    internal class RandomDateTime
    {
        DateTime start;
        Random gen;
        int range;

        public RandomDateTime()
        {
            start = new DateTime(1995, 1, 1);
            gen = new Random();
            range = (DateTime.Today - start).Days;
        }

        public DateTime Next()
        {
            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }
    }
}
