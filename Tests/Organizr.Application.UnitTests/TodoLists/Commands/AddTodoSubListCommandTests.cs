using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.TodoLists.Commands.AddTodoSubList;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class AddTodoSubListCommandTests : TodoListCommandsTestBase
    {
        private readonly AddTodoSubListCommandHandler _sut;

        public AddTodoSubListCommandTests()
        {
            _sut = new AddTodoSubListCommandHandler(CurrentUserServiceMock.Object,
                ResourceAuthorizationServiceMock.Object, TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new AddTodoSubListCommand(TodoListId, "Title", "Description");

            _sut.Invoking(s=>s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new AddTodoSubListCommand(nonExistentTodoListId, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<TodoList>>().And.ResourceId.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_CurrentUserHasNoAccess_ThrowsAccessDeniedException()
        {
            var noAccessUserId = "User2";
            CurrentUserServiceMock.Setup(m => m.CurrentUserId).Returns(noAccessUserId);

            var request = new AddTodoSubListCommand(TodoListId, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<AccessDeniedException<TodoList>>().Where(exception =>
                    exception.ResourceId == TodoListId && exception.UserId == noAccessUserId);
        }
    }
}
