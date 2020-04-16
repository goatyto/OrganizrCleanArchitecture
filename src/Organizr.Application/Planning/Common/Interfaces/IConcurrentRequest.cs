using MediatR;

namespace Organizr.Application.Planning.Common.Interfaces
{
    public interface IConcurrentRequest<out TResponse> : IRequest<TResponse>
    {
        
    }

    public interface IConcurrentRequest : IConcurrentRequest<Unit>, IRequest
    {

    }
}