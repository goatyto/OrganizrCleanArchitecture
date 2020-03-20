﻿using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class SetCompletedTodoTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public void SetCompletedTodo_ValidIdAndBool_CompletedStatusChanged(int todoId, bool isCompleted)
        {
            var fixture = new TodoListFixture();

            var editedTodo = fixture.Sut.Items.Single(item => item.Id == todoId);

            fixture.Sut.SetCompletedTodo(todoId, isCompleted);

            editedTodo.IsCompleted.Should().Be(isCompleted);
        }

        [Theory]
        [InlineData(3, true)]
        [InlineData(8, true)]
        [InlineData(3, false)]
        [InlineData(8, false)]
        public void SetCompletedTodo_DeletedTodoId_ThrowsTodoItemDeletedException(int todoId, bool isCompleted)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.SetCompletedTodo(todoId, isCompleted)).Should()
                .Throw<TodoItemDeletedException>().And.TodoId.Should().Be(todoId);
        }

        [Theory]
        [InlineData(11, true)]
        [InlineData(12, true)]
        [InlineData(13, true)]
        [InlineData(11, false)]
        [InlineData(12, false)]
        [InlineData(13, false)]
        public void SetCompletedTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int todoId, bool isCompleted)
        {
            var fixture = new TodoListFixture();

            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.SetCompletedTodo(todoId, isCompleted)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }
        
        [Fact]
        public void SetCompletedTodo_NonExistentTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentTodoId = 99;

            fixture.Sut.Invoking(l => l.SetCompletedTodo(nonExistentTodoId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }
    }
}