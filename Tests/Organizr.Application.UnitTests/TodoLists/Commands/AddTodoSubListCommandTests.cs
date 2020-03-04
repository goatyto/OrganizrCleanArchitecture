using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.TodoLists.Commands.AddTodoSubList;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class AddTodoSubListCommandTests : TodoListCommandsTestBase
    {
        private readonly AddTodoSubListCommandHandler _sut;

        public AddTodoSubListCommandTests()
        {
            _sut = new AddTodoSubListCommandHandler(TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddTodoSubListCommand(TodoListId, "Title", "Description");

            _sut.Invoking(s=>s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new AddTodoSubListCommand(nonExistentTodoListId, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<NotFoundException<TodoList>>().And.Id.Should().Be(nonExistentTodoListId);
        }
    }
}
