using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class DeleteTodoTests
    {
        private readonly TodoListFixture _fixture;

        public DeleteTodoTests()
        {
            _fixture = new TodoListFixture();
        }

        [Fact]
        public void DeleteTodo_ValidTodoId_TodoMarkedDeleted()
        {
            var todoId = 1;

            var deletedTodo = _fixture.GetTodoItemById(todoId);

            _fixture.Sut.DeleteTodo(todoId);

            deletedTodo.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteTodo_NonExistentTodoId_ThrowsInvalidOperationException()
        {
            var nonExistentTodoId = 99;

            _fixture.Sut.Invoking(l => l.DeleteTodo(nonExistentTodoId)).Should().Throw<InvalidOperationException>()
                .WithMessage($"*todo item*{nonExistentTodoId}*does not exist*");
        }

        [Fact]
        public void DeleteTodo_DeletedSubListTodoId_ThrowsInvalidOperationException()
        {
            var deletedSubListTodoId = 11;

            _fixture.Sut.Invoking(l => l.DeleteTodo(deletedSubListTodoId)).Should().Throw<InvalidOperationException>()
                .WithMessage($"*todo item*{deletedSubListTodoId}*does not exist*");
        }
    }
}
