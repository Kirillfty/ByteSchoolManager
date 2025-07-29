using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Lessons.GetAll
{

    public class GetAllLessonsQuery : IQuery<List<LessonDto>>;



    public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, List<LessonDto>>
    {
        private readonly RepositoryBase<Lesson> _repository;

        public GetAllLessonsQueryHandler(RepositoryBase<Lesson> rep) { 
            _repository = rep;
        }
        public async Task<List<LessonDto>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ListSelectionAsync(
                x => new LessonDto(x.Id,x.CoachId,x.DateAndTime,x.CoachId,x.Students)
                );
        }
    }
}
