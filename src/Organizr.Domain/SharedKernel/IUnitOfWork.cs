using System;
using System.Threading;
using System.Threading.Tasks;

namespace Organizr.Domain.SharedKernel
{
    public interface IUnitOfWork: IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}