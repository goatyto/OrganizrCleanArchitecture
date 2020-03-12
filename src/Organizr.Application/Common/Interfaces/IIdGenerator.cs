using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.Interfaces
{
    public interface IIdGenerator
    {
        Guid GenerateNext<TEntity>() where TEntity : Entity<Guid>;
    }
}