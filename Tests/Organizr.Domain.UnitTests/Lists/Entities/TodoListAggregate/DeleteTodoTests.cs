using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class DeleteTodoTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void DeleteTodo_ValidTodoId_TodoMarkedDeleted(int todoId)
        {
            var fixture = new TodoListFixture();

            var deletedTodo = fixture.TodoList.Items.Single(item => item.Id == todoId);

            fixture.TodoList.DeleteTodo(todoId);

            deletedTodo.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteTodo_NonExistantTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistantTodoId = 99;

            fixture.TodoList.Invoking(l => l.DeleteTodo(nonExistantTodoId)).Should().Throw<TodoItemDoesNotExistException>().And
                .TodoId.Should().Be(nonExistantTodoId);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void DeleteTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.DeleteTodo(todoId)).Should().Throw<TodoSubListDeletedException>().And
                .SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeleteTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            fixture.TodoList.Invoking(l => l.DeleteTodo(invalidTodoId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("todoId");
        }
    }
}
