﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.TodoLists.Commands.MoveTodoItem;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class MoveTodoItemCommandTests : TodoListCommandsTestBase
    {
        private readonly MoveTodoItemCommandHandler _sut;

        public MoveTodoItemCommandTests()
        {
            _sut = new MoveTodoItemCommandHandler(CurrentUserServiceMock.Object,
                ResourceAuthorizationServiceMock.Object, TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new MoveTodoItemCommand(TodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new MoveTodoItemCommand(nonExistentTodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<ResourceNotFoundException<TodoList>>().And.ResourceId.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_CurrentUserHasNoAccess_ThrowsAccessDeniedException()
        {
            var noAccessUserId = "User2";
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(noAccessUserId);

            var request = new MoveTodoItemCommand(TodoListId, 1, 2);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<AccessDeniedException<TodoList>>().Where(exception =>
                    exception.ResourceId == TodoListId && exception.UserId == noAccessUserId);
        }
    }
}
