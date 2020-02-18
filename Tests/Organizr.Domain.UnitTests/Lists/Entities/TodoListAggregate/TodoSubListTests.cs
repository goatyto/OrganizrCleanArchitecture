using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoSubListTests
    {
        [Fact]
        public void AddSubList_ValidData_SubListItemAdded()
        {
            var fixture = new TodoListFixture();

            var title = "Todo Sub List";
            var description = "Todo Sub List Description";

            fixture.TodoList.AddSubList(title, description);

            var insertedSubList = fixture.TodoList.SubLists.Last();

            insertedSubList.Id.Should().Be(default(int));
            insertedSubList.Title.Should().Be(title);
            insertedSubList.Ordinal.Should().Be(fixture.TodoList.SubLists.Count);
            insertedSubList.Description.Should().Be(description);
            insertedSubList.IsDeleted.Should().Be(false);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddSubList_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var fixture = new TodoListFixture();

            var description = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.AddSubList(title, description)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be(nameof(TodoSubList.Title));
        }

        [Fact]
        public void EditSubList_NewSubListData_SubListInfoChanges()
        {
            var fixture = new TodoListFixture();

            var subListId = 1;
            var editedSubList = fixture.TodoList.SubLists.Single(sl=>sl.Id == subListId);
            var currentOrdinal = editedSubList.Ordinal;

            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.EditSubList(subListId, newTitle, newDescription);


            editedSubList.Id.Should().Be(subListId);
            editedSubList.Title.Should().Be(newTitle);
            editedSubList.Ordinal.Should().Be(currentOrdinal);
            editedSubList.Description.Should().Be(newDescription);
            editedSubList.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void EditSubList_NonExistingSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistingSubListId = 3;
            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            fixture.TodoList.Invoking(l => l.EditSubList(nonExistingSubListId, newTitle, newDescription)).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(nonExistingSubListId);
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
                .And.ParamName.Should().Be(nameof(TodoSubList.Title));
        }

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
        public void DeleteSubList_NonExistingSubListId_ThrowsSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistingSubListId = 3;

            fixture.TodoList.Invoking(l => l.DeleteSubList(nonExistingSubListId)).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(nonExistingSubListId);
        }
    }
}
