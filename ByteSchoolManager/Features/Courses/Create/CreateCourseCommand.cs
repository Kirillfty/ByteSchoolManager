using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.Create;

public class CreateCourseCommand : ICommand<int>
{
    public int[] Days { get; set; }
    public TimeOnly TimeOfLesson { get; set; }
    public DateOnly DateOfStartCourse { get; set; }
    public DateOnly DateOfEndCourse { get; set; }
    public string Title { get; set; }
    public int CoachId { get; set; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
{
    private readonly RepositoryBase<Course> _courseRepository;
    private readonly RepositoryBase<Lesson> _lessonRepository;

    public CreateCourseCommandHandler(RepositoryBase<Course> courseRepository, RepositoryBase<Lesson> lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        Course course = new Course
        {
            TimeOfLesson = request.TimeOfLesson,
            DateOfEndCourse = request.DateOfEndCourse,
            DateOfStartCourse = request.DateOfStartCourse,
            DaysOfWeek = DaysHelper.GetDayOfWeek(request.Days.Select(i => (DayOfWeek)i).ToArray()),
            Title = request.Title,
            CoachId = request.CoachId
        };
        
        await _courseRepository.AddAsync(course, cancellationToken);

        await _courseRepository.SaveChangesAsync(cancellationToken);
        
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

        await _lessonRepository.AddRangeAsync(lessons);

        await _lessonRepository.SaveChangesAsync();

        return course.Id;
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