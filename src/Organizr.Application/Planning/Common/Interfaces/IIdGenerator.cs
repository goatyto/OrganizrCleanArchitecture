using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.Common.Interfaces
{
    public interface IIdGenerator
    {
        Guid GenerateNext<TEntity>() where TEntity : IAggregateRoot;
    }
}