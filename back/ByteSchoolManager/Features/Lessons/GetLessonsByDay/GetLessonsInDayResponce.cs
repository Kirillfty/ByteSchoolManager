namespace ByteSchoolManager.Features.Lessons.GetLessonsByDay
{
    public record GetLessonsInDayResponce(int Id, TimeOnly Time, string Title, int Students);
}
