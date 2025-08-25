using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Courses.EditCourseStudents;

public record CourseStudentDto(int StudentId, StudentCourse.StudentCourseStatus CourseStatus);