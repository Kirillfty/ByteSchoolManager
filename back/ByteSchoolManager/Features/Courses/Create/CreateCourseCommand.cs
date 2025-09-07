using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.Create;

public class CreateCourseCommand : ICommand<string>
{
    public int[] Days { get; set; }
    public TimeOnly TimeOfLesson { get; set; }
    public DateOnly DateOfStartCourse { get; set; }
    public DateOnly DateOfEndCourse { get; set; }
    public string Title { get; set; }
    public int CoachId { get; set; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryBase<Course> _courseRepository;

    public CreateCourseCommandHandler(
        RepositoryBase<Course> courseRepository,
        RepositoryBase<Lesson> lessonRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        Course course = new Course
        {
            TimeOfLesson = request.TimeOfLesson,
            DateOfEndCourse = request.DateOfEndCourse,
            DateOfStartCourse = request.DateOfStartCourse,
            DaysOfWeek = DaysHelper.GetDayOfWeek(request.Days.Select(i => (DayOfWeek)i)),
            Title = request.Title,
            CoachId = request.CoachId
        };
        
        var lessons = new List<Lesson>();
        var dates = GetDatesBetweenStartAndEndByDaysOfWeek(course.DateOfStartCourse, course.DateOfEndCourse,
            course.DaysOfWeek);

        foreach (var date in dates)
        {
            lessons.Add(new Lesson
            {
                CourseId = course.Id,
                CoachId = course.CoachId,
                DateAndTime = date.ToDateTime(course.TimeOfLesson)
            });
        }

        course.Lessons = lessons;
        await _courseRepository.AddAsync(course, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        return $"Course[{course.Id}] '{course.Title}' created";
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