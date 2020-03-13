using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupEdited : IDomainEvent
    {
        public UserGroup UserGroup { get; }

        public UserGroupEdited(UserGroup userGroup)
        {
            Guard.Against.Null(userGroup, nameof(userGroup));

            UserGroup = userGroup;
        }
    }
}
