using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface ICoachRepository : IRepository<Coach>
{
}

public class CoachRepository : ICoachRepository
{
    private readonly ApplicationContext _context;
    
    public CoachRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<Coach> GetAll()
    {
        throw new NotImplementedException();
    }

    public Coach? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int? Create(Coach coach)
    {
        var result = _context.Coaches.Add(coach);
        if (result == null)
            return null;

        try {
            _context.SaveChanges();
        }
        catch(Exception ex) {
            return null;
        }
        return coach.Id;
    }

    public bool Update(Coach entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }
}