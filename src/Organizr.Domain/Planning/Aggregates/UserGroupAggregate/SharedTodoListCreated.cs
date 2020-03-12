using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class SharedTodoListCreated : IDomainEvent
    {
        public Guid UserGroupId { get; }
        public Guid TodoListId { get; }

        public SharedTodoListCreated(Guid userGroupId, Guid todoListId)
        {
            Guard.Against.Default(userGroupId, nameof(userGroupId));
            Guard.Against.Default(todoListId, nameof(todoListId));
            
            UserGroupId = userGroupId;
            TodoListId = todoListId;
        }
    }
}
