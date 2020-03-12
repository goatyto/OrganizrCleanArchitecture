﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.UserGroupAggregate
{
    public class SharedTodoListTests
    {
        [Fact]
        public void CreateSharedTodoList_ValidData_SharedTodoListAdded()
        {
            var fixture = new UserGroupFixture();
            var initialTodoListCount = fixture.Sut.SharedTodoLists.Count;

            var todoListId = Guid.NewGuid();
            var todoListCreatorUserId = "User1";
            var todoListTitle = "TodoListTitle";
            var todoListDescription = "TodoListDescription";

            var todoList = fixture.Sut.CreateSharedTodoList(todoListId, todoListCreatorUserId, todoListTitle, todoListDescription);

            fixture.Sut.SharedTodoLists.Should().HaveCount(initialTodoListCount + 1);

            var addedSharedList = fixture.Sut.SharedTodoLists.Last();

            addedSharedList.UserGroupId.Should().Be(fixture.UserGroupId);
            addedSharedList.TodoListId.Should().Be(todoListId);
            addedSharedList.IsDeleted.Should().Be(false);

            todoList.Id.Should().Be(todoListId);
            todoList.CreatorUserId.Should().Be(todoListCreatorUserId);
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
        public void CreateSharedTodoList_NullOrWhiteSpaceTodoListCreatorUserId_ThrowsArgumentException(string nullOrWhiteSpaceTodoListCreatorUserId)
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.Invoking(s =>
                    s.CreateSharedTodoList(Guid.NewGuid(), nullOrWhiteSpaceTodoListCreatorUserId, "TodoListTitle", "TodoListDescription")).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("creatorUserId");
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

        [Fact]
        public void CreateSharedTodoList_ExistingSharedTodoListId_ThrowsSharedResourceAlreadyExistsException()
        {
            var fixture = new UserGroupFixture();

            fixture.Sut
                .Invoking(s => s.CreateSharedTodoList(fixture.SharedTodoListId, "User1", "TodoListTitle",
                    "TodoListDescription")).Should().Throw<SharedResourceAlreadyExistsException<TodoList>>().Where(exception =>
                    exception.UserGroupId == fixture.UserGroupId && exception.SharedResourceId == fixture.SharedTodoListId);
        }

        [Fact]
        public void DeleteSharedTodoList_ValidData_SharedTodoListDeleted()
        {
            var fixture = new UserGroupFixture();

            fixture.Sut.DeleteSharedTodoList(fixture.SharedTodoListId);

            fixture.Sut.SharedTodoLists.Should()
                .ContainSingle(list => list.TodoListId == fixture.SharedTodoListId && list.IsDeleted);
        }

        [Fact]
        public void DeleteSharedTodoList_DefaultTodoListId_ThrowsArgumentException()
        {
            var fixture = new UserGroupFixture();

            var defaultTodoListId = Guid.Empty;

            fixture.Sut.Invoking(s => s.DeleteSharedTodoList(defaultTodoListId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("id");
        }

        [Fact]
        public void DeleteSharedTodoList_NonExistentTodoListId_ThrowsSharedResourceDoesNotExistException()
        {
            var fixture = new UserGroupFixture();

            var nonExistentTodoListId = Guid.NewGuid();

            fixture.Sut.Invoking(s => s.DeleteSharedTodoList(nonExistentTodoListId)).Should()
                .Throw<SharedResourceDoesNotExistException<TodoList>>().Where(exception =>
                    exception.UserGroupId == fixture.UserGroupId &&
                    exception.SharedResourceId == nonExistentTodoListId);
        }
    }
}
