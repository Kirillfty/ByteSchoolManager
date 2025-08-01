namespace ByteSchoolManager.Responces
{
    public record GetLessonsInDayResponce(int lessonId, TimeOnly time, string name, int students);
}
