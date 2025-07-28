namespace ByteSchoolManager.Features.Courses.GetAll;

public record CourseDto(
    int Id,
    int[] DaysOfWeek,
    TimeOnly Time,
    DateOnly StartDate,
    DateOnly EndDate,
    string Title,
    int CoachId,
    int StudentCount,
    int LessonsCount);