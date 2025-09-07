using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Lessons.MarkLesson;

public record MarkLessonCommand(int LessonId, MarkLessonStudentDto[] Students) : ICommand<string>;

public record StudentLessonRequestHandler : IRequestHandler<MarkLessonCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryBase<Lesson> _lessonRepository;
    private readonly RepositoryBase<StudentCourse> _studentCourseRepository;
    private readonly RepositoryBase<StudentLesson> _studentLessonRepository;

    public StudentLessonRequestHandler(
        RepositoryBase<Lesson> lessonRepository,
        RepositoryBase<StudentCourse> studentCourseRepository,
        RepositoryBase<StudentLesson> studentLessonRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonRepository = lessonRepository;
        _studentCourseRepository = studentCourseRepository;
        _studentLessonRepository = studentLessonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(MarkLessonCommand request, CancellationToken ct)
    {
        var lesson = await _lessonRepository.FirstOrDefaultAsync(x => x.Id == request.LessonId, tracking: true, cancellationToken: ct);

        if (lesson is null)
            return "Selected lesson not found";

        var studentIds = request.Students.Select(x => x.StudentId).ToArray();

        var lessonStudentIds = await _studentCourseRepository.ListSelectionAsync(
            x => x.Student.Id,
            x => studentIds.Contains(x.StudentId) && x.CourseId == lesson.CourseId && x.CourseStatus == StudentCourse.StudentCourseStatus.Engaged,
            cancellationToken: ct);

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
            LessonStatus = x.LessonStatus
        });

        await _studentLessonRepository.AddRangeAsync(studentLessons, ct);

        lesson.Marked = true;
        
        await _unitOfWork.SaveChangesAsync(ct);
        
        return "Lesson successfully marked";
    }
}