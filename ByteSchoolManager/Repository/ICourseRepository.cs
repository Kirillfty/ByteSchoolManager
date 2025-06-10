using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{
    public bool UpdateDayOfLesson(Course course);
}

public class CourseRepository : ICourseRepository
{

    ApplicationContext _context;

    public CourseRepository(ApplicationContext context)
    {
        _context = context;
    }

    public int? Create(Course entity)
    {
        _context.Courses.Add(entity);

        _context.SaveChanges();

        List<DateOnly> l = [];
        var current = entity.TimeOfStartCourse;
        while (current.DayOfWeek != entity.DayOfWeekend)
        {
            current = current.AddDays(1);
        }
        l.Add(current);
        while (current <= entity.TimeOfEndCourse)
        {
            l.Add(current);
            current = current.AddDays(7);
        }

        List<Lesson> l1 = new List<Lesson>();
        foreach (var item in l)
        {
            l1.Add(new Lesson
            {
                CourseId = entity.Id,
                CoachId = entity.CoachId,
                DayOfWorkedLesson = item.ToDateTime(entity.TimeOfLesson)
            });

        }
        _context.Lessons.AddRange(l1);

        _context.SaveChanges();

        return entity.Id;
    }

    public bool UpdateDayOfLesson(Course course) {
        
        var notDoneLessons = _context.Lessons.Where(u => u.Status != Lesson.LessonStatus.Done);

        _context.Lessons.RemoveRange(notDoneLessons);

        var lastLesson = _context.Lessons.OrderBy(u => u.DayOfWorkedLesson).LastOrDefault();

        List<DateOnly> l = [];
        var current =DateOnly.FromDateTime(lastLesson.DayOfWorkedLesson);
        while (current.DayOfWeek != course.DayOfWeekend)
        {
            current = current.AddDays(1);
        }
        l.Add(current);
        while (current <= course.TimeOfEndCourse)
        {
            l.Add(current);
            current = current.AddDays(7);
        }

        List<Lesson> l1 = new List<Lesson>();
        foreach (var item in l)
        {
            l1.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DayOfWorkedLesson = item.ToDateTime(course.TimeOfLesson)
            });

        }
        _context.Lessons.AddRange(l1);
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