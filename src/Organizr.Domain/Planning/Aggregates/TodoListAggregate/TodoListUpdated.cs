using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoListUpdated : IDomainEvent
    {
        public TodoList TodoList { get; }

        public TodoListUpdated(TodoList todoList)
        {
            Assert.Argument.NotNull(todoList, nameof(todoList));

            TodoList = todoList;
        }
    }
}
