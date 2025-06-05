using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository
{
    public class CoordinatorRepository : CoordinatorR
    {
        public ApplicationContext _context;
        public CoordinatorRepository(ApplicationContext context)
        {
            _context = context;
        }
        public int? CreateCoach(Coach coach)
        {
            var result = _context.Coaches.Add(coach);
            if (result == null)
                return null;

            try {
                _context.SaveChanges();
            }
            catch(Exception ex) {
                return null;
            }
            return coach.Id;
        }

        public int? CreateStudent(Student student)
        {
            var result = _context.Students.Add(student);
            if (result == null)
                return null;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return student.Id;
        }

        public int? CreateTimeTableLesson(TimeTableLesson lesson)
        {
            var result = _context.TimetableLessons.Add(lesson);
            if (result == null)
                return null;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return lesson.Id;
        }

        public List<Student> GetAllStudents()
        {
           return _context.Students.ToList();
        }

        public TimeTableLesson? GetByIdTimeTableLesson(int lessonId)
        {
            return _context.TimetableLessons.FirstOrDefault(u => u.Id == lessonId);
        }

        public bool UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateStudentStatus(Student studentId)
        {
            _context.Students.Update(studentId);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateTimeTableLesson(TimeTableLesson lessonId)
        {
            _context.TimetableLessons.Update(lessonId);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateUser(User userId)
        {
            _context.Users.Update(userId);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateUserRole(User userId)
        {
            _context.Users.Update(userId);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
