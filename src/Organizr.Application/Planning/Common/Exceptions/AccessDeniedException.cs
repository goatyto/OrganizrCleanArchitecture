﻿using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.Common.Exceptions
{
    public class AccessDeniedException<TResource> : Exception where TResource : IAggregateRoot
    {
        public object ResourceId { get; }
        public string UserId { get; }

        public AccessDeniedException(object resourceId, string userId, Exception innerException = null) : base(
            $"Access to resource of type {nameof(TResource)} with id ${resourceId} for user {userId} is denied.",
            innerException)
        {
            ResourceId = resourceId;
            UserId = userId;
        }
    }
}
