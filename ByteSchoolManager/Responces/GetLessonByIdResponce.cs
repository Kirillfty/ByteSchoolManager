using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Responces
{
    public record GetLessonByIdResponce(int lessonId, List<Student> students);
    
}
