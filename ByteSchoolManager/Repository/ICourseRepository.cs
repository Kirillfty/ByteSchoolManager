using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{
    public bool UpdateDayOfLesson(Course course);
    public bool UpdateTimeStartCourse(Course course);
    public bool UpdateTimeEndCourse(Course course);
}

public class CourseRepository : ICourseRepository
{
    ApplicationContext _context;

    public CourseRepository(ApplicationContext context)
    {
        _context = context;
    }

    private List<DateOnly> GetDatesBetweenStartAndEndByDaysOfWeek(DateOnly start, DateOnly end,
        Course.DayOfWeek daysOfWeek)
    {
        List<DateOnly> dates = [];

        var days = DaysHelper.GetDays(daysOfWeek);

        var currentDate = start;
        while (currentDate <= end)
        {
            // Если текущий день недели содержится в списке, добавляем дату в результат
            if (days.Contains(currentDate.DayOfWeek))
            {
                dates.Add(currentDate);
            }

            // Переходим к следующей дате
            currentDate = currentDate.AddDays(1);
        }

        return dates;
    }

    public int? Create(Course course)
    {
        _context.Courses.Add(course);

        _context.SaveChanges();

        var lessons = new List<Lesson>();
        var dates = GetDatesBetweenStartAndEndByDaysOfWeek(course.DateOfStartCourse, course.DateOfEndCourse,
            course.DaysOfWeek);

        foreach (var date in dates)
        {
            lessons.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DateAndTime = date.ToDateTime(course.TimeOfLesson)
            });
        }

        _context.Lessons.AddRange(lessons);

        _context.SaveChanges();

        return course.Id;
    }

    public bool UpdateDayOfLesson(Course course)
    {
        var notDoneLessons = _context.Lessons.Where(u => u.Status != Lesson.LessonStatus.Done);

        _context.Lessons.RemoveRange(notDoneLessons);

        var lastLesson = _context.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();

        var dates = GetDatesBetweenStartAndEndByDaysOfWeek(
            DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1)),
            course.DateOfEndCourse,
            course.DaysOfWeek
        );

        List<Lesson> lessons = new List<Lesson>();
        foreach (var date in dates)
        {
            lessons.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DateAndTime = date.ToDateTime(course.TimeOfLesson)
            });
        }

        _context.Lessons.AddRange(lessons);
        _context.Courses.Update(course);
        _context.SaveChanges();

        return true;
    }

    public bool UpdateTimeStartCourse(Course newCourse)
    {
        var lessons = _context.Lessons.ToList();
        var oldCourses = _context.Courses.FirstOrDefault(u => u.Id == newCourse.Id);

        if (lessons.Any(u => u.Status == Lesson.LessonStatus.NotDone) &&
            oldCourses.DateOfStartCourse != newCourse.DateOfStartCourse)
        {
            _context.RemoveRange(lessons);

            var lastLesson = _context.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();
            List<DateOnly> l = [];
            var current = DateOnly.FromDateTime(lastLesson.DateAndTime);
            while (current.DayOfWeek != newCourse.DaysOfWeek)
            {
                current = current.AddDays(1);
            }

            l.Add(current);
            while (current <= newCourse.DateOfEndCourse)
            {
                l.Add(current);
                current = current.AddDays(7);
            }

            List<Lesson> l1 = new List<Lesson>();
            foreach (var item in l)
            {
                l1.Add(new Lesson
                {
                    CourseId = newCourse.Id,
                    CoachId = newCourse.CoachId,
                    DateAndTime = item.ToDateTime(newCourse.TimeOfLesson)
                });
            }

            _context.Lessons.AddRange(l1);

            return true;
        }

        return false;
    }

    public bool UpdateTimeEndCourse(Course newCourse)
    {
        var oldCourses = _context.Courses.FirstOrDefault(u => u.Id == newCourse.Id);

        if (oldCourses.DateOfEndCourse == newCourse.DateOfEndCourse)
        {
            return false;
        }

        var notDoneLessons = _context.Lessons.Where(u => u.Status != Lesson.LessonStatus.Done);

        _context.Lessons.RemoveRange(notDoneLessons);

        var lastLesson = _context.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();

        List<DateOnly> l = [];
        var current = DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1));
        while (current.DayOfWeek != newCourse.DaysOfWeek)
        {
            current = current.AddDays(1);
        }

        l.Add(current);
        while (current <= newCourse.DateOfEndCourse)
        {
            l.Add(current);
            current = current.AddDays(7);
        }

        List<Lesson> l1 = new List<Lesson>();
        foreach (var item in l)
        {
            l1.Add(new Lesson
            {
                CourseId = newCourse.Id,
                CoachId = newCourse.CoachId,
                DateAndTime = item.ToDateTime(newCourse.TimeOfLesson)
            });
        }

        _context.Lessons.AddRange(l1);
        _context.Courses.Update(newCourse);
        _context.SaveChanges();

        return true;
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }

    public List<Course> GetAll()
    {
        return _context.Courses.ToList();
    }

    public Course? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(Course entity)
    {
        throw new NotFiniteNumberException();
    }
}