using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager.Features.Lessons.GetAll
{

    public record GetAllLessonsQuery(
        bool Desc,
        int? CoachId,
        int? CourseId,
        DateTime? StartDate,
        DateTime? EndDate,
        string Sort = ""
        ) : IQuery<List<LessonResponse>>;

    public class GetAllLessonsQueryHandler : IRequestHandler<GetAllLessonsQuery, List<LessonResponse>>
    {
        private readonly RepositoryBase<Lesson> _repository;

        private static class SortType
        {
            public const string Date = "date";
            public const string Course = "course";
            public const string Coach = "coach";
        }

        public GetAllLessonsQueryHandler(RepositoryBase<Lesson> rep)
        {
            _repository = rep;
        }
        public async Task<List<LessonResponse>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query;

            if (request.CourseId != null)
            {
                query = query.Where(x => x.CourseId == request.CourseId);
            }

            if (request.CoachId != null)
            {
                query = query.Where(x => x.CoachId == request.CoachId);
            }

            if (request.StartDate != null && request.EndDate != null)
            {
                query = query.Where(x => x.DateAndTime >= request.StartDate && x.DateAndTime <= request.EndDate);
            }

            switch (request.Sort)
            {
                case SortType.Date:
                    query = request.Desc ? query.OrderByDescending(x => x.DateAndTime) : query.OrderBy(x => x.DateAndTime);
                    break;
                case SortType.Course:
                    query = request.Desc ? query.OrderByDescending(x => x.CourseId) : query.OrderBy(x => x.CourseId);
                    break;
                case SortType.Coach:
                    query = request.Desc ? query.OrderByDescending(x => x.CoachId) : query.OrderBy(x => x.CoachId);
                    break;
                default:
                    query = request.Desc ? query.OrderByDescending(x => x.DateAndTime) : query.OrderBy(x => x.DateAndTime);
                    break;
            }

            return await query.Select(x => new LessonResponse(x.Id, x.CourseId, x.DateAndTime, x.CoachId, x.Students.Count)).ToListAsync(cancellationToken);
        }
    }
}
