using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.TodoLists.Commands
{
    public abstract class TodoListCommandsTestBase
    {
        protected readonly Guid TodoListId;
        protected readonly Mock<ICurrentUserService> CurrentUserServiceMock;
        protected readonly Mock<IResourceAccessService> ResourceAccessServiceMock;
        protected readonly Mock<ITodoListRepository> TodoListRepositoryMock;
        protected readonly Mock<IDateTime> DateTimeProviderMock;

        public TodoListCommandsTestBase()
        {
            TodoListId = Guid.NewGuid();
            var ownerId = "User1";

            CurrentUserServiceMock = new Mock<ICurrentUserService>();
            CurrentUserServiceMock.Setup(m => m.UserId).Returns(ownerId);

            ResourceAccessServiceMock = new Mock<IResourceAccessService>();
            ResourceAccessServiceMock.Setup(m => m.CanAccess(TodoListId, ownerId)).Returns(true);

            DateTimeProviderMock = new Mock<IDateTime>();
            DateTimeProviderMock.Setup(m => m.Now).Returns(DateTime.UtcNow);
            DateTimeProviderMock.Setup(m => m.Today).Returns(DateTime.UtcNow.Date);

            TodoListRepositoryMock = new Mock<ITodoListRepository>();
            TodoListRepositoryMock.Setup(m => m.GetByIdAsync(TodoListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                    new TodoListStub(TodoListId, ownerId, "TodoList Title", "TodoList Description",
                        new List<TodoSubList>
                        {
                            new TodoSubListStub(1, TodoListId, "TodoSubList Title", 1,
                                "TodoSubList Description"),
                            new TodoSubListStub(2, TodoListId, "TodoSubList2 Title", 2,
                                "TodoSubList2 Description")

                        },
                        new List<TodoItem>
                        {
                            new TodoItemStub(1, TodoListId, "TodoItem Title", new TodoItemPosition(1, null),
                                "TodoItem Description", DateTimeProviderMock.Object.Today.AddDays(1)),
                            new TodoItemStub(2, TodoListId, "TodoItem2 Title", new TodoItemPosition(2, null),
                                "TodoItem2 Description", DateTimeProviderMock.Object.Today.AddDays(2))
                        }));
            TodoListRepositoryMock.Setup(m => m.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }

    public class TodoListStub : TodoList
    {
        public TodoListStub(Guid id, string ownerId, string title, string description = null, IEnumerable<TodoSubList> subLists = null, IEnumerable<TodoItem> items = null) : base(ownerId, title, description)
        {
            Id = id;

            if (subLists != null)
                foreach (var subList in subLists)
                    _subLists.Add(subList);

            if (items != null)
                foreach (var item in items)
                    _items.Add(item);
        }
    }

    public class TodoSubListStub : TodoSubList
    {
        public TodoSubListStub(int id, Guid mainListId, string title, int ordinal, string description = null) : base(mainListId, title, ordinal, description)
        {
            Id = id;
        }
    }

    public class TodoItemStub : TodoItem
    {
        public TodoItemStub(int id, Guid mainListId, string title, TodoItemPosition position, string description = null,
            DateTime? dueDate = null) : base(mainListId, title, position, description, dueDate)
        {
            Id = id;
        }
    }
}
