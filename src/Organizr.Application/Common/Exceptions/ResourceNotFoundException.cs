using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.Exceptions
{
    public class NotFoundException<TEntity> : Exception where TEntity : IAggregateRoot
    {
        public object Id { get; }

        public NotFoundException(object id, Exception innerException = null) : base($"Entity of type {nameof(TEntity)} with id {id} was not found.", innerException)
        {
            Id = id;
        }
    }

    public class ResourceNotFoundException : Exception
    {
        public Guid Id { get; }

        public ResourceNotFoundException(Guid id, Exception innerException = null) : base($"Resource with id {id} was not found.", innerException)
        {
            Id = id;
        }
    }
}
