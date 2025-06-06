using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{
    public class CourseRepository : ICourseRepository
    {

        ApplicationContext _context;

        public CourseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public int? Create(Course entity)
        {
            var result = _context.Courses.Add(entity);
            if (result == null)
                return null;

            List<DateOnly> l = [];
            var current = entity.TimeOfStartCourse;
            while (current.DayOfWeek != entity.DayOfWeekend)
            {
                current.AddDays(1);
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
                    TimeTableLessonId = entity.Id,
                    CoachId = entity.CoachId,
                    DayOfWorkedLesson = item.ToDateTime(entity.TimeOfLesson)
                });
               
            }
            _context.Lessons.AddRange(l1);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return entity.Id;
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetAll()
        {
            throw new NotImplementedException();
        }

        public Course? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}