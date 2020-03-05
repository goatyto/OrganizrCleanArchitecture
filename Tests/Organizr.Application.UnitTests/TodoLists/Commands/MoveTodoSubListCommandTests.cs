using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.TodoLists.Commands.MoveTodoSubList;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class MoveTodoSubListCommandTests : TodoListCommandsTestBase
    {
        private readonly MoveTodoSubListCommandHandler _sut;

        public MoveTodoSubListCommandTests()
        {
            _sut = new MoveTodoSubListCommandHandler(CurrentUserServiceMock.Object, ResourceAccessServiceMock.Object,
                TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new MoveTodoSubListCommand(TodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new MoveTodoSubListCommand(nonExistentTodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<NotFoundException<TodoList>>().And.Id.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_CurrentUserHasNoAccess_ThrowsNotFoundException()
        {
            var noAccessUserId = "User2";
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(noAccessUserId);

            var request = new MoveTodoSubListCommand(TodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<AccessDeniedException>().Where(exception =>
                    exception.ResourceId == TodoListId && exception.UserId == noAccessUserId);
        }
    }
}
