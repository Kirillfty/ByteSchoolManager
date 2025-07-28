using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class CourseBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<Course>(dbContext)
{
    public override IQueryable<Course> Query => _dbContext.Courses;
}