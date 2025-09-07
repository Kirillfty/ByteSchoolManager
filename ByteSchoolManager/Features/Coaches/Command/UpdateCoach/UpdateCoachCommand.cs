using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;

namespace ByteSchoolManager.Features.Coaches.Command.UpdateCoach
{
    public record UpdateCoachCommand(int CoachId, string Name, string Phone, string Telegram) : ICommand<string>;

    public class UpdateCoachCommandHandler : IRequestHandler<UpdateCoachCommand, string>
    {
        private readonly RepositoryBase<Coach> _coachRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCoachCommandHandler(RepositoryBase<Coach> coachRepository, IUnitOfWork unitOfWork)
        {
            _coachRepository = coachRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
        {
            var coach = await _coachRepository.FirstOrDefaultAsync(
                x => request.CoachId == x.Id,
                cancellationToken: cancellationToken,
                tracking: true
            );

            if (coach is null)
            {
                throw new NotFoundException(nameof(Coach), request.CoachId.ToString());
            }

            coach.Name = request.Name;
            coach.PhoneNumber = request.Phone;
            coach.Telegram = request.Telegram;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return "Coach successfully updated";
        }
    }
}