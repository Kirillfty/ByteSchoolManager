using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ByteSchoolManager.Repository;

public interface IStudentRepository: IRepository<Student>
{
}

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public StudentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Student> GetAll()
    {
        return _dbContext.Students.ToList();
    }

    public Student? GetById(int id)
    {
        return _dbContext.Students.FirstOrDefault(u => u.Id == id);
    }

    public int? Create(Student student)
    {
        var result = _dbContext.Students.Add(student);
        if (result == null)
            return null;

        _dbContext.SaveChanges();
     

        return student.Id;
    }

    public bool Update(Student student)
    {
        _dbContext.Students.Update(student);

        _dbContext.SaveChanges();
       
        return true;
    }

    public bool Delete(int entityId)
    {
        var student = _dbContext.Students.FirstOrDefault(u => u.Id == entityId);
        if (student is not null) {
            _dbContext.Remove(student);
            _dbContext.SaveChanges();
            return true;
        }
        return false;
    }
}