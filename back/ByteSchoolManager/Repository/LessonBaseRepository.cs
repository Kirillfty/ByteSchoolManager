using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class LessonBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<Lesson>(dbContext)
{
    public override IQueryable<Lesson> Query => _dbContext.Lessons;
}