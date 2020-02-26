﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class AddTodoTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void AddTodo_ValidData_TodoItemAdded(int? subListId)
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.AddTodo(title, description, dueDate, fixture.DateTimeProvider, subListId);

            var addedTodo = fixture.TodoList.Items.Last();

            addedTodo.Id.Should().Be(default(int));
            addedTodo.MainListId.Should().Be(fixture.TodoListId);
            addedTodo.Title.Should().Be(title);
            addedTodo.Description.Should().Be(description);
            addedTodo.DueDate.Should().Be(dueDate);
            addedTodo.Position.Should().Be(new TodoItemPosition(fixture.TodoList.Items.Count(item => item.Position.SubListId == subListId), subListId));
            addedTodo.IsCompleted.Should().Be(false);
            addedTodo.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddTodo_NonExistentSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);
            var nonExistentSubListId = 99;

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, fixture.DateTimeProvider, nonExistentSubListId)).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(nonExistentSubListId);
        }

        [Fact]
        public void AddTodo_DeletedSubListId_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);
            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, fixture.DateTimeProvider, deletedSubListId)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var fixture = new TodoListFixture();

            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void AddTodo_InvalidSubListId_ThrowsArgumentException(int invalidSubListId)
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, fixture.DateTimeProvider, invalidSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }

        [Fact]
        public void AddTodo_DueDateInThePast_ThrowsDueDateInThePastException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(-1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, fixture.DateTimeProvider)).Should()
                .Throw<DueDateInThePastException>().And.DueDate.Should().Be(dueDate);
        }

        [Fact]
        public void AddTodo_NullDateTimeProvider_ThrowsArgumentException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, null)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("dateTimeProvider");
        }
    }
}