using System;

namespace Organizr.Domain.Entities.TodoListAggregate
{
    public class TodoItem: OrderedListItemBase
    {
        public DateTime? DueDate { get; set; }
    }
}
