using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class CreateSharedTodoListTests
    {
        [Fact]
        public void CreateSharedTodoList_ValidData_SharedTodoListCreated()
        {
            var fixture = new UserGroupFixture();

            var todoListId = Guid.NewGuid();
            var todoListCreatorUserId = "User1";
            var todoListTitle = "TodoListTitle";
            var todoListDescription = "TodoListDescription";

            var todoList = fixture.Sut.CreateSharedTodoList(todoListId, todoListCreatorUserId, todoListTitle, todoListDescription);

            todoList.Id.Should().Be(todoListId);
            todoList.CreatorUserId.Should().Be(todoListCreatorUserId);
            todoList.UserGroupId.Should().Be(fixture.UserGroupId);
            todoList.Title.Should().Be(todoListTitle);
            todoList.Description.Should().Be(todoListDescription);
        }

        [Fact]
        public void CreateSharedTodoList_DefaultTodoListId_ThrowsArgumentException()
        {
            var fixture = new UserGroupFixture();

            var defaultTodoListId = Guid.Empty;

            fixture.Sut.Invoking(s =>
                    s.CreateSharedTodoList(defaultTodoListId, "User1", "TodoListTitle", "TodoListDescription")).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateSharedTodoList_NullOrWhiteSpaceTodoListTitle_ThrowsArgumentException(string nullOrWhiteSpaceTodoListTitle)
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s =>
                    s.CreateSharedTodoList(Guid.NewGuid(), "User1", nullOrWhiteSpaceTodoListTitle, "TodoListDescription")).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Fact]
        public void CreateSharedTodoList_NonMemberTodoListCreatorUserId_ThrowsUserNotAMemberException()
        {
            var fixture = new UserGroupFixture();

            var invalidTodoListCreatorUserId = "User99";

            fixture.Sut
                .Invoking(s => s.CreateSharedTodoList(Guid.NewGuid(), invalidTodoListCreatorUserId, "TodoListTitle",
                    "TodoListDescription")).Should().Throw<UserNotAMemberException>().Where(exception =>
                    exception.UserGroupId == fixture.UserGroupId && exception.UserId == invalidTodoListCreatorUserId);
        }
    }
}
