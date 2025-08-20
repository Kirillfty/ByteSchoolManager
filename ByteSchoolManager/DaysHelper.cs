using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;

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
        
        public static int[] GetDayNumbers(Course.DayOfWeek days)
        {
            var dayOfWeeks = Enum.GetValues(typeof(Course.DayOfWeek))
                .Cast<Course.DayOfWeek>()
                .Where(f => days.HasFlag(f))
                .ToList();

            return dayOfWeeks.Where(u => u != Course.DayOfWeek.None).Select(dayOfWeek => dayOfWeek switch
            {
                Course.DayOfWeek.Sunday => 0,
                Course.DayOfWeek.Monday => 1,
                Course.DayOfWeek.Tuesday => 2,
                Course.DayOfWeek.Wednesday => 3,
                Course.DayOfWeek.Thursday => 4,
                Course.DayOfWeek.Friday => 5,
                Course.DayOfWeek.Saturday => 6
            }).ToArray();
        }

        public static Course.DayOfWeek GetDayOfWeek(IEnumerable<DayOfWeek> days)
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
                    _ => throw new BadRequestException($"Unknown day[{(int)day}]. DayOfWeek must be in range 0 - 6.")
                };
            }

            return dayOfWeek;
        }
    }
}