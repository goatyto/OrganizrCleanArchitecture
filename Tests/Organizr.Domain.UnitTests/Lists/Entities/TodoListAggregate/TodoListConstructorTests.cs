using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoListConstructorTests
    {
        [Fact]
        public void TodoListConstructor_ValidData_ObjectInitializedProperly()
        {
            var ownerId = "User1";
            var title = "ListTitle1";
            var description = "ListDescription1";

            var list = new TodoList(ownerId, title, description);

            list.OwnerId.Should().Be(ownerId);
            list.Title.Should().Be(title);
            list.Description.Should().Be(description);
            list.Items.Should().NotBeNull();
            list.SubLists.Should().NotBeNull();
            list.OwnerId.Should().NotBeNullOrWhiteSpace();
            list.ContributorIds.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TodoListConstructor_NullOrWhiteSpaceOwnerId_ThrowsArgumentException(string ownerId)
        {
            Func<TodoList> construct = () => new TodoList(ownerId, "TodoList");

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be("ownerId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TodoListConstructor_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            Func<TodoList> construct = () => new TodoList( "User1", title);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }
    }
}
