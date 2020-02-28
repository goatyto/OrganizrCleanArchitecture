using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class EditSubListTests
    {
        [Fact]
        public void EditSubList_NewSubListData_SubListInfoChanges()
        {
            var fixture = new TodoListFixture();

            var subListId = 1;
            var editedSubList = fixture.TodoList.SubLists.Single(sl => sl.Id == subListId);
            var originalOrdinal = editedSubList.Ordinal;

            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.EditSubList(subListId, newTitle, newDescription);


            editedSubList.Id.Should().Be(subListId);
            editedSubList.Title.Should().Be(newTitle);
            editedSubList.Ordinal.Should().Be(originalOrdinal);
            editedSubList.Description.Should().Be(newDescription);
            editedSubList.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void EditSubList_NonExistingSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistingSubListId = 99;
            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.EditSubList(nonExistingSubListId, newTitle, newDescription)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }

        [Fact]
        public void EditSubList_DeletedSubListId_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var deletedSubListId = 2;
            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.EditSubList(deletedSubListId, newTitle, newDescription)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditSubList_NullOrWhiteSpaceTitle_ThrowsArgumentException(string newTitle)
        {
            var fixture = new TodoListFixture();

            var subListId = 1;
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.EditSubList(subListId, newTitle, newDescription)).Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("title");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void EditSubList_InvalidSubListId_ThrowsArgumentException(int invalidSubListId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.EditSubList(invalidSubListId, newTitle, newDescription)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }
    }
}
