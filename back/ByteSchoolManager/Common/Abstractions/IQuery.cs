using MediatR;

namespace ByteSchoolManager.Common.Abstractions;

public interface IQuery<out T> : IRequest<T>;