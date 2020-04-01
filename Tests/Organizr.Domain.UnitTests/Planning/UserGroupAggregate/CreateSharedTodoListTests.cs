using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning;
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
            var todoListId = new TodoListId(Guid.NewGuid());
            var todoListCreatorUserId = new CreatorUser("User1");
            var todoListTitle = "TodoListTitle";
            var todoListDescription = "TodoListDescription";

            var todoList = _fixture.Sut.CreateSharedTodoList(todoListId, todoListCreatorUserId, todoListTitle, todoListDescription);

            todoList.TodoListId.Should().Be(todoListId);
            todoList.CreatorUser.Should().Be(todoListCreatorUserId);
            todoList.UserGroupId.Should().Be(_fixture.UserGroupId);
            todoList.Title.Should().Be(todoListTitle);
            todoList.Description.Should().Be(todoListDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateSharedTodoList_NullOrWhiteSpaceTodoListTitle_ThrowsArgumentException(string nullOrWhiteSpaceTodoListTitle)
        {
            _fixture.Sut.Invoking(s =>
                    s.CreateSharedTodoList((TodoListId)Guid.NewGuid(), (CreatorUser)"User1", nullOrWhiteSpaceTodoListTitle, "TodoListDescription")).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Fact]
        public void CreateSharedTodoList_NonMemberTodoListCreatorUserId_ThrowsUserGroupException()
        {
            var invalidTodoListCreatorUserId = new CreatorUser("User99");

            _fixture.Sut
                .Invoking(s => s.CreateSharedTodoList((TodoListId)Guid.NewGuid(), invalidTodoListCreatorUserId,
                    "TodoListTitle", "TodoListDescription")).Should().Throw<UserGroupException>()
                .WithMessage($"*user*{invalidTodoListCreatorUserId}*not a member*");
        }
    }
}
