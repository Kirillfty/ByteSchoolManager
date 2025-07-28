using MediatR;

namespace ByteSchoolManager.Common.Abstractions;

public interface ICommand<out T> : IRequest<T>;