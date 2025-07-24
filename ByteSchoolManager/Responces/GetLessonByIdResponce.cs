using ByteSchoolManager.Entities;

namespace ByteSchoolManager.Responces
{
    public record GetLessonByIdResponce(int Id, List<GetLessonByIdResponceStudent> Students);
    public record GetLessonByIdResponceStudent(int Id,string Name);
}
