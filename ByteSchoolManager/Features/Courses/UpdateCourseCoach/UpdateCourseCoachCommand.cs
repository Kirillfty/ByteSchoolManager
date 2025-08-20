using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.UpdateCourseCoach;

public record UpdateCourseCoachCommand(int CourseId, int CoachId) : ICommand<string>;

public class UpdateCourseCoachCommandHandler : IRequestHandler<UpdateCourseCoachCommand, string>
{
    private readonly RepositoryBase<Course> _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCoachCommandHandler(RepositoryBase<Course> courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdateCourseCoachCommand request, CancellationToken ct)
    {
        var course = await _courseRepository.FirstOrDefaultAsync(
            predicate: u => u.Id == request.CourseId,
            includes: [x => x.Lessons],
            tracking: true,
            cancellationToken: ct
        );

        if (course is null)
        {
            throw new NotFoundException(nameof(Course), request.CourseId.ToString());
        }

        course.CoachId = request.CoachId;

        var notMarkedLessons = course.Lessons.Where(u => !u.Marked);

        foreach (var lesson in notMarkedLessons)
        {
            lesson.CoachId = course.CoachId;
        }

        await _unitOfWork.SaveChangesAsync(ct);

        return $"The coach for the '{course.Title}' course has been successfully changed.";
    }
}