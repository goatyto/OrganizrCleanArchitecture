using System;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoListException : Exception
    {
        public TodoListException(string message): base(message)
        {
            
        }
    }
}
