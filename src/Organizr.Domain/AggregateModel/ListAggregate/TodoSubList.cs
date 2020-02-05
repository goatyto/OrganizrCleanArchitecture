using System;
using System.Linq;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public class TodoSubList: ListBase<TodoItem>, IEntity
    {
        public int Id { get; }

        public DateTime? DueDate => Items.Any(item => item.DueDate.HasValue)
            ? Items.Where(item => item.DueDate.HasValue).Max(item => item.DueDate)
            : null;

        public void AddTodo(string title, string description, DateTime? dueDate = null)
        {
            var todo = new TodoItem(title, description, dueDate);
            _items.Add(todo);
        }
    }
}
