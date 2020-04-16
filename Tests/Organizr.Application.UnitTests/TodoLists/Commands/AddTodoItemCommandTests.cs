using System;
using System.Threading;
using FluentAssertions;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.TodoLists.Commands.AddTodoItem;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class AddTodoItemCommandTests : TodoListCommandTestsBase
    {
        private readonly AddTodoItemCommandHandler _sut;

        public AddTodoItemCommandTests()
        {
            _sut = new AddTodoItemCommandHandler(IdentityServiceMock.Object, TodoListRepositoryMock.Object);
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
        public void Handle_NonMemberUserId_ThrowsResourceNotFoundException()
        {
            var noAccessUserId = "User2";
            IdentityServiceMock.Setup(m => m.CurrentUserId).Returns(noAccessUserId);

            var request = new AddTodoItemCommand(TodoListId, "Title", "Description", ClientDateToday.AddDays(1),
                ClientTimeZoneOffsetInMinutes);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<TodoList>>().Where(exception => exception.ResourceId == TodoListId);
        }
    }
}
