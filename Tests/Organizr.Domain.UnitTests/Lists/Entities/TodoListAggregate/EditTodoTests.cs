using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class EditTodoTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public void EditTodo_ValidData_TodoEdited(int todoId)
        {
            var fixture = new TodoListFixture();

            var todoToBeEdited = fixture.TodoList.Items.Single(item => item.Id == todoId);
            var originalPosition = todoToBeEdited.Position;
            var originalIsCompleted = todoToBeEdited.IsCompleted;
            var originalIsDeleted = todoToBeEdited.IsDeleted;

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.EditTodo(todoId, newTitle, newDescription, newDueDate, fixture.DateTimeProvider);


            todoToBeEdited.Id.Should().Be(todoId);
            todoToBeEdited.MainListId.Should().Be(fixture.TodoListId);
            todoToBeEdited.Title.Should().Be(newTitle);
            todoToBeEdited.Description.Should().Be(newDescription);
            todoToBeEdited.DueDate.Should().Be(newDueDate);
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
            var newDueDate = DateTime.Today.AddDays(5);
            var nonExistentTodoId = 99;

            fixture.TodoList.Invoking(l => l.EditTodo(nonExistentTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(7)]
        public void EditTodo_CompletedTodoId_ThrowsTodoItemCompletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate, fixture.DateTimeProvider)).Should()
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
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate, fixture.DateTimeProvider)).Should()
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
            var newDueDate = DateTime.Today.AddDays(5);
            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.EditTodo(deletedSubListTodoId, newTitle, newDescription, newDueDate, fixture.DateTimeProvider)).Should()
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
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void EditTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(invalidTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }

        [Fact]
        public void EditTodo_DueDateInThePast_ThrowsDueDateInThePastException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(-1);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate, fixture.DateTimeProvider)).Should()
                .Throw<DueDateInThePastException>().And.DueDate.Should().Be(newDueDate);
        }

        [Fact]
        public void EditTodo_NullDateTimeProvider_ThrowsArgumentException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate, null)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("dateTimeProvider");
        }
    }
}
