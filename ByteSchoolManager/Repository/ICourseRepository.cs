
using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{
    public bool UpdateDayOfLesson(Course course);
    public bool UpdateTimeStartCourse(Course course);
    public bool UpdateTimeEndCourse(Course course);
    public bool UpdateCoachCourse(Course course);
    public bool UpdateTimeOfCourse(Course course);
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
        var oldCourses = _context.Courses.FirstOrDefault(u => u.Id == newCourse.Id);
        var lessons = _context.Lessons.Where(u => u.CourseId == newCourse.Id).ToList();


        if (lessons.All(u => u.Status == Lesson.LessonStatus.NotDone) && oldCourses.DateOfStartCourse != newCourse.DateOfStartCourse)
        {
            _context.RemoveRange(lessons);
            var days = GetDatesBetweenStartAndEndByDaysOfWeek(newCourse.DateOfStartCourse,newCourse.DateOfEndCourse,newCourse.DaysOfWeek);
            var newLessons = days.Select(u => new Lesson { CoachId = newCourse.CoachId,
                CourseId = newCourse.Id,
                DateAndTime = u.ToDateTime(newCourse.TimeOfLesson)
            }).ToList();

            _context.Lessons.AddRange(newLessons);
            _context.Courses.Update(newCourse);

            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool UpdateTimeEndCourse(Course newCourse)
    {
        var oldCourses = _context.Courses.FirstOrDefault(u => u.Id == newCourse.Id);

        if (oldCourses.DateOfEndCourse == newCourse.DateOfEndCourse)
            return false;
       
        

        var notDoneLessons = _context.Lessons.Where(u => u.Status != Lesson.LessonStatus.Done);

        _context.Lessons.RemoveRange(notDoneLessons);

        var lastLesson = _context.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();
        DateOnly date;

        if (lastLesson == null)
        {
            date = DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1));
        }
        else {
            date = oldCourses.DateOfStartCourse;
        }

        var days = GetDatesBetweenStartAndEndByDaysOfWeek(date, newCourse.DateOfEndCourse, newCourse.DaysOfWeek);
        var newLessons = days.Select(u => new Lesson
        {
            CoachId = newCourse.CoachId,
            CourseId = newCourse.Id,
            DateAndTime = u.ToDateTime(newCourse.TimeOfLesson)
        }).ToList();

        _context.Lessons.AddRange(newLessons);
        _context.Courses.Update(newCourse);
        _context.SaveChanges();

        return true;
    }
    public bool UpdateCoachCourse(Course course) {

        var lessons = _context.Lessons.Where(u => u.CourseId == course.Id && u.Status == Lesson.LessonStatus.NotDone).ToList();
        var lastLesson = _context.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();

        var dates = GetDatesBetweenStartAndEndByDaysOfWeek(
            DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1)),
            course.DateOfEndCourse,
            course.DaysOfWeek
        );

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
    public bool UpdateTimeOfCourse(Course course) {
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
