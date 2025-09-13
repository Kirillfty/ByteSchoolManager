using ByteSchoolManager.Common.Abstractions;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using MediatR;
using System.Windows.Input;

namespace ByteSchoolManager.Features.Courses.Delete
{
    public record DeleteCourseCommond(int id) : ICommand<string>;

    //public class DeleteCourseCommondHandler : IRequestHandler<DeleteCourseCommond, string>
    //{
    //    private readonly RepositoryBase<Course> _courseRepository;
    //    private readonly IUnitOfWork _unitOfWork;
    //    public DeleteCourseCommondHandler(RepositoryBase<Course> courseRepository, IUnitOfWork unitOfWork)
    //    {
    //        _courseRepository = courseRepository;
    //        _unitOfWork = unitOfWork;
    //    }
    //    public Task<string> Handle(DeleteCourseCommond request, CancellationToken ct)
    //    {
    //        var course =  _courseRepository.FirstOrDefaultAsync(
    //         predicate: u => u.Id == request.id,
    //         includes: [x => x.Lessons],
    //         tracking: true,
    //         cancellationToken: ct
    //         );


            
    //    }
    //}


}
