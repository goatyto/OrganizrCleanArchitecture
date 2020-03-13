using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class UserGroupExceptionsTests
    {
        [Fact]
        public void UserAlreadyMemberExceptionConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var userId = "User1";
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new UserAlreadyMemberException(userGroupId, userId, innerException);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserId.Should().Be(userId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void UserNotAMemberExceptionConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var userId = "User1";
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new UserNotAMemberException(userGroupId, userId, innerException);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.UserId.Should().Be(userId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void CreatorCannotBeRemovedExceptionConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new CreatorCannotBeRemovedException(userGroupId, innerException);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void SharedResourceAlreadyExistsExceptionConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var sharedResourceId = Guid.NewGuid();
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new SharedResourceAlreadyExistsException<AnySharedResourceType>(userGroupId, sharedResourceId, innerException);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.SharedResourceId.Should().Be(sharedResourceId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void SharedResourceDoesNotExistExceptionConstructor_ValidData_ObjectInitialized()
        {
            var userGroupId = Guid.NewGuid();
            var sharedResourceId = Guid.NewGuid();
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var sut = new ResourceNotSharedInGroupException<AnySharedResourceType>(userGroupId, sharedResourceId, innerException);

            sut.UserGroupId.Should().Be(userGroupId);
            sut.SharedResourceId.Should().Be(sharedResourceId);
            sut.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        private class AnySharedResourceType : Entity<Guid>, IAggregateRoot
        {

        }
    }
}
