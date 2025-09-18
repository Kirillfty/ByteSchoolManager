using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Common.Exceptions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace ByteSchoolManager.Features.Courses.Delete
{
    public class DeleteCourseCommand : IRequest<Unit>
    {
        [Required]
        public int Id { get; set; }
    }

    // Обработчик команды
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCourseCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);


            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }


}
