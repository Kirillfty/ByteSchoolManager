using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Features.Lessons.GetAll
{
    public record LessonDto(
        int Id,

        int CourseId,

        DateTime DateAndTime,

        int CoachId,

        int Studentss
    );
}




