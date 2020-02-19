using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class ResourceEntityTests
    {
        [Fact]
        public void AddContributor_ValidContributorId_ContributorAdded()
        {
            var fixture = new ResourceEntityFixture();

            var contributorId = "User";
            fixture.ResourceEntity.AddContributor(contributorId);

            var addedContributor = fixture.ResourceEntity.ContributorIds.Last();

            addedContributor.Should().Be(contributorId);
        }

        [Fact]
        public void AddContributor_ExistingContributorId_ThrowsContributorAlreadyExistsException()
        {
            var fixture = new ResourceEntityFixture();

            var existingContributorId = "User2";

            fixture.ResourceEntity.Invoking(re => re.AddContributor(existingContributorId)).Should()
                .Throw<ContributorAlreadyExistsException>().And.ContributorId.Should().Be(existingContributorId);
        }

        [Fact]
        public void AddContributor_OwnerId_ThrowsContributorAlreadyOwnerException()
        {
            var fixture = new ResourceEntityFixture();

            var ownerId = "User1";

            fixture.ResourceEntity.Invoking(re => re.AddContributor(ownerId)).Should()
                .Throw<ContributorAlreadyOwnerException>().And.ContributorId.Should().Be(ownerId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddContributor_NullOrWhiteSpaceContributorId_ThrowsArgumentException(string contributorId)
        {
            var fixture = new ResourceEntityFixture();

            fixture.ResourceEntity.Invoking(re => re.AddContributor(contributorId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("contributorId");
        }

        [Fact]
        public void RemoveContributor_ExistingContributorId_ContributorRemoved()
        {
            var fixture = new ResourceEntityFixture();

            var contributorId = "User2";

            fixture.ResourceEntity.RemoveContributor(contributorId);

            fixture.ResourceEntity.ContributorIds.Should().NotContain(contributorId);
        }

        [Fact]
        public void RemoveContributor_NonExistentContributorId_ThrowsContributorDoesNotExistException()
        {
            var fixture = new ResourceEntityFixture();

            var nonExistentContributorId = "User";

            fixture.ResourceEntity.Invoking(re => re.RemoveContributor(nonExistentContributorId)).Should()
                .Throw<ContributorDoesNotExistException>().And.ContributorId.Should().Be(nonExistentContributorId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RemoveContributor_NullOrWhiteSpaceContributorId_ThrowsArgumentException(string contributorId)
        {
            var fixture = new ResourceEntityFixture();

            fixture.ResourceEntity.Invoking(re => re.RemoveContributor(contributorId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("contributorId");
        }

        private class ResourceEntityStub : ResourceEntity
        {
            public ResourceEntityStub(string ownerId, IEnumerable<string> contributorIds = null)
            {
                OwnerId = ownerId;

                if (contributorIds == null)
                    contributorIds = new List<string>();

                foreach (var contributorId in contributorIds)
                {
                    _contributorIds.Add(contributorId);
                }
            }
        }

        private class ResourceEntityFixture : IDisposable
        {
            public ResourceEntity ResourceEntity { get; private set; }

            public ResourceEntityFixture()
            {
                ResourceEntity = new ResourceEntityStub("User1", new[] { "User2", "User3" });
            }

            public void Dispose()
            {

            }
        }
    }
}
