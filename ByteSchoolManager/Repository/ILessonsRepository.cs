using ByteSchoolManager.Entities;
using ByteSchoolManager.Responces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository
{
    public interface ILessonsRepository : IRepository<Lesson>
    {
        public GetLessonByIdResponce? GetLessonByIdWithStudents(int id);
        public Lesson? GetById([FromRoute] int id);
    }

    public class LessonRepository : ILessonsRepository
    {
        private readonly ApplicationContext _context;

        public LessonRepository(ApplicationContext context)
        {
            _context = context;
        }
        public GetLessonByIdResponce? GetLessonByIdWithStudents(int id)
        {
            var lesson = _context.Lessons
                .Include(u => u.Students)
                .FirstOrDefault(u => u.Id == id);

            if (lesson == null)
            {
                return null;
            }

            return new GetLessonByIdResponce(
                        lesson.Id,
                        lesson.Students.Select(u => new GetLessonByIdResponceStudent(u.Id, u.Name)).ToList());
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
