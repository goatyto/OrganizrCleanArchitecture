using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListExceptionsTests
    {
        [Fact]
        public void TodoSubListDeletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var subListId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new TodoSubListDeletedException(listId, subListId, innerException);

            sut.ListId.Should().Be(listId);
            sut.SubListId.Should().Be(subListId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void TodoItemCompletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var todoId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new TodoItemCompletedException(listId, todoId, innerException);

            sut.ListId.Should().Be(listId);
            sut.TodoId.Should().Be(todoId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void TodoItemDeletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var todoId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new TodoItemDeletedException(listId, todoId, innerException);

            sut.ListId.Should().Be(listId);
            sut.TodoId.Should().Be(todoId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void DueDateInThePastExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var dueDate = DateTime.Today;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new DueDateBeforeTodayException(listId, dueDate, innerException);

            sut.ListId.Should().Be(listId);
            sut.DueDate.Should().Be(dueDate);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }
    }
}
