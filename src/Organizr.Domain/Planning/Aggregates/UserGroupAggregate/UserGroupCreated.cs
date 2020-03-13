using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupCreated : IDomainEvent
    {
        public UserGroup UserGroup { get; }

        public UserGroupCreated(UserGroup userGroup)
        {
            Guard.Against.Null(userGroup, nameof(userGroup));

            UserGroup = userGroup;
        }
    }
}
