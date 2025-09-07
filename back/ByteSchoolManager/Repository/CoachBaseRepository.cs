using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class CoachBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<Coach>(dbContext)
{
    public override IQueryable<Coach> Query => _dbContext.Coaches;
}