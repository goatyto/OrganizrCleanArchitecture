using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListTests
    {
        [Fact]
        public void Create_ValidData_ObjectInitializedProperly()
        {
            var id = Guid.NewGuid();
            var creatorUserId = "User1";
            var title = "ListTitle1";
            var description = "ListDescription1";

            var sut = TodoList.Create(id, creatorUserId, title, description);

            sut.Id.Should().Be(id);
            sut.CreatorUserId.Should().Be(creatorUserId);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.Items.Should().NotBeNull();
            sut.SubLists.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_NullOrWhiteSpaceTitle_ThrowsTodoListException(string title)
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";

            Func<TodoList> sut = () => TodoList.Create(todoListId, creatorUserId, title);

            sut.Should().Throw<TodoListException>().WithMessage("*title*cannot be empty*");
        }

        [Fact]
        public void Edit_ValidData_StateChanges()
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";
            var todoListTitle = "Todo List 1 Title";
            var todoListDescription = "Todo List 1 Description";

            var sut = TodoList.Create(todoListId, creatorUserId, todoListTitle, todoListDescription);

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
        public void Edit_NullOrWhiteSpaceTitle_ThrowsTodoListException(string title)
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";
            var todoListTitle = "Todo List 1 Title";
            var todoListDescription = "Todo List 1 Description";

            var sut = TodoList.Create(todoListId, creatorUserId, todoListTitle, todoListDescription);

            var newDescription = "New Todo List 1 Description";

            sut.Invoking(l => l.Edit(title, newDescription)).Should().Throw<TodoListException>()
                .WithMessage("*title*cannot be empty*");
        }
    }
}
