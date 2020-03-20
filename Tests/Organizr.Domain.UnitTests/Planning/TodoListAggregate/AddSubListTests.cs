using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class AddSubListTests
    {
        [Fact]
        public void AddSubList_ValidData_SubListItemAdded()
        {
            var fixture = new TodoListFixture();

            var title = "Todo Sub List";
            var description = "Todo Sub List Description";

            var initialSubListCount = fixture.Sut.SubLists.Count;

            fixture.Sut.AddSubList(title, description);

            var insertedSubList = fixture.Sut.SubLists.Last();

            insertedSubList.Id.Should().Be(initialSubListCount + 1);
            insertedSubList.Title.Should().Be(title);
            insertedSubList.Ordinal.Should().Be(fixture.Sut.SubLists.Count(list => !list.IsDeleted));
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

            fixture.Sut.Invoking(l => l.AddSubList(title, description)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("title");
        }
    }
}
