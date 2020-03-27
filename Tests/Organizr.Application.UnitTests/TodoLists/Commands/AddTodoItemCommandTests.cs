using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.TodoLists.Commands.AddTodoItem;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class AddTodoItemCommandTests : TodoListCommandsTestBase
    {
        private readonly AddTodoItemCommandHandler _sut;

        public AddTodoItemCommandTests()
        {
            _sut = new AddTodoItemCommandHandler(CurrentUserServiceMock.Object, ResourceAuthorizationServiceMock.Object,
                TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrowException()
        {
            var request = new AddTodoItemCommand(TodoListId, "Title", "Description", ClientDateToday.AddDays(1),
                ClientTimeZoneOffsetInMinutes);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new AddTodoItemCommand(nonExistentTodoListId, "Title", "Description",
                ClientDateToday.AddDays(1), ClientTimeZoneOffsetInMinutes);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<TodoList>>().And.ResourceId.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_CurrentUserHasNoAccess_ThrowsAccessDeniedException()
        {
            var noAccessUserId = "User2";
            CurrentUserServiceMock.Setup(m => m.CurrentUserId).Returns(noAccessUserId);

            var request = new AddTodoItemCommand(TodoListId, "Title", "Description", ClientDateToday.AddDays(1),
                ClientTimeZoneOffsetInMinutes);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<AccessDeniedException<TodoList>>().Where(exception =>
                    exception.ResourceId == TodoListId && exception.UserId == noAccessUserId);
        }
    }
}
