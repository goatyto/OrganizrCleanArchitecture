﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.TodoLists.Commands.EditTodoSubList;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class EditTodoSubListCommandTests : TodoListCommandsTestBase
    {
        private readonly EditTodoSubListCommandHandler _sut;

        public EditTodoSubListCommandTests()
        {
            _sut = new EditTodoSubListCommandHandler(CurrentUserServiceMock.Object, ResourceAccessServiceMock.Object,
                TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new EditTodoSubListCommand(TodoListId, 1, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentTodoListId_ThrowsNotFoundException()
        {
            var nonExistentTodoListId = Guid.NewGuid();

            var request = new EditTodoSubListCommand(nonExistentTodoListId, 1, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should()
                .Throw<NotFoundException<TodoList>>().And.Id.Should().Be(nonExistentTodoListId);
        }

        [Fact]
        public void Handle_CurrentUserHasNoAccess_ThrowsNotFoundException()
        {
            var noAccessUserId = "User2";
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(noAccessUserId);

            var request = new EditTodoSubListCommand(TodoListId, 1, "Title", "Description");

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<AccessDeniedException>().Where(exception =>
                    exception.ResourceId == TodoListId && exception.UserId == noAccessUserId);
        }
    }
}