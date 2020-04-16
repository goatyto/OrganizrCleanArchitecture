using System;
using System.Threading;
using FluentAssertions;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.TodoLists.Commands.DeleteTodoSubList;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class DeleteTodoSubListCommandTests : TodoListCommandTestsBase
    {
        private readonly DeleteTodoSubListCommandHandler _sut;

        public DeleteTodoSubListCommandTests()
        {
            _sut = new DeleteTodoSubListCommandHandler(IdentityServiceMock.Object, TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new DeleteTodoSubListCommand(TodoListId, 1);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();
            
            var request = new DeleteTodoSubListCommand(nonExistentTodoListId, 1);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<TodoList>>().And.ResourceId.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_NonUserMemberId_ThrowsResourceNotFoundException()
        {
            var noAccessUserId = "User2";
            IdentityServiceMock.Setup(m => m.CurrentUserId).Returns(noAccessUserId);

            var request = new DeleteTodoSubListCommand(TodoListId, 1);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<TodoList>>().Where(exception => exception.ResourceId == TodoListId);
        }
    }
}
