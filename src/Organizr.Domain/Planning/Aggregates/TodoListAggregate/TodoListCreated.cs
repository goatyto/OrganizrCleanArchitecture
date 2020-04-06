using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoListCreated : IDomainEvent
    {
        public TodoList TodoList { get; }

        public TodoListCreated(TodoList todoList)
        {
            Assert.Argument.NotNull(todoList, nameof(todoList));

            TodoList = todoList;
        }
    }
}
