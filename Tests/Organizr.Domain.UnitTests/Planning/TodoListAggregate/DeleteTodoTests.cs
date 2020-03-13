using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
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

            var deletedTodo = fixture.Sut.Items.Single(item => item.Id == todoId);

            fixture.Sut.DeleteTodo(todoId);

            deletedTodo.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteTodo_NonExistentTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentTodoId = 99;

            fixture.Sut.Invoking(l => l.DeleteTodo(nonExistentTodoId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("todoId");
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void DeleteTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.DeleteTodo(todoId)).Should().Throw<TodoSubListDeletedException>().And
                .SubListId.Should().Be(deletedSubListId);
        }
    }
}
