using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository
{
    public interface CoordinatorR
    {
        public bool UpdateUser(User userId);
        public bool UpdateUserRole(User userId);
        public int? CreateCoach(Coach coach);
        public int? CreateStudent(Student student);
        public bool UpdateStudent(Student studentId);
        public bool UpdateStudentStatus(Student studentId);
        public List<Student> GetAllStudents();
        public int? CreateTimeTableLesson(TimeTableLesson lesson);
        public bool UpdateTimeTableLesson(TimeTableLesson lessonId);
        public TimeTableLesson? GetByIdTimeTableLesson(int lessonId);

    }
}
