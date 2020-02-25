using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public abstract class TodoListException : Exception
    {
        public Guid ListId { get; set; }

        public TodoListException(Guid listId, string message, Exception innerException = null) : base(message,
            innerException)
        {
            ListId = listId;
        }
    }

    public class TodoSubListDoesNotExistException : TodoListException
    {
        public int SubListId { get; }

        public TodoSubListDoesNotExistException(Guid listId, int subListId, Exception innerException = null) : base(
            listId, $"Sublist with id \"{subListId}\" does not exist in the context of list \"{listId}\"",
            innerException)
        {
            SubListId = subListId;
        }
    }
    
    public class TodoSubListDeletedException : TodoListException
    {
        public int SubListId { get; }

        public TodoSubListDeletedException(Guid listId, int subListId, Exception innerException = null) : base(listId,
            $"Sublist with id \"{subListId}\" is marked as deleted", innerException)
        {
            SubListId = subListId;
        }
    }

    public class TodoItemDoesNotExistException : TodoListException
    {
        public int TodoId { get; }

        public TodoItemDoesNotExistException(Guid listId, int todoId, Exception innerException = null) : base(listId,
            $"Todo with id \"{todoId}\" does not exist in list \"{listId}\"", innerException)
        {
            TodoId = todoId;
        }
    }

    public class TodoItemCompletedException : TodoListException
    {
        public int TodoId { get; }

        public TodoItemCompletedException(Guid listId, int todoId, Exception innerException = null) : base(listId,
            $"Todo with id \"{todoId}\" is marked as completed", innerException)
        {
            TodoId = todoId;
        }
    }

    public class TodoItemDeletedException : TodoListException
    {
        public int TodoId { get; }

        public TodoItemDeletedException(Guid listId, int todoId, Exception innerException = null) : base(listId,
            $"Todo with id \"{todoId}\" is marked as deleted", innerException)
        {
            TodoId = todoId;
        }
    }

    public class DueDateInThePastException : TodoListException
    {
        public DateTime DueDate { get; }

        public DueDateInThePastException(Guid listId, DateTime dueDate, Exception innerException = null) : base(listId,
            $"Todo item due date cannot be in the past: {dueDate}", innerException)
        {
            DueDate = dueDate;
        }
    }
}
