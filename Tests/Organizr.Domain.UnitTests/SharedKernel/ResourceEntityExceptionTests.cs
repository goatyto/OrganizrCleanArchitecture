using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class ResourceEntityExceptionTests
    {
        [Fact]
        public void ContributorAlreadyOwnerExceptionConstructor_ValidData_ObjectInitialized()
        {
            var resourceId = Guid.NewGuid();
            var contributorId = "User1";
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);
            
            var exception = new ContributorAlreadyOwnerException(resourceId, contributorId, innerException);

            exception.ResourceId.Should().Be(resourceId);
            exception.ContributorId.Should().Be(contributorId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void ContributorAlreadyExistsExceptionConstructor_ValidData_ObjectInitialized()
        {
            var resourceId = Guid.NewGuid();
            var contributorId = "User1";
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new ContributorAlreadyExistsException(resourceId, contributorId, innerException);

            exception.ResourceId.Should().Be(resourceId);
            exception.ContributorId.Should().Be(contributorId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }

        [Fact]
        public void ContributorDoesNotExistExceptionConstructor_ValidData_ObjectInitialized()
        {
            var resourceId = Guid.NewGuid();
            var contributorId = "User1";
            var innerExceptionMessage = "Inner Exception";
            var innerException = new Exception(innerExceptionMessage);

            var exception = new ContributorDoesNotExistException(resourceId, contributorId, innerException);

            exception.ResourceId.Should().Be(resourceId);
            exception.ContributorId.Should().Be(contributorId);
            exception.InnerException.Message.Should().Be(innerExceptionMessage);
        }
    }
}
