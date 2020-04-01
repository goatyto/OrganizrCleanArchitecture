﻿using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupMemberAdded : IDomainEvent
    {
        public UserGroupId UserGroupId { get; }
        public UserGroupMember UserGroupMember { get; }

        public UserGroupMemberAdded(UserGroupId userGroupId, UserGroupMember userGroupMember)
        {
            Guard.Against.Null(userGroupId, nameof(userGroupId));
            Guard.Against.Null(userGroupMember, nameof(userGroupMember));
            
            UserGroupId = userGroupId;
            UserGroupMember = userGroupMember;
        }
    }
}
