using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Coaches.Query.GetAllCoaches;

public record GetAllCoachesQuery() : IQuery<IReadOnlyList<CoachDto>>;

public class GetAllCoachesQueryHandler : IRequestHandler<GetAllCoachesQuery, IReadOnlyList<CoachDto>>
{
    private readonly RepositoryBase<Coach> _coachRepository;

    public GetAllCoachesQueryHandler(RepositoryBase<Coach> coachRepository)
    {
        _coachRepository = coachRepository;
    }

    public async Task<IReadOnlyList<CoachDto>> Handle(GetAllCoachesQuery request, CancellationToken cancellationToken)
    {
        return await _coachRepository.ListSelectionAsync(x =>
                new CoachDto(x.Id,
                    x.User.Login,
                    x.Name,
                    x.PhoneNumber,
                    x.Telegram),
            cancellationToken: cancellationToken);
    }
}