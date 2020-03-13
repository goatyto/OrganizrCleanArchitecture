using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoListCreated : IDomainEvent
    {
        public TodoList TodoList { get; }

        public TodoListCreated(TodoList todoList)
        {
            Guard.Against.Null(todoList, nameof(todoList));

            TodoList = todoList;
        }
    }
}
