using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository
{
    public class CoordinatorRepository
    {
        /*//перенести в course repository
        public int? CreateTimeTableLesson(Course lesson)
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

        
        //перенести в course repository
        public Course? GetByIdTimeTableLesson(int lessonId)
        {
            return _context.TimetableLessons.FirstOrDefault(u => u.Id == lessonId);
        }

        
        //перенести в course repository
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

        //перенести в course repository
        public bool UpdateTimeTableLesson(Course lessonId)
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
        }*/
    }
}
