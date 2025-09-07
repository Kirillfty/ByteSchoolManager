using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Coaches.Command.CreateCoach
{
    public record CreateCoachCommand(int UserId, string Name, string Phone, string Telegram) : ICommand<string>;

    public class CreateCoachCommandHandler : IRequestHandler<CreateCoachCommand, string>
    {
        private readonly RepositoryBase<User> _userRepository;
        private readonly RepositoryBase<Coach> _coachRepository;

        public CreateCoachCommandHandler(RepositoryBase<User> userRepository, RepositoryBase<Coach> coachRepository)
        {
            _userRepository = userRepository;
            _coachRepository = coachRepository;
        }

        public async Task<string> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(
                x => request.UserId == x.Id,
                cancellationToken: cancellationToken
            );

            if (user is null)
            {
                throw new NotFoundException(nameof(User), request.UserId.ToString());
            }

            await _coachRepository.AddAsync(
                new Coach
                {
                    Name = request.Name,
                    UserId = user.Id,
                    PhoneNumber = request.Phone,
                    Telegram = request.Telegram
                },
                cancellationToken);

            return "New successfully coach created";
        }
    }
}