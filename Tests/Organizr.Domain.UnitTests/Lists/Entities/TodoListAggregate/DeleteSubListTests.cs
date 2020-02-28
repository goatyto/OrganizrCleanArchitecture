using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class DeleteSubListTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteSubList_SubListId_MarksSubListAsDeleted(int subListId)
        {
            var fixture = new TodoListFixture();

            var subListToBeDeleted = fixture.TodoList.SubLists.Single(sl => sl.Id == subListId);

            fixture.TodoList.DeleteSubList(subListId);

            subListToBeDeleted.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteSubList_NonExistentSubListId_ThrowsSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentSubListId = 99;

            fixture.TodoList.Invoking(l => l.DeleteSubList(nonExistentSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeleteSubList_InvalidSubListId_ThrowsArgumentException(int invalidSubListId)
        {
            var fixture = new TodoListFixture();

            fixture.TodoList.Invoking(l => l.DeleteSubList(invalidSubListId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("subListId");
        }
    }
}
