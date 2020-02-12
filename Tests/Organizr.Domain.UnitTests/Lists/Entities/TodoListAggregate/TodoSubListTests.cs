using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoSubListTests
    {
        [Fact]
        public void AddSubList_TodoSubListInfo_SubListItemAdded()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var originalItemCount = list.SubLists.Count;

            var subListId = Guid.NewGuid();
            var subListTitle = "TodoSubList1";
            var subListDescription = "TodoSubListDescription1";
            list.AddSubList(subListId, subListTitle, subListDescription);

            list.SubLists.Count.Should().Be(originalItemCount + 1);

            var insertedSubList = list.SubLists.Last();
            insertedSubList.Id.Should().Be(subListId);
            insertedSubList.Title.Should().Be(subListTitle);
            insertedSubList.Description.Should().Be(subListDescription);
            insertedSubList.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddSubList_DuplicateSubListId_ThrowsTodoSubListAlreadyExistsException()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var duplicatedSubListId = Guid.NewGuid();

            list.AddSubList(duplicatedSubListId, "TodoSubList1");
            list.Invoking(l => l.AddSubList(duplicatedSubListId, "TodoSubList2")).Should().Throw<TodoSubListAlreadyExistsException>()
                .And.SubListId.Should().Be(duplicatedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddSubList_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            list.Invoking(l => l.AddSubList(Guid.NewGuid(), title)).Should().Throw<ArgumentException>().And.ParamName
                .Should().Be(nameof(TodoList.Title));
        }

        [Fact]
        public void EditSubList_NewSubListInfo_SubListInfoChanges()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var subListId = Guid.NewGuid();
            list.AddSubList(subListId, "SubListTitle", "SubListDescription");
            var newSubListTitle = "NewSubListTitle";
            var newSubListDescription = "NewSubListDescription";

            list.EditSubList(subListId, newSubListTitle, newSubListDescription);

            var editedSubList = list.SubLists.Last();

            editedSubList.Id.Should().Be(subListId);
            editedSubList.Title.Should().Be(newSubListTitle);
            editedSubList.Description.Should().Be(newSubListDescription);
            editedSubList.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void EditSubList_InvalidSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var insertedSubListId = Guid.NewGuid();
            var invalidSubListId = Guid.NewGuid();
            list.AddSubList(insertedSubListId, "SubList1");
            list.Invoking(l => l.EditSubList(invalidSubListId, "NewSubList1")).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(invalidSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditSubList_NullOrWhiteSpaceTitle_ThrowsArgumentException(string newTitle)
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var subListId = Guid.NewGuid();
            list.AddSubList(subListId, "TodoSubList1");
            list.Invoking(l => l.EditSubList(subListId, newTitle)).Should().Throw<ArgumentException>().And.ParamName
                .Should().Be(nameof(TodoSubList.Title));
        }

        [Fact]
        public void DeleteSubList_SubListId_MarksSubListAsDeleted()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var subListId = Guid.NewGuid();

            list.AddSubList(subListId, "TodoSubList1");

            list.DeleteSubList(subListId);

            list.SubLists.Count.Should().Be(1);

            var insertedSubList = list.SubLists.Last();

            insertedSubList.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteSubList_InvalidSubListId_ThrowsSubListDoesNotExistException()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var subListId = Guid.NewGuid();
            var invalidListId = Guid.NewGuid();

            list.AddSubList(subListId, "TodoSubList1");

            list.Invoking(l => l.DeleteSubList(invalidListId)).Should().Throw<TodoSubListDoesNotExistException>().And
                .SubListId.Should().Be(invalidListId);
        }
    }
}
