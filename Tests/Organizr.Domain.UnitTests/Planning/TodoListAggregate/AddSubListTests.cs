using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class AddSubListTests
    {
        private readonly TodoListFixture _fixture;

        public AddSubListTests()
        {
            _fixture = new TodoListFixture();
        }

        [Fact]
        public void AddSubList_ValidData_SubListItemAdded()
        {
            var title = "Todo Sub List";
            var description = "Todo Sub List Description";

            var initialSubListCount = _fixture.Sut.SubLists.Count;

            _fixture.Sut.AddSubList(title, description);

            var insertedSubList = _fixture.Sut.SubLists.Last();

            insertedSubList.Id.Should().Be(initialSubListCount + 1);
            insertedSubList.Title.Should().Be(title);
            insertedSubList.Ordinal.Should().Be(_fixture.Sut.SubLists.Count(list => !list.IsDeleted));
            insertedSubList.Description.Should().Be(description);
            insertedSubList.IsDeleted.Should().Be(false);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddSubList_NullOrWhiteSpaceTitle_ThrowsTodoListException(string title)
        {
            var description = "Todo Sub List Description";

            _fixture.Sut.Invoking(l => l.AddSubList(title, description)).Should().Throw<TodoListException>()
                .WithMessage("*title*cannot be empty*");
        }
    }
}
