using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Lessons.GetLessonsByDay
{
    public record GetLessonsByDayQuery(int Day, int UserId) : IQuery<List<GetLessonsInDayResponce>>;

    public class GetLessonsByDayQueryHandler : IRequestHandler<GetLessonsByDayQuery, List<GetLessonsInDayResponce>>
    {
        private readonly LessonBaseRepository _lessonRepository;
        private readonly CoachBaseRepository _coachRepository;

        public GetLessonsByDayQueryHandler(LessonBaseRepository lessonRepository, CoachBaseRepository coachRepository)
        {
            _lessonRepository = lessonRepository;
            _coachRepository = coachRepository;
        }

        public async Task<List<GetLessonsInDayResponce>> Handle(GetLessonsByDayQuery request, CancellationToken cancellationToken)
        {
            var coachId = await _coachRepository.FirstOrDefaultSelectionAsync(x => x.Id, x => x.UserId == request.UserId);

            DateTime today = DateTime.Now;
            int currentDayOfWeek = (int)today.DayOfWeek;
            if (currentDayOfWeek == 0) currentDayOfWeek = 7;

            int daysDifference = request.Day - currentDayOfWeek;
            DateTime targetDate = today.AddDays(daysDifference);

            return await _lessonRepository.ListSelectionAsync(
                selector: lesson => new GetLessonsInDayResponce(lesson.Id, TimeOnly.FromDateTime(lesson.DateAndTime), lesson.Course.Title, lesson.Students.Count),
                predicate: u => u.CoachId == coachId && u.DateAndTime.Date == targetDate.Date,
                includes: [u => u.Students, u => u.Course]
            );
        }
    }
}
