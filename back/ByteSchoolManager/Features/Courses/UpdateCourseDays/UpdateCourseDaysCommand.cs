using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Features.Courses.UpdateCourseDays;

public record UpdateCourseDaysCommand(int CourseId, int[] Days) : ICommand<string>;

public class UpdateDaysOnCourseCommandHandler : IRequestHandler<UpdateCourseDaysCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryBase<Course> _courseRepository;

    public UpdateDaysOnCourseCommandHandler(
        RepositoryBase<Course> courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateCourseDaysCommand request, CancellationToken ct)
    {
        var courseDaysOfWeek = DaysHelper.GetDayOfWeek(request.Days.Select(x => (DayOfWeek)x));
        var course = await _courseRepository.FirstOrDefaultAsync(
            c => c.Id == request.CourseId,
            includes: [x => x.Lessons],
            cancellationToken: ct,
            tracking: true
        );

        if (course is null)
        {
            throw new NotFoundException(nameof(Course), request.CourseId.ToString());
        }

        course.DaysOfWeek = courseDaysOfWeek;

        var notMarkedLessons = course.Lessons.Where(l => !l.Marked).ToList();

        var lastLesson = course.Lessons.Where(x => x.Marked).OrderBy(x => x.DateAndTime).LastOrDefault();

        var date = lastLesson is not null
            ? DateOnly.FromDateTime(lastLesson.DateAndTime).AddDays(1)
            : course.DateOfStartCourse;

        var lessonDates = GetDatesBetweenStartAndEndByDaysOfWeek(date, course.DateOfEndCourse, course.DaysOfWeek);

        List<Lesson> newLessons = new List<Lesson>();
        foreach (var lessonDate in lessonDates)
        {
            newLessons.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DateAndTime = lessonDate.ToDateTime(course.TimeOfLesson)
            });
        }

        course.Lessons.RemoveAll(l => notMarkedLessons.Any(x => x.Id == l.Id));
        course.Lessons.AddRange(newLessons);

        await _unitOfWork.SaveChangesAsync(ct);

        return $"The days of lessons in the ‘{course.Title}’ course have been successfully updated. " +
               $"{notMarkedLessons.Count} lessons have been deleted and {newLessons.Count} lessons have been added.";
    }

    private List<DateOnly> GetDatesBetweenStartAndEndByDaysOfWeek(DateOnly start, DateOnly end,
        Entities.Course.DayOfWeek daysOfWeek)
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
}