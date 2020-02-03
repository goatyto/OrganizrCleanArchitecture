using System;
using System.Linq;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.Entities.TodoListAggregate
{
    public class TodoList: ListBase<TodoItem>, IAggregate
    {
        public DateTime? DueDate => Items.Where(item => item.DueDate.HasValue).Max(item => item.DueDate);
    }
}
