using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public class UserBaseRepository(ApplicationDbContext dbContext) : RepositoryBase<User>(dbContext)
{
    public override IQueryable<User> Query => _dbContext.Users;
}