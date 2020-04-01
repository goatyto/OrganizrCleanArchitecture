using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public abstract class TodoListCommandsTestBase
    {
        protected readonly Guid TodoListId;
        protected readonly Mock<IIdentityService> CurrentUserServiceMock;
        protected readonly Mock<IResourceAuthorizationService<TodoList>> ResourceAuthorizationServiceMock;
        protected readonly Mock<ITodoListRepository> TodoListRepositoryMock;
        protected readonly DateTime ClientDateToday;
        protected readonly int ClientTimeZoneOffsetInMinutes;

        public TodoListCommandsTestBase()
        {
            TodoListId = Guid.NewGuid();
            var creatorUserId = "User1";

            ClientDateToday = DateTime.UtcNow.Date;
            ClientTimeZoneOffsetInMinutes = 0;

            var todoList = TodoList.Create(TodoListId, creatorUserId, "TodoList Title", "TodoList Description");

            todoList.AddSubList("TodoSubList Title", "TodoSubList Description");
            todoList.AddSubList("TodoSubList Title2", "TodoSubList Description2");

            todoList.AddTodo("TodoItem Title", "TodoItem Description", ClientDateUtc.Create(ClientDateToday.AddDays(1), ClientTimeZoneOffsetInMinutes));
            todoList.AddTodo("TodoItem Title", "TodoItem Description", ClientDateUtc.Create(ClientDateToday.AddDays(2), ClientTimeZoneOffsetInMinutes));

            CurrentUserServiceMock = new Mock<IIdentityService>();
            CurrentUserServiceMock.Setup(m => m.CurrentUserId).Returns(creatorUserId);

            ResourceAuthorizationServiceMock = new Mock<IResourceAuthorizationService<TodoList>>();
            ResourceAuthorizationServiceMock.Setup(m => m.CanRead(creatorUserId, todoList)).Returns(true);
            ResourceAuthorizationServiceMock.Setup(m => m.CanModify(creatorUserId, todoList)).Returns(true);
            ResourceAuthorizationServiceMock.Setup(m => m.CanDelete(creatorUserId, todoList)).Returns(true);

            TodoListRepositoryMock = new Mock<ITodoListRepository>();
            TodoListRepositoryMock.Setup(m => m.GetAsync(TodoListId, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => todoList);
            TodoListRepositoryMock.Setup(m => m.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}
