using ByteSchoolManager.Entities;
using ByteSchoolManager.Responces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository
{
    public interface ILessonsRepository : IRepository<Lesson>
    {
        public GetLessonByIdResponce? GetByIdWithStudents(int id);
        public bool RescheduleLesson(int lessonId, DateTime date);
        public bool RescheduleCoachInLesson(int lessonId, int coachId);
    }

    public class LessonRepository : ILessonsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LessonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetLessonByIdResponce? GetByIdWithStudents(int id)
        {
            var lesson = _dbContext.Lessons
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

        public bool RescheduleLesson(int lessonId, DateTime date)
        {
            var lesson = _dbContext.Lessons.FirstOrDefault(u => u.Id == lessonId);

            if (lesson is null)
            {
                return false;
            }

            lesson.DateAndTime = date;

            _dbContext.SaveChanges();
            return true;
        }

        public bool RescheduleCoachInLesson(int lessonId, int coachId)
        {
            var lesson = _dbContext.Lessons.FirstOrDefault(u => u.Id == lessonId);

            if (lesson == null)
            {
                return false;
            }

            lesson.CoachId = coachId;

            _dbContext.SaveChanges();
            return true;
        }

        public int? Create(Lesson entity) => throw new NotImplementedException();

        public bool Delete(int entityId) => throw new NotImplementedException();

        public List<Lesson> GetAll() => throw new NotImplementedException();


        public bool Update(Lesson entity) => throw new NotImplementedException();

        Lesson? IRepository<Lesson>.GetById(int id) => throw new NotImplementedException();

        public Lesson? GetById([FromRoute] int id) => throw new NotImplementedException();
    }
}