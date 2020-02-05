using System;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public class TodoItem: OrderedListItemBase
    {
        public DateTime? DueDate { get; private set; }

        public TodoItem(string title, string description, DateTime? dueDate = null) : base(title, description)
        {
            DueDate = dueDate;
        }

        public void Set(string title, string description, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public void SetCompleted(bool isCompleted = true)
        {
            IsCompleted = isCompleted;
        }
    }
}
