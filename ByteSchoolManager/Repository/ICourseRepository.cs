using ByteSchoolManager.Entities;
using ByteSchoolManager.Features.Lessons.GetLessonsByDay;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Repository;

public interface ICourseRepository : IRepository<Course>
{
    public bool UpdateDayOfLesson(Course course);
    public bool UpdateDayStartCourse(Course course);
    public bool UpdateDayEndCourse(Course course);
    public bool UpdateCoachCourse(Course course);
    public bool UpdateTimeOfCourse(Course course);
    public bool AddStudentInCourse(int courseId, int[] studentsId);
    public List<GetLessonsInDayResponce> GetLessonsInDays(int coach, int day);
}

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private List<DateOnly> GetDatesBetweenStartAndEndByDaysOfWeek(DateOnly start, DateOnly end,
        Course.DayOfWeek daysOfWeek)
    {
        List<DateOnly> dates = [];

        var days = DaysHelper.GetDays(daysOfWeek);


        var currentDate = start;
        while (currentDate <= end)
        {
            // Если текущий день недели содержится в списке, добавляем дату в результат
            if (days.Contains(currentDate.DayOfWeek))
            {
                dates.Add(currentDate);
            }

            // Переходим к следующей дате
            currentDate = currentDate.AddDays(1);
        }

        return dates;
    }

    public bool UpdateDayOfLesson(Course course)
    {
        var notMarkedLessons = _dbContext.Lessons.Where(u => !u.Marked && u.CourseId == course.Id);

        _dbContext.Lessons.RemoveRange(notMarkedLessons);
        _dbContext.SaveChanges();

        var lastLesson = _dbContext.Lessons.Where(u => u.CourseId == course.Id).OrderBy(u => u.DateAndTime)
            .LastOrDefault();

        var date = lastLesson != null ? DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1)) : course.DateOfStartCourse;

        var dates = GetDatesBetweenStartAndEndByDaysOfWeek(date, course.DateOfEndCourse, course.DaysOfWeek);

        List<Lesson> lessons = new List<Lesson>();
        foreach (var data in dates)
        {
            lessons.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DateAndTime = data.ToDateTime(course.TimeOfLesson)
            });
        }

        _dbContext.Lessons.AddRange(lessons);
        _dbContext.Courses.Update(course);
        _dbContext.SaveChanges();

        return true;
    }

    public bool UpdateDayStartCourse(Course newCourse)
    {
        var oldCourses = _dbContext.Courses.AsNoTracking().FirstOrDefault(u => u.Id == newCourse.Id);
        var lessons = _dbContext.Lessons.Where(u => u.CourseId == newCourse.Id).ToList();

        if (oldCourses is null || !lessons.Any())
        {
            return false;
        }

        if (lessons.All(u => !u.Marked) && oldCourses.DateOfStartCourse != newCourse.DateOfStartCourse)
        {
            _dbContext.RemoveRange(lessons);
            var days = GetDatesBetweenStartAndEndByDaysOfWeek(newCourse.DateOfStartCourse, newCourse.DateOfEndCourse,
                newCourse.DaysOfWeek);
            var newLessons = days.Select(u => new Lesson
            {
                CoachId = newCourse.CoachId,
                CourseId = newCourse.Id,
                DateAndTime = u.ToDateTime(newCourse.TimeOfLesson)
            }).ToList();

            _dbContext.Lessons.AddRange(newLessons);
            _dbContext.Courses.Update(newCourse);

            _dbContext.SaveChanges();
            return true;
        }

        return false;
    }

    public bool UpdateDayEndCourse(Course newCourse)
    {
        var oldCourses = _dbContext.Courses.AsNoTracking().FirstOrDefault(u => u.Id == newCourse.Id);

        if (oldCourses is null)
        {
            return false;
        }

        if (oldCourses.DateOfEndCourse == newCourse.DateOfEndCourse)
            return false;

        var notDoneLessons = _dbContext.Lessons.Where(u => !u.Marked);

        _dbContext.Lessons.RemoveRange(notDoneLessons);

        var lastLesson = _dbContext.Lessons.OrderBy(u => u.DateAndTime).LastOrDefault();

        var date = lastLesson is not null
            ? DateOnly.FromDateTime(lastLesson.DateAndTime.AddDays(1))
            : oldCourses.DateOfStartCourse;

        var days = GetDatesBetweenStartAndEndByDaysOfWeek(date, newCourse.DateOfEndCourse, newCourse.DaysOfWeek);
        var newLessons = days.Select(u => new Lesson
        {
            CoachId = newCourse.CoachId,
            CourseId = newCourse.Id,
            DateAndTime = u.ToDateTime(newCourse.TimeOfLesson)
        }).ToList();

        _dbContext.Lessons.AddRange(newLessons);
        _dbContext.Courses.Update(newCourse);
        _dbContext.SaveChanges();

        return true;
    }

    public bool UpdateCoachCourse(Course course)
    {
        var lessons = _dbContext.Lessons.Where(u => u.CourseId == course.Id && !u.Marked).ToList();
        var oldCourses = _dbContext.Courses.FirstOrDefault(u => u.Id == course.Id);

        if (oldCourses is null || !lessons.Any())
        {
            return false;
        }

        foreach (var lesson in lessons)
        {
            lesson.CoachId = course.CoachId;
        }

        oldCourses.CoachId = course.CoachId;

        _dbContext.SaveChanges();

        return true;
    }

    public bool UpdateTimeOfCourse(Course course)
    {
        var oldCourses = _dbContext.Courses.FirstOrDefault(u => u.Id == course.Id);
        var lessons = _dbContext.Lessons.Where(u => u.CourseId == course.Id && !u.Marked).ToList();

        if (oldCourses is null || !lessons.Any())
        {
            return false;
        }

        oldCourses.TimeOfLesson = course.TimeOfLesson;

        foreach (var lesson in lessons)
        {
            lesson.DateAndTime = DateOnly.FromDateTime(lesson.DateAndTime).ToDateTime(course.TimeOfLesson);
        }

        _dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int entityId)
    {
        throw new NotImplementedException();
    }

    public List<Course> GetAll()
    {
        return _dbContext.Courses.ToList();
    }

    public Course? GetById(int id)
    {
        return _dbContext.Courses.FirstOrDefault(u => u.Id == id);
    }

    public int? Create(Course entity)
    {
        throw new NotImplementedException();
    }

    public bool AddStudentInCourse(int courseId, int[] studentsId)
    {
        // Получаем текущие связи студентов с курсом
        var oldStudentCourses = _dbContext.StudentCourses
            .Where(sc => sc.CourseId == courseId)
            .ToList();

        var notMarkedLessons = _dbContext.Lessons.Where(sc => sc.CourseId == courseId && !sc.Marked).ToList();

        var oldStudents = oldStudentCourses.Select(sc => sc.StudentId).ToList();

        var deletedStudentIds = oldStudents.Except(studentsId).ToList();

        var addedStudentIds = studentsId.Except(oldStudents).ToList();

        // Удаляем связи с студентами, которых больше не должно быть
        var toRemove = oldStudentCourses.Where(sc => deletedStudentIds.Contains(sc.StudentId));
        _dbContext.StudentCourses.RemoveRange(toRemove);

        // Добавляем новые связи
        var newStudentCourses = addedStudentIds.Select(id => new StudentCourse
            {
                StudentId = id,
                CourseId = courseId,
                Status = StudentCourse.StudentStatus.NotEngaged
            })
            .ToList();


        _dbContext.StudentCourses.AddRange(newStudentCourses);


        foreach (var lesson in notMarkedLessons)
        {
            var newStudentLessons = newStudentCourses.Select(sc => new StudentLesson
            {
                StudentId = sc.StudentId,
                Status = StudentLesson.StudentStatus.Online,
                LessonId = lesson.Id
            }).ToList();

            _dbContext.StudentLessons.AddRange(newStudentLessons);
        }

        _dbContext.SaveChanges();

        return true;
    }

    public List<GetLessonsInDayResponce> GetLessonsInDays(int coachId, int day)
    {
        DateTime today = DateTime.Now;
        int currentDayOfWeek = (int)today.DayOfWeek;
        if (currentDayOfWeek == 0) currentDayOfWeek = 7;

        int daysDifference = day - currentDayOfWeek;
        DateTime targetDate = today.AddDays(daysDifference);

        return _dbContext.Lessons.Include(u => u.Students)
            .Include(u => u.Course)
            .Where(u => u.CoachId == coachId && u.DateAndTime.Date == targetDate.Date)
            .Select(lesson => new GetLessonsInDayResponce(lesson.Id, TimeOnly.FromDateTime(lesson.DateAndTime),
                lesson.Course.Title, lesson.Students.Count)).ToList();
    }

    public bool Update(Course entity)
    {
        throw new NotFiniteNumberException();
    }
}