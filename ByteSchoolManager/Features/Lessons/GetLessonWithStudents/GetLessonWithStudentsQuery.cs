using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Features.Lessons.GetLessonWithStudents;

public record GetLessonWithStudentsQuery(int LessonId) : IQuery<LessonWithStudentsDto>;

public class GetLessonByIdWithStudentsQueryHandler : IRequestHandler<GetLessonWithStudentsQuery, LessonWithStudentsDto>
{
    private readonly RepositoryBase<Lesson> _lessonRepository;

    public GetLessonByIdWithStudentsQueryHandler(RepositoryBase<Lesson> lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<LessonWithStudentsDto> Handle(GetLessonWithStudentsQuery request, CancellationToken ct)
    {
        var lesson = await _lessonRepository.Query
            .AsNoTracking()
            .Include(x => x.Course)
            .ThenInclude(x => x.Students)
            .ThenInclude(x => x.Student)
            .FirstOrDefaultAsync(x => x.Id == request.LessonId, cancellationToken: ct);

        if (lesson is null)
        {
            throw new NotFoundException(nameof(Lesson), request.LessonId.ToString());
        }

        return new LessonWithStudentsDto(lesson.Id,
            lesson.Course.Students.Select(x => new LessonStudentDto(x.StudentId, x.Student.Name)).ToList());
    }
}