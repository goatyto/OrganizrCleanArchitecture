using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class MoveTodoTests
    {
        public static IEnumerable<object[]> MoveTodoValidTestData = new[]
        {
            new object[]{1, new TodoItemPosition(3, null) },
            new object[]{4, new TodoItemPosition(2, null) },
            new object[]{1, new TodoItemPosition(3, 1) },
            new object[]{4, new TodoItemPosition(2, 1) },
            new object[]{5, new TodoItemPosition(3, null) },
            new object[]{9, new TodoItemPosition(2, null) },
        };

        [Theory, MemberData(nameof(MoveTodoValidTestData))]
        public void MoveTodo_ValidPosition_PositionChanged(int todoId, TodoItemPosition newPosition)
        {
            var fixture = new TodoListFixture();

            var todoToBeMoved = fixture.Sut.Items.Single(item => item.Id == todoId);
            var sourceSubListId = todoToBeMoved.Position.SubListId;
            var destinationSubListId = newPosition.SubListId;

            fixture.Sut.MoveTodo(todoId, newPosition);

            todoToBeMoved.Position.Should().Be(newPosition);

            var sourceSubListItems =
                fixture.Sut.Items.Where(item => !item.IsDeleted && item.Position.SubListId == sourceSubListId)
                    .OrderBy(item => item.Position.Ordinal).ToList();
            var destinationSubListItems =
                fixture.Sut.Items.Where(item => !item.IsDeleted && item.Position.SubListId == destinationSubListId).
                    OrderBy(item => item.Position.Ordinal).ToList();

            for (int i = 0; i < sourceSubListItems.Count; i++)
            {
                sourceSubListItems[i].Position.Ordinal.Should().Be(i + 1);
            }

            for (int i = 0; i < destinationSubListItems.Count; i++)
            {
                destinationSubListItems[i].Position.Ordinal.Should().Be(i + 1);
            }
        }

        [Fact]
        public void MoveTodo_SamePosition_PositionDoesNotChange()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;

            var todoToBeMoved = fixture.Sut.Items.Single(item => item.Id == todoId);

            var samePosition = (TodoItemPosition)todoToBeMoved.Position.GetCopy();

            fixture.Sut.MoveTodo(todoId, samePosition);

            todoToBeMoved.Position.Should().Be(samePosition);

            for (int subListIndex = 0; subListIndex < fixture.Sut.SubLists.Count; subListIndex++)
            {
                var activeItemsInSubList = fixture.Sut.Items.Where(item =>
                        !item.IsDeleted && item.Position.SubListId == fixture.Sut.SubLists.ElementAt(subListIndex).Id)
                    .ToList();

                for (int todoItemIndex = 0; todoItemIndex < activeItemsInSubList.Count; todoItemIndex++)
                {
                    activeItemsInSubList[todoItemIndex].Position.Ordinal.Should().Be(todoItemIndex + 1);
                }
            }

        }

        public static IEnumerable<object[]> MoveTodoInvalidPositionTestData = new[]
        {
            new object[]{1, new TodoItemPosition(99, null) },
            new object[]{4, new TodoItemPosition(99, 1) },
            new object[]{9, new TodoItemPosition(99, null) }
        };

        [Theory, MemberData(nameof(MoveTodoInvalidPositionTestData))]
        public void MoveTodo_InvalidPosition_ThrowsArgumentException(int todoId, TodoItemPosition invalidPosition)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, invalidPosition)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("newPosition");
        }

        public static IEnumerable<object[]> MoveTodoDeletedTodoTestData = new[]
        {
            new object[]{3, new TodoItemPosition(4, null) },
            new object[]{3, new TodoItemPosition(4, 1) },
            new object[]{8, new TodoItemPosition(1, 1) },
            new object[]{8, new TodoItemPosition(1, null) },
        };

        [Theory, MemberData(nameof(MoveTodoDeletedTodoTestData))]
        public void MoveTodo_DeletedTodoId_ThrowsTodoItemDeletedException(int deletedTodoId, TodoItemPosition newPosition)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.MoveTodo(deletedTodoId, newPosition)).Should()
                .Throw<TodoItemDeletedException>().And.TodoId.Should().Be(deletedTodoId);
        }

        [Fact]
        public void MoveTodo_DeletedSourceSubList_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var deletedSubListTodoId = 11;
            var newPosition = new TodoItemPosition(1, null);
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.MoveTodo(deletedSubListTodoId, newPosition)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Fact]
        public void MoveTodo_DeletedDestinationSubList_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newPosition = new TodoItemPosition(1, 2);
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, newPosition)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Fact]
        public void MoveTodo_NonExistentDestinationSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var nonExistentSubListId = 99;
            var newPosition = new TodoItemPosition(1, nonExistentSubListId);

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, newPosition)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("SubListId");
        }

        [Fact]
        public void MoveTodo_NonExistentTodoId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentTodoId = 99;
            var newPosition = new TodoItemPosition(1, 1);

            fixture.Sut.Invoking(l => l.MoveTodo(nonExistentTodoId, newPosition)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }
    }
}
