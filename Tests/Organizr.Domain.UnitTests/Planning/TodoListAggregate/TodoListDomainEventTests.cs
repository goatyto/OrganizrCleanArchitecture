using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListDomainEventTests
    {
        [Fact]
        public void TodoListCreatedConstructor_ValidTodoList_ObjectInitialized()
        {
            var todoList = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Title", "Description");

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
            var todoList = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Title", "Description");

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
