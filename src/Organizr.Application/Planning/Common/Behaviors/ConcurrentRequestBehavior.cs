using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.SharedKernel;
using Polly;

namespace Organizr.Application.Planning.Common.Behaviors
{
    public class ConcurrentRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IConcurrentRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            return Policy.Handle<TransientFailureException>().Retry(3).Execute(() => next());
        }
    }
}