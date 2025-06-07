using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{

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
        _context.Courses.Update(entity);

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

        return true;
    }
}