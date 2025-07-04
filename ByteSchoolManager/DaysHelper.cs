﻿using ByteSchoolManager.Entities;

namespace ByteSchoolManager
{
    public static class DaysHelper
    {
        public static List<DayOfWeek> GetDays(Course.DayOfWeek days)
        {
            var dayOfWeeks = Enum.GetValues(typeof(Course.DayOfWeek))
                .Cast<Course.DayOfWeek>()
                .Where(f => days.HasFlag(f))
                .ToList();
            
            return dayOfWeeks.Where(u => u != Course.DayOfWeek.None).Select(dayOfWeek => dayOfWeek switch
            {
                
                Course.DayOfWeek.Sunday => DayOfWeek.Sunday,
                Course.DayOfWeek.Monday => DayOfWeek.Monday,
                Course.DayOfWeek.Tuesday => DayOfWeek.Tuesday,
                Course.DayOfWeek.Wednesday => DayOfWeek.Wednesday,
                Course.DayOfWeek.Thursday => DayOfWeek.Thursday,
                Course.DayOfWeek.Friday => DayOfWeek.Friday,
                Course.DayOfWeek.Saturday => DayOfWeek.Saturday
            }).ToList();

        }
        public static Course.DayOfWeek GetDayOfWeek(DayOfWeek[] days)
        {
            Course.DayOfWeek dayOfWeek = 0;
            foreach (var day in days)
            {
                dayOfWeek = dayOfWeek | day switch
                {
                    DayOfWeek.Sunday => Course.DayOfWeek.Sunday,
                    DayOfWeek.Monday => Course.DayOfWeek.Monday,
                    DayOfWeek.Tuesday => Course.DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday => Course.DayOfWeek.Wednesday,
                    DayOfWeek.Thursday => Course.DayOfWeek.Thursday,
                    DayOfWeek.Friday => Course.DayOfWeek.Friday,
                    DayOfWeek.Saturday => Course.DayOfWeek.Saturday,
                    
                };
            }
            return dayOfWeek;
        }
    }
}