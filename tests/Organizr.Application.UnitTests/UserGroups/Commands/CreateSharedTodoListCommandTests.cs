using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.UserGroups.CreateSharedTodoList;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Application.UnitTests.UserGroups.Commands
{
    public class CreateSharedTodoListCommandTests : UserGroupCommandTestsBase
    {
        private readonly CreateSharedTodoListCommandHandler _sut;

        public CreateSharedTodoListCommandTests()
        {
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock.Setup(m => m.GenerateNext<TodoList>()).Returns(Guid.NewGuid());

            var todoListRepositoryMock = new Mock<ITodoListRepository>();
            todoListRepositoryMock.Setup(m => m.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _sut = new CreateSharedTodoListCommandHandler(IdentityServiceMock.Object, UserGroupRepositoryMock.Object,
                idGeneratorMock.Object, todoListRepositoryMock.Object);
        }

        [Fact]
        public void Handle_ValidData_DoesNotThrow()
        {
            var title = "Title";

            var request = new CreateSharedTodoListCommand(UserGroupId, title);

            _sut.Invoking(s=>s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }

        [Fact]
        public void Handle_NonExistentUserGroupId_ThrowsResourceNotFoundException()
        {
            var nonExistentUserGroupId = Guid.NewGuid();
            var todoListTitle = "Title";

            var request = new CreateSharedTodoListCommand(nonExistentUserGroupId, todoListTitle);

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should()
                .Throw<ResourceNotFoundException<UserGroup>>().And.ResourceId.Should().Be(nonExistentUserGroupId);
        }
    }
}
