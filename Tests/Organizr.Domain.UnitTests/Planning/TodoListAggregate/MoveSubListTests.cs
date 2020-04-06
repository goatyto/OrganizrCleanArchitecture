using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class MoveSubListTests
    {
        private readonly TodoListFixture _fixture;

        public MoveSubListTests()
        {
            _fixture = new TodoListFixture();
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(3, 3)]
        [InlineData(5, 2)]
        [InlineData(3, 1)]
        [InlineData(4, 3)]
        public void MoveSubList_ValidOrdinal_SubListOrdinalChanges(int subListId, int targetOrdinal)
        {
            var subListToBeMoved = _fixture.Sut.SubLists.Single(sl => sl.Id == subListId);

            _fixture.Sut.MoveSubList(subListId, targetOrdinal);

            subListToBeMoved.Ordinal.Should().Be(targetOrdinal);

            var resultSubListOrdinals =
                _fixture.Sut.SubLists.Where(sl => !sl.IsDeleted).OrderBy(sl => sl.Ordinal).ToList();

            for (int i = 0; i < resultSubListOrdinals.Count; i++)
            {
                resultSubListOrdinals[i].Ordinal.Should().Be(i + 1);
            }
        }

        [Fact]
        public void MoveSubList_DeletedSubList_ThrowsInvalidOperationException()
        {
            var deletedSubListId = 2;
            var destinationOrdinal = 4;

            _fixture.Sut.Invoking(l => l.MoveSubList(deletedSubListId, destinationOrdinal)).Should().Throw<InvalidOperationException>()
                .WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Fact]
        public void MoveSubList_OutOfRangeOrdinal_ThrowsInvalidOperationException()
        {
            var subListId = 3;
            var invalidOrdinal = 99;

            _fixture.Sut.Invoking(l => l.MoveSubList(subListId, invalidOrdinal)).Should().Throw<InvalidOperationException>()
                .WithMessage("*ordinal*out of range*");
        }

        [Fact]
        public void MoveSubList_NonExistentSubListId_ThrowsInvalidOperationException()
        {
            var nonExistentSubListId = 99;
            var destinationOrdinal = 4;

            _fixture.Sut.Invoking(l => l.MoveSubList(nonExistentSubListId, destinationOrdinal)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }
    }
}
