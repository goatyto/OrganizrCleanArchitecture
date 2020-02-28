using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoListExceptionTests
    {
        [Fact]
        public void TodoSubListDeletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var subListId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new TodoSubListDeletedException(listId, subListId, innerException);

            exception.ListId.Should().Be(listId);
            exception.SubListId.Should().Be(subListId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void TodoItemCompletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var todoId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new TodoItemCompletedException(listId, todoId, innerException);

            exception.ListId.Should().Be(listId);
            exception.TodoId.Should().Be(todoId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void TodoItemDeletedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var todoId = 1;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new TodoItemDeletedException(listId, todoId, innerException);

            exception.ListId.Should().Be(listId);
            exception.TodoId.Should().Be(todoId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void DueDateInThePastExceptionConstructor_ValidData_ObjectInitialized()
        {
            var listId = Guid.NewGuid();
            var dueDate = DateTime.Today;
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new DueDateInThePastException(listId, dueDate, innerException);

            exception.ListId.Should().Be(listId);
            exception.DueDate.Should().Be(dueDate);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }
    }
}
