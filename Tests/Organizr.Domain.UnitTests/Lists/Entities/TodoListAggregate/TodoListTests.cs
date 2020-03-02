using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoListTests
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
            list.ResourceContributors.Should().NotBeNull();
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
            Func<TodoList> construct = () => new TodoList("User1", title);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var list = new TodoList("User1", "Todo List 1 Title", "Todo List 1 Description");

            var newTitle = "New Todo List 1";
            var newDescription = "New Todo List 1 Description";

            list.Edit(newTitle, newDescription);

            list.Title.Should().Be(newTitle);
            list.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Edit_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var list = new TodoList("User1", "Todo List 1 Title", "Todo List 1 Description");

            var newDescription = "New Todo List 1 Description";

            list.Invoking(l => l.Edit(title, newDescription)).Should().Throw<ArgumentException>().And.ParamName.Should()
                .Be("title");
        }

        [Fact]
        public void DueDate_ReturnsMostDistantTodoDueDate()
        {
            var fixture = new TodoListFixture();

            fixture.TodoList.DueDate.Should().Be(fixture.LastDueDate);
        }

        [Fact]
        public void DueDate_TodoListWithNoDueDateSet_ReturnsNull()
        {
            var todoList = new TodoListStub(Guid.NewGuid(), "User1", "Todo List Title");
            todoList.AddTodo("Todo Item Title");

            todoList.DueDate.Should().Be(null);
        }
    }
}
