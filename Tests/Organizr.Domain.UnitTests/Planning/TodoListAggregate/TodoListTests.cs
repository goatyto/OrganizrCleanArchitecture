using System;
using FluentAssertions;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListTests
    {
        [Fact]
        public void Create_ValidData_ObjectInitializedProperly()
        {
            var id = new TodoListId(Guid.NewGuid());
            var creatorUserId = new CreatorUser("User1");
            var title = "ListTitle1";
            var description = "ListDescription1";

            var sut = TodoList.Create(id, creatorUserId, title, description);

            sut.TodoListId.Should().Be(id);
            sut.CreatorUser.Should().Be(creatorUserId);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.Items.Should().NotBeNull();
            sut.SubLists.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            Func<TodoList> sut = () => TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), title);

            sut.Should().Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var sut = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Todo List 1 Title", "Todo List 1 Description");

            var newTitle = "New Todo List 1";
            var newDescription = "New Todo List 1 Description";

            sut.Edit(newTitle, newDescription);

            sut.Title.Should().Be(newTitle);
            sut.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Edit_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var sut = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Todo List 1 Title", "Todo List 1 Description");

            var newDescription = "New Todo List 1 Description";

            sut.Invoking(l => l.Edit(title, newDescription)).Should().Throw<ArgumentException>().And.ParamName.Should()
                .Be("title");
        }
    }
}
