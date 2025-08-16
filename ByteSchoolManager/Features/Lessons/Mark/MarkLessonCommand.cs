using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Lessons.Mark;

public record MarkLessonCommand(int LessonId, LessonStudentRequest[] Students) : ICommand<string>;

public record StudentLessonRequestHandler : IRequestHandler<MarkLessonCommand, string>
{
    private readonly RepositoryBase<Lesson> _lessonRepository;
    private readonly RepositoryBase<StudentCourse> _studentCourseRepository;
    private readonly RepositoryBase<StudentLesson> _studentLessonRepository;

    public StudentLessonRequestHandler(
        RepositoryBase<Lesson> lessonRepository,
        RepositoryBase<StudentCourse> studentCourseRepository,
        RepositoryBase<StudentLesson> studentLessonRepository)
    {
        _lessonRepository = lessonRepository;
        _studentCourseRepository = studentCourseRepository;
        _studentLessonRepository = studentLessonRepository;
    }

    public async Task<string> Handle(MarkLessonCommand request, CancellationToken ct)
    {
        var lesson = await _lessonRepository.FirstOrDefaultAsync(x => x.Id == request.LessonId, ct: ct);

        if (lesson is null)
            return "Selected lesson not found";

        var studentIds = request.Students.Select(x => x.StudentId).ToArray();

        var lessonStudentIds = await _studentCourseRepository.ListSelectionAsync(
            x => x.Student.Id,
            x => studentIds.Contains(x.StudentId) && x.CourseId == lesson.CourseId && x.Status == StudentCourse.StudentStatus.Engaged,
            ct: ct);

        foreach (var studentId in studentIds)
        {
            if (!lessonStudentIds.Contains(studentId))
            {
                return $"The student[{studentId}] is not studying in this course[{lesson.CourseId}]";
            }
        }

        var studentLessons = request.Students.Select(x => new StudentLesson
        {
            LessonId = lesson.Id,
            StudentId = x.StudentId,
            Status = x.Status
        });

        await _studentLessonRepository.AddRangeAsync(studentLessons, ct);
        
        await _studentLessonRepository.SaveChangesAsync(ct);
        
        return "Lesson successfully marked";
    }
}