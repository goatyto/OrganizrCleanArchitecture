using System;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public abstract class TodoListExceptionBase : Exception
    {
        public Guid ListId { get; set; }

        public TodoListExceptionBase(Guid listId, string message, Exception innerException = null) : base(message,
            innerException)
        {
            ListId = listId;
        }
    }

    public class TodoSubListDeletedException : TodoListExceptionBase
    {
        public int SubListId { get; }

        public TodoSubListDeletedException(Guid listId, int subListId, Exception innerException = null) : base(listId,
            $"Sublist with id \"{subListId}\" is marked as deleted.", innerException)
        {
            SubListId = subListId;
        }
    }

    public class TodoItemCompletedException : TodoListExceptionBase
    {
        public int TodoId { get; }

        public TodoItemCompletedException(Guid listId, int todoId, Exception innerException = null) : base(listId,
            $"Todo with id \"{todoId}\" is marked as completed.", innerException)
        {
            TodoId = todoId;
        }
    }

    public class TodoItemDeletedException : TodoListExceptionBase
    {
        public int TodoId { get; }

        public TodoItemDeletedException(Guid listId, int todoId, Exception innerException = null) : base(listId,
            $"Todo with id \"{todoId}\" is marked as deleted.", innerException)
        {
            TodoId = todoId;
        }
    }

    public class DueDateBeforeTodayException : TodoListExceptionBase
    {
        public DateTime DueDate { get; }

        public DueDateBeforeTodayException(Guid listId, DateTime dueDate, Exception innerException = null) : base(listId,
            $"Todo item due date cannot be in the past: {dueDate}.", innerException)
        {
            DueDate = dueDate;
        }
    }
}
