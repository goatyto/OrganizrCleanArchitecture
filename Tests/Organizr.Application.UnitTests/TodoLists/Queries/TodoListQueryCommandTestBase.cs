using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Moq;
using Organizr.Application.Common.Interfaces;
using Organizr.Application.TodoLists.Queries;
using Organizr.Application.TodoLists.Queries.GetTodoLists;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public abstract class TodoListQueryCommandTestBase
    {
        protected readonly string UserId;
        protected readonly Mock<ICurrentUserService> CurrentUserServiceMock;
        protected readonly Mock<ITodoListQueries> TodoListQueriesMock;

        public TodoListQueryCommandTestBase()
        {
            UserId = "User1";

            CurrentUserServiceMock = new Mock<ICurrentUserService>();
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(UserId);

            TodoListQueriesMock = new Mock<ITodoListQueries>();
            TodoListQueriesMock.Setup(m => m.GetTodoListsForUserAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TodoListDto>());
        }
    }
}
