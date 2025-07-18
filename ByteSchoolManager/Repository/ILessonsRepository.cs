using ByteSchoolManager.Entities;
using ByteSchoolManager.Responces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository
{
    public interface ILessonsRepository : IRepository<Lesson>
    {
        public List<GetLessonByIdResponce> GetLessonByIdWithStudents(int id);
        public Lesson? GetById([FromRoute] int id);
    }

    public class LessonRepository : ILessonsRepository
    {
        private readonly ApplicationContext _context;

        public LessonRepository(ApplicationContext context)
        {
            _context = context;
        }
        public List<GetLessonByIdResponce> GetLessonByIdWithStudents(int id)
        {
            var res = _context.Lessons
                .Include(u => u.Students)
                .Where(u => u.Id == id)
                .Select(responce => new GetLessonByIdResponce(responce.Id,responce.Students))
                .ToList();
            return res;
        }
        public int? Create(Lesson entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Lesson> GetAll()
        {
            throw new NotImplementedException();
        }


        public bool Update(Lesson entity)
        {
            throw new NotImplementedException();
        }

        Lesson? IRepository<Lesson>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Lesson? GetById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }

}
