using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class StudentBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<Student>(dbContext)
{
    public override IQueryable<Student> Query => _dbContext.Students;
}