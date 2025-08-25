using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.UpdateCourseDates;

public record UpdateCourseDatesCommand(int CourseId, DateOnly? StartDate, DateOnly? EndDate) : ICommand<string>;

public class UpdateCourseDatesCommandHandler : IRequestHandler<UpdateCourseDatesCommand, string>
{
    private readonly RepositoryBase<Course> _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseDatesCommandHandler(RepositoryBase<Course> courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateCourseDatesCommand request, CancellationToken ct)
    {
        if (!request.StartDate.HasValue && !request.EndDate.HasValue)
        {
            throw new BadRequestException("You must specify the start date or end date.");
        }

        var course = await _courseRepository.FirstOrDefaultAsync(
            u => u.Id == request.CourseId,
            includes: [x => x.Lessons],
            cancellationToken: ct,
            tracking: true
        );

        if (course is null)
        {
            throw new NotFoundException(nameof(Course), request.CourseId.ToString());
        }

        if (request.StartDate.HasValue && course.DateOfStartCourse == request.StartDate ||
            request.EndDate.HasValue && course.DateOfEndCourse == request.EndDate)
        {
            throw new BadRequestException("The current date corresponds to the updated date.");
        }

        var notMarkedLessons = course.Lessons.Where(x => !x.Marked).ToArray();

        if (request.StartDate.HasValue && notMarkedLessons.Length != course.Lessons.Count)
        {
            throw new BadRequestException(
                "You cannot update the course start date if at least one lesson has been marked.");
        }

        if (request.EndDate.HasValue && notMarkedLessons.Length == 0)
        {
            throw new BadRequestException("You cannot update the course end date if all lessons have been marked.");
        }

        course.DateOfStartCourse = request.StartDate ?? course.DateOfStartCourse;
        course.DateOfEndCourse = request.EndDate ?? course.DateOfEndCourse;

        course.Lessons.RemoveAll(x => !x.Marked);

        var lastMarkedLesson = course.Lessons.OrderBy(x => x.DateAndTime).FirstOrDefault();

        DateOnly newLessonsStartDate;

        if (lastMarkedLesson is null)
        {
            newLessonsStartDate = course.DateOfStartCourse;
        }
        else
        {
            newLessonsStartDate = DateOnly.FromDateTime(lastMarkedLesson.DateAndTime).AddDays(1);
        }

        var days = GetDatesBetweenStartAndEndByDaysOfWeek(newLessonsStartDate, course.DateOfEndCourse,
            course.DaysOfWeek);

        var newLessons = days.Select(d => new Lesson
        {
            CoachId = course.CoachId,
            CourseId = course.Id,
            DateAndTime = d.ToDateTime(course.TimeOfLesson)
        }).ToList();

        course.Lessons.AddRange(newLessons);

        await _unitOfWork.SaveChangesAsync(ct);

        return $"The '{course.Title}' course and the dates for the {newLessons.Count} lessons have been updated.";
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
}