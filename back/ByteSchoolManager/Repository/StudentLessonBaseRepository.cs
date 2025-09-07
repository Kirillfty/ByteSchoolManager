using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class StudentLessonBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<StudentLesson>(dbContext)
{
    public override IQueryable<StudentLesson> Query => _dbContext.StudentLessons;
}