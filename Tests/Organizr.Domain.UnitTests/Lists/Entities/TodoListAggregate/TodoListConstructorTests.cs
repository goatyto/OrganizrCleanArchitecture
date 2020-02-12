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
        public void TodoListConstructor_TodoListParameters_ObjectInitializedProperly()
        {
            var id = Guid.NewGuid();
            var ownerId = "User1";
            var title = "ListTitle1";
            var description = "ListDescription1";

            var list = new TodoList(id, ownerId, title, description);

            list.Id.Should().Be(id);
            list.OwnerId.Should().Be(ownerId);
            list.Title.Should().Be(title);
            list.Description.Should().Be(description);
            list.Items.Should().NotBeNull();
            list.SubLists.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TodoListConstructor_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            Func<TodoList> construct = () => new TodoList(Guid.NewGuid(), "User1", title);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be(nameof(TodoList.Title));
        }
    }
}
