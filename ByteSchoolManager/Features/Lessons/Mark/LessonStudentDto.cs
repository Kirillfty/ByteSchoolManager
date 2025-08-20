using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Lessons.Mark;

public record LessonStudentDto(int StudentId, StudentLesson.StudentStatus Status);