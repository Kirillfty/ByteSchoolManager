using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICoachRepository : IRepository<Coach>
{
    public Coach? GetByUserId(int userId);
}

public class CoachRepository : ICoachRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public CoachRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Coach? GetByUserId(int userId) {
        return _dbContext.Coaches.FirstOrDefault(u => u.UserId == userId);
    }
    public List<Coach> GetAll()
    {
        return _dbContext.Coaches.ToList();
    }

    public Coach? GetById(int id)
    {
        return _dbContext.Coaches.FirstOrDefault(u => u.Id == id);
    }

    public int? Create(Coach coach)
    {
        var result = _dbContext.Coaches.Add(coach);
        if (result == null)
            return null;

        
        _dbContext.SaveChanges();
        
       
  
        return coach.Id;
    }

    public bool Update(Coach entity)
    {
        _dbContext.Coaches.Update(entity);
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

   
}