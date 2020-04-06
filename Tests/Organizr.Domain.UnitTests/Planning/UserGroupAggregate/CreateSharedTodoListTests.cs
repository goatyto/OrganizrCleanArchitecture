using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class CreateSharedTodoListTests
    {
        private readonly UserGroupFixture _fixture;

        public CreateSharedTodoListTests()
        {
            _fixture = new UserGroupFixture();
        }

        [Fact]
        public void CreateSharedTodoList_ValidData_SharedTodoListCreated()
        {
            var todoListId = Guid.NewGuid();
            var todoListCreatorUserId = "User1";
            var todoListTitle = "TodoListTitle";
            var todoListDescription = "TodoListDescription";

            var todoList = _fixture.Sut.CreateSharedTodoList(todoListId, todoListCreatorUserId, todoListTitle, todoListDescription);

            todoList.Id.Should().Be(todoListId);
            todoList.CreatorUserId.Should().Be(todoListCreatorUserId);
            todoList.UserGroupId.Should().Be(_fixture.UserGroupId);
            todoList.Title.Should().Be(todoListTitle);
            todoList.Description.Should().Be(todoListDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateSharedTodoList_NullOrWhiteSpaceTodoListTitle_ThrowsTodoListException(string nullOrWhiteSpaceTodoListTitle)
        {
            var todoListId = Guid.NewGuid();
            var creatorUserId = "User1";
            var todoListDescription = "TodoListDescription";

            _fixture.Sut.Invoking(s => s.CreateSharedTodoList(todoListId, creatorUserId, nullOrWhiteSpaceTodoListTitle,
                    todoListDescription)).Should()
                .Throw<TodoListException>().WithMessage("*title*cannot be empty*");
        }

        [Fact]
        public void CreateSharedTodoList_NonMemberTodoListCreatorUserId_ThrowsInvalidOperationException()
        {
            var todoListId = Guid.NewGuid();
            var todoListTitle = "TodoListTitle";
            var todoListDescription = "TodoListDescription";
            var invalidTodoListCreatorUserId = "User99";

            _fixture.Sut
                .Invoking(s => s.CreateSharedTodoList(todoListId, invalidTodoListCreatorUserId,
                    todoListTitle, todoListDescription)).Should().Throw<InvalidOperationException>()
                .WithMessage($"*user*{invalidTodoListCreatorUserId}*not a member*");
        }
    }
}
