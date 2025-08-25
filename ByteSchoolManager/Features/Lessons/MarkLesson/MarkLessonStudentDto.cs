using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Lessons.MarkLesson;

public record MarkLessonStudentDto(int StudentId, StudentLesson.StudentLessonStatus LessonStatus);