using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface IUserRepository : IRepository<User>
{
    public bool SetRefreshToken(string refreshToken, int id);
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
        _context.Users.Add(entity);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return null;
        }
        return entity.Id;
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
        return _context.Users.ToList();
    }

    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }
    public bool SetRefreshToken(string refreshToken, int id)
    {
        User? r = _context.Users.FirstOrDefault(u => u.Id == id);

        if (r == null)
            return false;

        r.RefreshToken = refreshToken;

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
}