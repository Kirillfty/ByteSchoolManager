using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface IUserRepository : IRepository<User>
{
    
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public int? Create(User entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(User userId)
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

    public List<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }
}