using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class AddSubListTests
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
                .ParamName.Should().Be("title");
        }
    }
}
