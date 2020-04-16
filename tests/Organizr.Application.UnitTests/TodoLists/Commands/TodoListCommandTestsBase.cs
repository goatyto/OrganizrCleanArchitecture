using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public abstract class TodoListCommandTestsBase
    {
        protected Guid TodoListId { get; }
        protected Mock<IIdentityService> IdentityServiceMock { get; }
        protected Mock<ITodoListRepository> TodoListRepositoryMock { get; }
        protected DateTime ClientDateToday { get; }
        protected int ClientTimeZoneOffsetInMinutes { get; }

        public TodoListCommandTestsBase()
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

            IdentityServiceMock = new Mock<IIdentityService>();
            IdentityServiceMock.Setup(m => m.CurrentUserId).Returns(creatorUserId);

            TodoListRepositoryMock = new Mock<ITodoListRepository>();
            TodoListRepositoryMock.Setup(m => m.GetOwnAsync(TodoListId, creatorUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => todoList);
            TodoListRepositoryMock.Setup(m => m.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}
