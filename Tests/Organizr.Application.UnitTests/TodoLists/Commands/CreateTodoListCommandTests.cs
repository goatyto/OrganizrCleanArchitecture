﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.TodoLists.Commands.CreateTodoList;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public class CreateTodoListCommandTests : TodoListCommandsTestBase
    {
        private readonly CreateTodoListCommandHandler _sut;

        public CreateTodoListCommandTests()
        {
            _sut = new CreateTodoListCommandHandler(CurrentUserServiceMock.Object, TodoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new CreateTodoListCommand("Title", "Description");

            _sut.Invoking(s => s.Handle(request, It.IsAny<CancellationToken>())).Should().NotThrow();
        }
    }
}
