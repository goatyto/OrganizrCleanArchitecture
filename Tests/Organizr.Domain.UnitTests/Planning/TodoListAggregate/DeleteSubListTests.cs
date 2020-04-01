using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class DeleteSubListTests
    {
        private readonly TodoListFixture _fixture;

        public DeleteSubListTests()
        {
            _fixture = new TodoListFixture();
        }

        [Fact]
        public void DeleteSubList_SubListId_MarksSubListAsDeleted()
        {
            var subListId = 1;

            var subListToBeDeleted = _fixture.Sut.SubLists.Single(sl => sl.Id == subListId);

            _fixture.Sut.DeleteSubList(subListId);

            subListToBeDeleted.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteSubList_NonExistentSubListId_ThrowsTodoListException()
        {
            var nonExistentSubListId = 99;

            _fixture.Sut.Invoking(l => l.DeleteSubList(nonExistentSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }
    }
}
