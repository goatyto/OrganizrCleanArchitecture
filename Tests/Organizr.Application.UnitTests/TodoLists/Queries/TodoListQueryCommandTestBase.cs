﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.TodoLists.Queries;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public abstract class TodoListQueryCommandTestBase
    {
        private readonly string UserId;
        protected readonly Mock<IIdentityService> CurrentUserServiceMock;
        protected readonly Mock<ITodoListQueries> TodoListQueriesMock;

        public TodoListQueryCommandTestBase()
        {
            UserId = "User1";

            CurrentUserServiceMock = new Mock<IIdentityService>();
            CurrentUserServiceMock.Setup(m => m.CurrentUserId).Returns(UserId);

            TodoListQueriesMock = new Mock<ITodoListQueries>();
            TodoListQueriesMock.Setup(m => m.GetTodoListsForUserAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TodoListDto>());
        }
    }
}
