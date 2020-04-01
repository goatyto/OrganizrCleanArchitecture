using System;
using System.Collections.Generic;
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

        public static IEnumerable<object[]> MoveSubListValidMemberData = new List<object[]>
        {
            new object[]{(TodoSubListId)1, (TodoSubListPosition)4},
            new object[]{(TodoSubListId)3, (TodoSubListPosition)3},
            new object[]{(TodoSubListId)5, (TodoSubListPosition)2},
            new object[]{(TodoSubListId)3, (TodoSubListPosition)1},
            new object[]{(TodoSubListId)4, (TodoSubListPosition)3}
        };

        [Theory, MemberData(nameof(MoveSubListValidMemberData))]
        public void MoveSubList_ValidPosition_SubListOrdinalChanges(TodoSubListId subListId, TodoSubListPosition destinationPosition)
        {
            var subListToBeMoved = _fixture.Sut.SubLists.Single(sl => sl.TodoSubListId == subListId);

            _fixture.Sut.MoveSubList(subListId, destinationPosition);

            subListToBeMoved.Position.Should().Be(destinationPosition);

            var resultSubListOrdinals =
                _fixture.Sut.SubLists.Where(sl => !sl.IsDeleted).OrderBy(sl => sl.Position).ToList();

            for (int i = 0; i < resultSubListOrdinals.Count; i++)
            {
                resultSubListOrdinals[i].Position.Should().Be((TodoSubListPosition)(i + 1));
            }
        }

        [Fact]
        public void MoveSubList_DeletedSubList_ThrowsTodoListException()
        {
            var deletedSubListId = (TodoSubListId)2;

            _fixture.Sut.Invoking(l => l.MoveSubList(deletedSubListId, (TodoSubListPosition)4)).Should().Throw<TodoListException>()
                .WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Fact]
        public void MoveSubList_OutOfRangeOrdinal_ThrowsTodoListException()
        {
            var invalidPosition = (TodoSubListPosition)99;

            _fixture.Sut.Invoking(l => l.MoveSubList((TodoSubListId)3, invalidPosition)).Should().Throw<TodoListException>()
                .WithMessage("*ordinal*out of range*");
        }

        [Fact]
        public void MoveSubList_NonExistentSubListId_ThrowsTodoListException()
        {
            var nonExistentSubListId = (TodoSubListId)99;

            _fixture.Sut.Invoking(l => l.MoveSubList(nonExistentSubListId, (TodoSubListPosition)4)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }
    }
}
