using System;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public interface IUserGroupRepository: IRepository<UserGroup>
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        void Add(UserGroup userGroup);
        void Update(UserGroup userGroup);
    }
}