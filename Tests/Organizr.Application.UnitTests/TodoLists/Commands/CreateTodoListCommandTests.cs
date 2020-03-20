using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.TodoLists.Commands.CreateTodoList;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class CreateTodoListCommandTests : TodoListCommandsTestBase
    {
        private readonly Mock<IIdGenerator> _idGeneratorMock;
        private readonly CreateTodoListCommandHandler _sut;

        public CreateTodoListCommandTests()
        {
            var todoListId = Guid.NewGuid();

            _idGeneratorMock = new Mock<IIdGenerator>();
            _idGeneratorMock.Setup(m => m.GenerateNext<TodoList>()).Returns(todoListId);

            _sut = new CreateTodoListCommandHandler(_idGeneratorMock.Object, CurrentUserServiceMock.Object,
                TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new CreateTodoListCommand("Title", "Description");

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }
    }
}
