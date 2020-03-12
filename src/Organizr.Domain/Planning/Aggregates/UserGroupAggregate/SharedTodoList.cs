using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class SharedTodoList : CompositeKeyEntity
    {
        public Guid UserGroupId { get; }
        public Guid TodoListId { get; }
        public bool IsDeleted { get; private set; }

        internal SharedTodoList(Guid userGroupId, Guid todoListId)
        {
            UserGroupId = userGroupId;
            TodoListId = todoListId;
        }

        internal void Delete()
        {
            IsDeleted = true;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserGroupId;
            yield return TodoListId;
        }
    }
}
