using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Lessons.GetAll
{

    public class GetAllLessonsQuery : IQuery<List<LessonResponse>>;

    public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, List<LessonResponse>>
    {
        private readonly RepositoryBase<Lesson> _repository;

        public GetAllLessonsQueryHandler(RepositoryBase<Lesson> rep)
        {
            _repository = rep;
        }
        public async Task<List<LessonResponse>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ListSelectionAsync(x => new LessonResponse(x.Id, x.CoachId, x.DateAndTime, x.CoachId, x.Students.Count));
        }
    }
}
