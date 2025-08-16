using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class StudentCourseBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<StudentCourse>(dbContext)
{
    public override IQueryable<StudentCourse> Query => _dbContext.StudentCourses;
}