using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.UpdateCourseTime;

public record UpdateCourseTimeCommand(int CourseId, TimeOnly Time) : ICommand<string>;

public class UpdateCourseTimeCommandHandler : IRequestHandler<UpdateCourseTimeCommand, string>
{
    private readonly RepositoryBase<Course> _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseTimeCommandHandler(RepositoryBase<Course> courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateCourseTimeCommand request, CancellationToken ct)
    {
        var course = await _courseRepository.FirstOrDefaultAsync(
            x => x.Id == request.CourseId,
            includes: [x => x.Lessons],
            cancellationToken: ct,
            tracking: true
        );

        if (course is null)
        {
            throw new NotFoundException(nameof(Course), request.CourseId.ToString());
        }

        if (course.TimeOfLesson == request.Time)
        {
            throw new BadRequestException("The current time corresponds to the updated time.");
        }

        var notMarkedLessons = course.Lessons.Where(x => !x.Marked).ToArray();
        
        if (notMarkedLessons.Length == 0)
        {
            throw new BadRequestException("You cannot update the course time if all lessons have been marked.");
        }
        
        course.TimeOfLesson = request.Time;

        foreach (var lesson in notMarkedLessons)
        {
            var date = DateOnly.FromDateTime(lesson.DateAndTime);
            lesson.DateAndTime = date.ToDateTime(request.Time);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        
        return $"The '{course.Title}' course and the time for the {notMarkedLessons.Length} lessons have been updated.";
    }
}