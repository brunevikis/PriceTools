using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary {
    public static class Tools {
        public static int[] GetCalendarDaysFromOperativeMonth(int year, int month) {

            var checkSum = 0;

            var result = new int[6];
            int i = 0;

            var monthStart = new DateTime(year, month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            int daysToRemove = 0;
            if (monthStart.DayOfWeek != DayOfWeek.Saturday) daysToRemove = 1 + (int)monthStart.DayOfWeek;

            var week = monthStart.AddDays(-daysToRemove);//.AddDays(-7);

            var weekStart = monthStart;
            var weekEnd = week.AddDays(7);

            do {
                result[i++] = (weekEnd - weekStart).Days;
                checkSum += (weekEnd - weekStart).Days;

                weekStart = weekEnd;


                if (weekStart.AddDays(7) > nextMonthStart)
                    weekEnd = nextMonthStart;
                else
                    weekEnd = weekStart.AddDays(7);

            } while (weekStart.Month == month);

            if ((nextMonthStart - monthStart).Days != checkSum) throw new Exception();


            return result;
        }

        public static Tuple<int, int> GetWeekNumberAndYear(DateTime date) {

            var nextFriday = date.DayOfWeek == DayOfWeek.Saturday ? date.AddDays(6) :
                date.AddDays((int)DayOfWeek.Friday - (int)date.DayOfWeek);

            var y = nextFriday.Year;

            var yearStart = new DateTime(y, 1, 1);
            yearStart = yearStart.AddDays(-1 * ((int)yearStart.DayOfWeek + 1) % 7);

            var weekNumber = (int)Math.Floor(((date - yearStart).TotalDays) / 7) + 1;

            return new Tuple<int, int>(weekNumber, y);
        }
    }
}
