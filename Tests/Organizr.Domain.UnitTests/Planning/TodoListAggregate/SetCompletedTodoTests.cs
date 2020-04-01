using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class SetCompletedTodoTests
    {
        private readonly TodoListFixture _fixture;

        public SetCompletedTodoTests()
        {
            _fixture = new TodoListFixture();
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public void SetCompletedTodo_ValidIdAndBool_CompletedStatusChanged(int todoId, bool isCompleted)
        {
            var editedTodo = _fixture.Sut.Items.Single(item => item.Id == todoId);

            _fixture.Sut.SetCompletedTodo(todoId, isCompleted);

            editedTodo.IsCompleted.Should().Be(isCompleted);
        }

        [Fact]
        public void SetCompletedTodo_DeletedTodoId_ThrowsTodoListException()
        {
            var deletedTodoId = 3;
            var isCompleted = true;

            _fixture.Sut.Invoking(l => l.SetCompletedTodo(deletedTodoId, isCompleted)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedTodoId}*does not exist*");
        }

        [Fact]
        public void SetCompletedTodo_DeletedSubListTodoId_ThrowsTodoListException()
        {
            var deletedSubListTodoId = 11;
            var isCompleted = true;

            _fixture.Sut.Invoking(l => l.SetCompletedTodo(deletedSubListTodoId, isCompleted)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedSubListTodoId}*does not exist*");
        }
        
        [Fact]
        public void SetCompletedTodo_NonExistentTodoId_ThrowsTodoListException()
        {
            var nonExistentTodoId = 99;

            _fixture.Sut.Invoking(l => l.SetCompletedTodo(nonExistentTodoId)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{nonExistentTodoId}*does not exist*");
        }
    }
}
