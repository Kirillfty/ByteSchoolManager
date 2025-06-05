using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Repository;

public interface IStudentRepository: IRepository<Student>
{
}

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationContext _context;
    
    public StudentRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Student> GetAll()
    {
        return _context.Students.ToList();
    }

    public Student? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int? Create(Student student)
    {
        var result = _context.Students.Add(student);
        if (result == null)
            return null;

        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return null;
        }

        return student.Id;
    }

    public bool Update(Student student)
    {
        _context.Students.Update(student);
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