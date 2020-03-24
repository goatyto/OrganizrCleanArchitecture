using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class MoveSubListTests
    {
        [Theory]
        [InlineData(1, 4)]
        [InlineData(3, 3)]
        [InlineData(5, 2)]
        [InlineData(3, 1)]
        [InlineData(4, 3)]
        public void MoveSubList_ValidPosition_SubListOrdinalChanges(int subListId, int targetOrdinal)
        {
            var fixture = new TodoListFixture();

            var subListToBeMoved = fixture.Sut.SubLists.Single(sl => sl.Id == subListId);

            fixture.Sut.MoveSubList(subListId, targetOrdinal);

            subListToBeMoved.Ordinal.Should().Be(targetOrdinal);

            var resultSubListOrdinals =
                fixture.Sut.SubLists.Where(sl => !sl.IsDeleted).OrderBy(sl => sl.Ordinal).ToList();

            for (int i = 0; i < resultSubListOrdinals.Count; i++)
            {
                resultSubListOrdinals[i].Ordinal.Should().Be(i + 1);
            }
        }

        [Fact]
        public void MoveSubList_DeletedSubList_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.MoveSubList(deletedSubListId, 4)).Should().Throw<TodoSubListDeletedException>().And
                .SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(-99)]
        [InlineData(0)]
        [InlineData(99)]
        public void MoveSubList_OutOfRangeOrdinal_ThrowsArgumentException(int invalidOrdinal)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.MoveSubList(3, invalidOrdinal)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("destinationOrdinal");
        }

        [Fact]
        public void MoveSubList_NonExistentSubListId_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();
            var nonExistentSubListId = 99;

            fixture.Sut.Invoking(l => l.MoveSubList(nonExistentSubListId, 4)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }
    }
}
