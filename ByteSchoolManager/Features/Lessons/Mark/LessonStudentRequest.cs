using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Lessons.Mark;

public record LessonStudentRequest(int StudentId, StudentLesson.StudentStatus Status);