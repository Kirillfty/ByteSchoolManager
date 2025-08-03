using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ByteSchoolManager.Repository;

public interface IUserRepository : IRepository<User>
{
    public bool SetRefreshToken(string refreshToken, int id);
    public bool UpdateRoleUser(int userid,string role);
    public User? GetByLogin(string login);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int? Create(User entity)
    {
        _dbContext.Users.Add(entity);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return null;
        }
        return entity.Id;
    }

    public bool Update(User userId)
    {
        _dbContext.Users.Update(userId);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public List<User> GetAll()
    {
        return _dbContext.Users.ToList();
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
        User? r = _dbContext.Users.FirstOrDefault(u => u.Id == id);

        if (r == null)
            return false;

        r.RefreshToken = refreshToken;

        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public bool UpdateRoleUser(int userid, string role)
    {
        var result = _dbContext.Users.Where(u => u.Id == userid)
            .FirstOrDefault();
        if (result == null) {
            return false;
        }
        result.Role = role;
        _dbContext.SaveChanges();
        return true;
    }

    public User? GetByLogin(string login)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Login == login);
    }
}