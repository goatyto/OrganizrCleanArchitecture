using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class EditSubListTests
    {
        private readonly TodoListFixture _fixture;

        public EditSubListTests()
        {
            _fixture = new TodoListFixture();
        }

        [Fact]
        public void EditSubList_NewSubListData_SubListInfoChanges()
        {
            var subListId = 1;
            var editedSubList = _fixture.Sut.SubLists.Single(sl => sl.Id == subListId);
            var originalOrdinal = editedSubList.Ordinal;

            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            _fixture.Sut.EditSubList(subListId, newTitle, newDescription);


            editedSubList.Id.Should().Be(subListId);
            editedSubList.Title.Should().Be(newTitle);
            editedSubList.Ordinal.Should().Be(originalOrdinal);
            editedSubList.Description.Should().Be(newDescription);
            editedSubList.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void EditSubList_NonExistingSubListId_ThrowsInvalidOperationException()
        {
            var nonExistingSubListId = 99;
            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            _fixture.Sut.Invoking(l => l.EditSubList(nonExistingSubListId, newTitle, newDescription)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*sublist*{nonExistingSubListId}*does not exist*");
        }

        [Fact]
        public void EditSubList_DeletedSubListId_ThrowsInvalidOperationException()
        {
            var deletedSubListId = 2;
            var newTitle = "Todo Sub List";
            var newDescription = "Todo Sub List Description";

            _fixture.Sut.Invoking(l => l.EditSubList(deletedSubListId, newTitle, newDescription)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditSubList_NullOrWhiteSpaceTitle_ThrowsTodoListException(string newTitle)
        {
            var subListId = 1;
            var newDescription = "Todo Sub List Description";

            _fixture.Sut.Invoking(l => l.EditSubList(subListId, newTitle, newDescription)).Should()
                .Throw<TodoListException>().WithMessage("*title*cannot be empty*");
        }
    }
}
