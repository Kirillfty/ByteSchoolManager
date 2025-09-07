namespace ByteSchoolManager.Features.Lessons.GetLessonWithStudents
{
    public record LessonWithStudentsDto(int Id, List<LessonStudentDto> Students);
    public record LessonStudentDto(int Id,string Name);
}
