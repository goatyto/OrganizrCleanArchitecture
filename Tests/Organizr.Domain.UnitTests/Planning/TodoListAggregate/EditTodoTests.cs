using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class EditTodoTests
    {
        private readonly TodoListFixture _fixture;

        public EditTodoTests()
        {
            _fixture = new TodoListFixture();
        }

        [Fact]
        public void EditTodo_ValidData_TodoEdited()
        {
            var todoId = (TodoItemId)1;

            var todoToBeEdited = _fixture.Sut.Items.Single(item => item.TodoItemId == todoId);
            var originalPosition = todoToBeEdited.Position;
            var originalIsCompleted = todoToBeEdited.IsCompleted;
            var originalIsDeleted = todoToBeEdited.IsDeleted;

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(5);

            _fixture.Sut.EditTodo(todoId, newTitle, newDescription, newDueDateUtc);

            todoToBeEdited.TodoItemId.Should().Be(todoId);
            todoToBeEdited.Title.Should().Be(newTitle);
            todoToBeEdited.Description.Should().Be(newDescription);
            todoToBeEdited.DueDateUtc.Should().Be(newDueDateUtc);
            todoToBeEdited.Position.Should().Be(originalPosition);
            todoToBeEdited.IsCompleted.Should().Be(originalIsCompleted);
            todoToBeEdited.IsDeleted.Should().Be(originalIsDeleted);
        }

        [Fact]
        public void EditTodo_NonExistingTodoId_ThrowsTodoListException()
        {
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = _fixture.CreateClientDateUtcWithDaysOffset(5);
            var nonExistentTodoId = (TodoItemId)99;

            _fixture.Sut.Invoking(l => l.EditTodo(nonExistentTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{nonExistentTodoId}*does not exist*");
        }

        [Fact]
        public void EditTodo_DeletedTodoId_ThrowsTodoItemDeletedException()
        {
            var deletedTodoId = (TodoItemId)3;

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(5);

            _fixture.Sut.Invoking(l => l.EditTodo(deletedTodoId, newTitle, newDescription, newDueDateUtc)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedTodoId}*does not exist*");
        }

        [Fact]
        public void EditTodo_DeletedSubListTodoId_ThrowsTodoListException()
        {
            var deletedSubListTodoId = (TodoItemId)11;
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(5);

            _fixture.Sut.Invoking(l => l.EditTodo(deletedSubListTodoId, newTitle, newDescription, newDueDateUtc)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedSubListTodoId}*does not exist*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string newTitle)
        {
            var todoId = (TodoItemId)1;
            var newDescription = "Todo Description";
            var newDueDate = _fixture.CreateClientDateUtcWithDaysOffset(5);

            _fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Fact]
        public void EditTodo_DueDateInThePast_ThrowsTodoListException()
        {
            var todoId = (TodoItemId)1;
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(-1);

            _fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDateUtc)).Should()
                .Throw<TodoListException>().WithMessage("*due date*before client today*");
        }
    }
}
