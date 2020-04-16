using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Application.UnitTests.UserGroups.Commands
{
    public class UserGroupCommandTestsBase
    {
        protected Guid UserGroupId { get; }
        protected Mock<IIdentityService> IdentityServiceMock { get; }
        protected Mock<IUserGroupRepository> UserGroupRepositoryMock { get; }
        protected string ValidNewMemberUserId { get; }

        public UserGroupCommandTestsBase()
        {
            UserGroupId = Guid.NewGuid();

            var creatorUserId = "User1";

            var userGroup = UserGroup.Create(UserGroupId, creatorUserId, "UserGroup", new List<string> { "User2" },
                "UserGroup Description");

            ValidNewMemberUserId = "User3";
            
            IdentityServiceMock = new Mock<IIdentityService>();
            IdentityServiceMock.Setup(m => m.CurrentUserId).Returns(creatorUserId);
            IdentityServiceMock.Setup(m => m.UserExistsAsync(ValidNewMemberUserId)).ReturnsAsync(true);

            UserGroupRepositoryMock = new Mock<IUserGroupRepository>();
            UserGroupRepositoryMock.Setup(m => m.GetAsync(UserGroupId, creatorUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userGroup);
            UserGroupRepositoryMock.Setup(m => m.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}
