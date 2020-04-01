using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Infrastructure.Persistence.Configuration
{
    public class UserGroupIdConverter : ValueConverter<UserGroupId, Guid>
    {
        public UserGroupIdConverter() : base(userGroupId => userGroupId.Id, id => new UserGroupId(id))
        {
        }
    }
}
