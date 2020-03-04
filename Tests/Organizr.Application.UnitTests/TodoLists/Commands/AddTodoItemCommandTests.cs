using Organizr.Application.TodoLists.Commands.AddTodoItem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class AddTodoItemCommandTests : TodoListCommandsTestBase
    {
        private readonly AddTodoItemCommandHandler _sut;

        public AddTodoItemCommandTests()
        {
            _sut = new AddTodoItemCommandHandler(TodoListRepositoryMock.Object, DateTimeProviderMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrowException()
        {
            var request = new AddTodoItemCommand(TodoListId, "Title", "Description", DateTimeProviderMock.Object.Today.AddDays(1));

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new AddTodoItemCommand(nonExistentTodoListId, "Title", "Description", DateTimeProviderMock.Object.Today.AddDays(1));

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<NotFoundException<TodoList>>().And.Id.Should().Be(nonExistentTodoListId);
        }
    }
}
