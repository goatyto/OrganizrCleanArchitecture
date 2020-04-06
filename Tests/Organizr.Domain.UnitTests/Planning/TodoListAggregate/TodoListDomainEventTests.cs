using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListDomainEventTests
    {
        [Fact]
        public void TodoListCreatedConstructor_ValidTodoList_ObjectInitialized()
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";
            var title = "Title";
            var description = "Description";

            var todoList = TodoList.Create(todoListId, creatorUserId, title, description);

            var sut = new TodoListCreated(todoList);

            sut.TodoList.Should().Be(todoList);
        }

        [Fact]
        public void TodoListCreatedConstructor_NullTodoList_ThrowsArgumentException()
        {
            TodoList nullTodoList = null;

            Func<TodoListCreated> sut = () => new TodoListCreated(nullTodoList);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("todoList");
        }

        [Fact]
        public void TodoListUpdatedConstructor_ValidTodoList_ObjectInitialized()
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";
            var title = "Title";
            var description = "Description";

            var todoList = TodoList.Create(todoListId, creatorUserId, title, description);

            var sut = new TodoListUpdated(todoList);

            sut.TodoList.Should().Be(todoList);
        }

        [Fact]
        public void TodoListUpdatedConstructor_NullTodoList_ThrowsArgumentException()
        {
            TodoList nullTodoList = null;

            Func<TodoListUpdated> sut = () => new TodoListUpdated(nullTodoList);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("todoList");
        }
    }
}
