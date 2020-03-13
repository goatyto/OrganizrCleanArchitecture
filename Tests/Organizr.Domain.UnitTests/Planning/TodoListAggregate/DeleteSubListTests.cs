using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class DeleteSubListTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteSubList_SubListId_MarksSubListAsDeleted(int subListId)
        {
            var fixture = new TodoListFixture();

            var subListToBeDeleted = fixture.Sut.SubLists.Single(sl => sl.Id == subListId);

            fixture.Sut.DeleteSubList(subListId);

            subListToBeDeleted.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteSubList_NonExistentSubListId_ThrowsSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentSubListId = 99;

            fixture.Sut.Invoking(l => l.DeleteSubList(nonExistentSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }
    }
}
