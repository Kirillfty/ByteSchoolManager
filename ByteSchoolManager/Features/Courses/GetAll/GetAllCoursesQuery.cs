using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Courses.GetAll;

public class GetAllCoursesQuery : IQuery<List<CourseDto>>;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
{
    private readonly RepositoryBase<Course> _repository;

    public GetAllCoursesQueryHandler(RepositoryBase<Course> repository)
    {
        _repository = repository;
    }

    public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.ListSelectionAsync(
            x => new CourseDto(x.Id, DaysHelper.GetDayNumbers(x.DaysOfWeek), x.TimeOfLesson, x.DateOfStartCourse,
                x.DateOfEndCourse, x.Title, x.CoachId,
                x.Students.Count, x.Lessons.Count), cancellationToken: cancellationToken);
    }
}