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
        public Guid SubListId { get; }

        public TodoSubListDoesNotExistException(Guid listId, Guid subListId, Exception innerException = null): base(
            $"Sublist with id \"{subListId}\" does not exist in the context of list \"{listId}\"")
        {
            ListId = listId;
            SubListId = subListId;
        }
    }

    public class TodoSubListAlreadyExistsException : TodoListException
    {
        public Guid ListId { get; }
        public Guid SubListId { get; }

        public TodoSubListAlreadyExistsException(Guid listId, Guid subListId, Exception innerException = null) : base(
            $"Sublist with id \"{subListId}\" already exists in the context of list \"{listId}\"")
        {
            ListId = listId;
            SubListId = subListId;
        }
    }

    public class TodoItemDoesNotExistException : TodoListException
    {
        public Guid ListId { get; }
        public Guid TodoId { get; }

        public TodoItemDoesNotExistException(Guid listId, Guid todoId, Exception innerException = null) : base(
            $"Todo with id \"{todoId}\" does not exist in list \"{listId}\"")
        {
            ListId = listId;
            TodoId = todoId;
        }
    }
}
