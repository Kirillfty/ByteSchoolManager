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

            return dayOfWeeks.Select(dayOfWeek => dayOfWeek switch
            {
                Course.DayOfWeek.Sunday => DayOfWeek.Sunday,
                Course.DayOfWeek.Monday => DayOfWeek.Sunday,
                Course.DayOfWeek.Tuesday => DayOfWeek.Sunday,
                Course.DayOfWeek.Wednesday => DayOfWeek.Sunday,
                Course.DayOfWeek.Thursday => DayOfWeek.Sunday,
                Course.DayOfWeek.Friday => DayOfWeek.Sunday,
                Course.DayOfWeek.Saturday => DayOfWeek.Sunday
            }).ToList();
        }
    }
}