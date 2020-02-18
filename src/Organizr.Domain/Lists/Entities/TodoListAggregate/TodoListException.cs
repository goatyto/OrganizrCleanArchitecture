using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Domain.Lists.Entities.TodoListAggregate
{
    public abstract class TodoListException : Exception
    {
        public TodoListException(string message, Exception innerException = null) : base(message, innerException)
        {

        }
    }

    public class TodoSubListDoesNotExistException : TodoListException
    {
        public Guid ListId { get; }
        public int SubListId { get; }

        public TodoSubListDoesNotExistException(Guid listId, int subListId, Exception innerException = null): base(
            $"Sublist with id \"{subListId}\" does not exist in the context of list \"{listId}\"", innerException)
        {
            ListId = listId;
            SubListId = subListId;
        }
    }

    public class TodoSubListAlreadyExistsException : TodoListException
    {
        public Guid ListId { get; }
        public int SubListId { get; }

        public TodoSubListAlreadyExistsException(Guid listId, int subListId, Exception innerException = null) : base(
            $"Sublist with id \"{subListId}\" already exists in the context of list \"{listId}\"", innerException)
        {
            ListId = listId;
            SubListId = subListId;
        }
    }

    public class TodoSubListDeletedException : TodoListException
    {
        public Guid ListId { get; }
        public int SubListId { get; }

        public TodoSubListDeletedException(Guid listId, int subListId, Exception innerException = null) : base(
            $"Sublist with id \"{subListId}\" is marked as deleted", innerException)
        {
            ListId = listId;
            SubListId = subListId;
        }
    }

    public class TodoItemDoesNotExistException : TodoListException
    {
        public Guid ListId { get; }
        public int TodoId { get; }

        public TodoItemDoesNotExistException(Guid listId, int todoId, Exception innerException = null) : base(
            $"Todo with id \"{todoId}\" does not exist in list \"{listId}\"", innerException)
        {
            ListId = listId;
            TodoId = todoId;
        }
    }

    public class TodoItemCompletedException : TodoListException
    {
        public Guid ListId { get; }
        public int TodoId { get; }

        public TodoItemCompletedException(Guid listId, int todoId, Exception innerException = null) : base(
            $"Todo with id \"{todoId}\" is marked as completed", innerException)
        {
            ListId = listId;
            TodoId = todoId;
        }
    }

    public class TodoItemDeletedException : TodoListException
    {
        public Guid ListId { get; }
        public int TodoId { get; }

        public TodoItemDeletedException(Guid listId, int todoId, Exception innerException = null) : base(
            $"Todo with id \"{todoId}\" is marked as deleted", innerException)
        {
            ListId = listId;
            TodoId = todoId;
        }
    }
}
