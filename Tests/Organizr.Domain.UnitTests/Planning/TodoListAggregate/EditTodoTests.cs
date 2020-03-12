using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class EditTodoTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public void EditTodo_ValidData_TodoEdited(int todoId)
        {
            var fixture = new TodoListFixture();

            var todoToBeEdited = fixture.Sut.Items.Single(item => item.Id == todoId);
            var originalPosition = todoToBeEdited.Position;
            var originalIsCompleted = todoToBeEdited.IsCompleted;
            var originalIsDeleted = todoToBeEdited.IsDeleted;

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = fixture.ClientDateTimeToday.AddDays(5);

            fixture.Sut.EditTodo(todoId, newTitle, newDescription, newDueDateUtc, fixture.ClientTimeZoneOffsetInMinutes,
                fixture.ClientDateValidator);

            todoToBeEdited.Id.Should().Be(todoId);
            todoToBeEdited.TodoListId.Should().Be(fixture.TodoListId);
            todoToBeEdited.Title.Should().Be(newTitle);
            todoToBeEdited.Description.Should().Be(newDescription);
            todoToBeEdited.DueDateUtc.Should().Be(newDueDateUtc);
            todoToBeEdited.Position.Should().Be(originalPosition);
            todoToBeEdited.IsCompleted.Should().Be(originalIsCompleted);
            todoToBeEdited.IsDeleted.Should().Be(originalIsDeleted);
        }

        [Fact]
        public void EditTodo_NonExistingTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = fixture.ClientDateTimeToday.AddDays(5);
            var nonExistentTodoId = 99;

            fixture.Sut.Invoking(l => l.EditTodo(nonExistentTodoId, newTitle, newDescription, newDueDate,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<ArgumentException>()
                .And.ParamName.Should().Be("todoId");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(7)]
        public void EditTodo_CompletedTodoId_ThrowsTodoItemCompletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = fixture.ClientDateTimeToday.AddDays(5);

            fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<TodoItemCompletedException>().And.TodoId.Should().Be(todoId);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(8)]
        public void EditTodo_DeletedTodoId_ThrowsTodoItemDeletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = fixture.ClientDateTimeToday.AddDays(5);

            fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDateUtc,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<TodoItemDeletedException>().And.TodoId.Should().Be(todoId);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void EditTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int deletedSubListTodoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = fixture.ClientDateTimeToday.AddDays(5);
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.EditTodo(deletedSubListTodoId, newTitle, newDescription, newDueDateUtc,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string newTitle)
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newDescription = "Todo Description";
            var newDueDate = fixture.ClientDateTimeToday.AddDays(5);

            fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<ArgumentException>()
                .And.ParamName.Should().Be("title");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void EditTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = fixture.ClientDateTimeToday.AddDays(5);

            fixture.Sut.Invoking(l => l.EditTodo(invalidTodoId, newTitle, newDescription, newDueDate,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<ArgumentException>()
                .And.ParamName.Should().Be("todoId");
        }

        [Fact]
        public void EditTodo_DueDateInThePast_ThrowsDueDateInThePastException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDateUtc = fixture.ClientDateTimeToday.AddDays(-1);

            fixture.Sut.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDateUtc,
                    fixture.ClientTimeZoneOffsetInMinutes, fixture.ClientDateValidator)).Should()
                .Throw<DueDateBeforeTodayException>().And.DueDate.Should().Be(newDueDateUtc);
        }
    }
}
