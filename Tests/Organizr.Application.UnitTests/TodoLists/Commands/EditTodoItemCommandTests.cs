using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.TodoLists.Commands.EditTodoItem;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class EditTodoItemCommandTests : TodoListCommandsTestBase
    {
        private readonly EditTodoItemCommandHandler _sut;

        public EditTodoItemCommandTests()
        {
            _sut = new EditTodoItemCommandHandler(TodoListRepositoryMock.Object, DateTimeProviderMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new EditTodoItemCommand(TodoListId, 1, "Title", "Description", DateTimeProviderMock.Object.Today.AddDays(1));

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new EditTodoItemCommand(nonExistentTodoListId, 1, "Title", "Description", DateTimeProviderMock.Object.Today.AddDays(1));

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<NotFoundException<TodoList>>().And.Id.Should().Be(nonExistentTodoListId);
        }
    }
}
