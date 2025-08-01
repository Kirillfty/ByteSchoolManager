using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICoachRepository : IRepository<Coach>
{
    public Coach? GetByUserId(int userId);
}

public class CoachRepository : ICoachRepository
{
    private readonly ApplicationContext _context;
    
    public CoachRepository(ApplicationContext context)
    {
        _context = context;
    }
    public Coach? GetByUserId(int userId) {
        return _context.Coaches.FirstOrDefault(u => u.UserId == userId);
    }
    public List<Coach> GetAll()
    {
        return _context.Coaches.ToList();
    }

    public Coach? GetById(int id)
    {
        return _context.Coaches.FirstOrDefault(u => u.Id == id);
    }

    public int? Create(Coach coach)
    {
        var result = _context.Coaches.Add(coach);
        if (result == null)
            return null;

        
        _context.SaveChanges();
        
       
  
        return coach.Id;
    }

    public bool Update(Coach entity)
    {
        _context.Coaches.Update(entity);
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

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }
}