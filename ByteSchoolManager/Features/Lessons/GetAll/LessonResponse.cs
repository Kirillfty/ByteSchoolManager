using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Lessons.GetAll
{
    public record LessonResponse(
        int Id,
        int CourseId,
        DateTime DateAndTime,
        int CoachId,
        int Students
    );
}
