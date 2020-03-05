using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Common.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public Guid ResourceId { get; }
        public string UserId { get; }

        public AccessDeniedException(Guid resourceId, string userId, Exception innerException = null) : base(
            $"Access to resource ${resourceId} for user {userId} is denied.", innerException)
        {
            ResourceId = resourceId;
            UserId = userId;
        }
    }
}
