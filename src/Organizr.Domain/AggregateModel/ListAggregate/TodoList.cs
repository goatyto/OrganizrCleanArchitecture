using System;
using System.Linq;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public class TodoList : MainListBase<TodoSubList, TodoItem>, IEntity<Guid>, IAggregateRoot
    {
        public Guid Id { get; }

        public DateTime? DueDate
        {
            get
            {
                var dueDates = Items.Union(SubLists.SelectMany(sublist => sublist.Items)).Where(item => item.DueDate.HasValue).Select(item => item.DueDate).ToList();
                return dueDates.Any() ? dueDates.Max() : null;
            }
        }

        public TodoList() : base()
        {
            Id = Guid.NewGuid();
        }

        public void AddTodo(string title, string description, DateTime? dueDate = null, int? subListId = null)
        {
            if (subListId.HasValue)
            {
                var subList = SubLists.SingleOrDefault(sublist => sublist.Id == subListId.Value);
                if(subList == null)
                    // TODO: Add custom exceptions
                    throw new Exception();

                subList.AddTodo(title, description, dueDate);
            }
            else
            {
                var todo = new TodoItem(title, description, dueDate);
                _items.Add(todo);
            }
        }

        public void EditTodo(int todoId, string title, string description, DateTime? dueDate = null)
        {
            var todo = GetItemById(todoId);
            todo.Set(title, description, dueDate);
        }

        public void SetCompletedTodo(int todoId, bool isCompleted = true)
        {
            var todo = GetItemById(todoId);
            todo.SetCompleted(isCompleted);
        }

        public void DeleteTodo(int todoId)
        {

        }
    }
}
