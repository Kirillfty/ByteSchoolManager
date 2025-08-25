using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Features.Courses.EditCourseStudents;

public record EditCourseStudentsCommand(int CourseId, CourseStudentDto[] Students) : ICommand<string>;

public class EditCourseStudentsCommandHandler : IRequestHandler<EditCourseStudentsCommand, string>
{
    private readonly RepositoryBase<Course> _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditCourseStudentsCommandHandler(RepositoryBase<Course> courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(EditCourseStudentsCommand request, CancellationToken cancellationToken)
    {
        //Course => StudentCourses => Student => StudentLessons => Lesson
        var course = await _courseRepository.Query
            .Include(x => x.Students)
            .ThenInclude(x => x.Student)
            .ThenInclude(x => x.Lessons)
            .ThenInclude(x => x.Lesson)
            .FirstOrDefaultAsync(x => x.Id == request.CourseId, cancellationToken);

        if (course is null)
        {
            throw new NotFoundException(nameof(Course), request.CourseId.ToString());
        }

        var oldStudentIds = course.Students.Select(x => x.StudentId).Distinct().ToHashSet();
        var newStudentIds = request.Students.Select(x => x.StudentId).Distinct().ToHashSet();

        var addedStudentIds = new HashSet<int>(newStudentIds);
        addedStudentIds.ExceptWith(oldStudentIds);

        var removedStudentIds = new HashSet<int>(oldStudentIds);
        removedStudentIds.ExceptWith(newStudentIds);

        var unchangedStudentIds = new HashSet<int>(oldStudentIds);
        unchangedStudentIds.IntersectWith(newStudentIds);

        //TODO: Check user
        course.Students.RemoveAll(x => removedStudentIds.Contains(x.StudentId));
        //TODO: Check user
        course.Students.AddRange(request.Students.Where(x => addedStudentIds.Contains(x.StudentId)).Select(x =>
            new StudentCourse
            {
                StudentId = x.StudentId,
                CourseId = request.CourseId,
                CourseStatus = x.CourseStatus
            }));

        foreach (var studentCourse in course.Students.Where(x => unchangedStudentIds.Contains(x.StudentId)))
        {
            studentCourse.CourseStatus = request.Students.First(x => x.StudentId == studentCourse.StudentId).CourseStatus;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return $"{removedStudentIds.Count} students were removed from the course, " +
               $"{addedStudentIds.Count} students were added, and " +
               $"{unchangedStudentIds.Count} students had their status updated.";
    }
}