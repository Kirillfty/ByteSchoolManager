using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        return _context.Students.FirstOrDefault(u => u.Id == id);
    }

    public int? Create(Student student)
    {
        var result = _context.Students.Add(student);
        if (result == null)
            return null;

        _context.SaveChanges();
     

        return student.Id;
    }

    public bool Update(Student student)
    {
        _context.Students.Update(student);

        _context.SaveChanges();
       
        return true;
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }
}