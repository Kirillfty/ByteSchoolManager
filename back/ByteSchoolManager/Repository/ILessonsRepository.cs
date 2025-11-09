using ByteSchoolManager.Entities;
using ByteSchoolManager.Features.Lessons.GetLessonWithStudents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository
{
    public interface ILessonsRepository : IRepository<Lesson>
    {
        public bool RescheduleLesson(int lessonId, DateTime date);
        public bool RescheduleCoachInLesson(int lessonId, int coachId);
        public List<Lesson> FilterLessonsWithCoachId(int coachid);
    }

    public class LessonRepository : ILessonsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LessonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public List<Lesson> FilterLessonsWithCoachId(int coachid)
        {

            var lessons = _dbContext.Lessons
            .Where(u => u.CoachId == coachid)
            .OrderBy(u => u.DateAndTime)
            .Reverse()
            .ToList();

            return lessons;
        }
    }
}