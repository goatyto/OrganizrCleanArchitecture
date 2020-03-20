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
            new object[]{1, 3, null },
            new object[]{4, 2, null },
            new object[]{1, 3, 1 },
            new object[]{4, 2, 1 },
            new object[]{5, 3, null },
            new object[]{9, 2, null },
        };

        [Theory, MemberData(nameof(MoveTodoValidTestData))]
        public void MoveTodo_ValidPosition_PositionChanged(int todoId, int destinationOrdinal, int? destinationSubListId)
        {
            var fixture = new TodoListFixture();

            var todoToBeMoved = fixture.Sut.Items.Single(item => item.Id == todoId);
            var sourceSubListId = todoToBeMoved.SubListId;

            fixture.Sut.MoveTodo(todoId, destinationOrdinal, destinationSubListId);

            todoToBeMoved.Ordinal.Should().Be(destinationOrdinal);
            todoToBeMoved.SubListId.Should().Be(destinationSubListId);

            var sourceSubListItems =
                fixture.Sut.Items.Where(item => !item.IsDeleted && item.SubListId == sourceSubListId)
                    .OrderBy(item => item.Ordinal).ToList();
            var destinationSubListItems =
                fixture.Sut.Items.Where(item => !item.IsDeleted && item.SubListId == destinationSubListId).
                    OrderBy(item => item.Ordinal).ToList();

            for (int i = 0; i < sourceSubListItems.Count; i++)
            {
                sourceSubListItems[i].Ordinal.Should().Be(i + 1);
            }

            for (int i = 0; i < destinationSubListItems.Count; i++)
            {
                destinationSubListItems[i].Ordinal.Should().Be(i + 1);
            }
        }

        [Fact]
        public void MoveTodo_SamePosition_PositionDoesNotChange()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;

            var todoToBeMoved = fixture.Sut.Items.Single(item => item.Id == todoId);

            var sameOrdinal = todoToBeMoved.Ordinal;
            var sameSubListId = todoToBeMoved.SubListId;

            fixture.Sut.MoveTodo(todoId, sameOrdinal, sameSubListId);

            todoToBeMoved.Ordinal.Should().Be(sameOrdinal);
            todoToBeMoved.SubListId.Should().Be(sameSubListId);

            for (int subListIndex = 0; subListIndex < fixture.Sut.SubLists.Count; subListIndex++)
            {
                var activeItemsInSubList = fixture.Sut.Items.Where(item =>
                        !item.IsDeleted && item.SubListId == fixture.Sut.SubLists.ElementAt(subListIndex).Id)
                    .ToList();

                for (int todoItemIndex = 0; todoItemIndex < activeItemsInSubList.Count; todoItemIndex++)
                {
                    activeItemsInSubList[todoItemIndex].Ordinal.Should().Be(todoItemIndex + 1);
                }
            }

        }

        public static IEnumerable<object[]> MoveTodoInvalidPositionTestData = new[]
        {
            new object[]{1, 99, null },
            new object[]{4, 99, 1 },
            new object[]{9, 99, null }
        };

        [Theory, MemberData(nameof(MoveTodoInvalidPositionTestData))]
        public void MoveTodo_InvalidOrdinal_ThrowsArgumentException(int todoId, int invalidOrdinal, int? subListId)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, invalidOrdinal, subListId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("newOrdinal");
        }

        public static IEnumerable<object[]> MoveTodoDeletedTodoTestData = new[]
        {
            new object[]{3, 4, null },
            new object[]{3, 4, 1 },
            new object[]{8, 1, 1 },
            new object[]{8, 1, null },
        };

        [Theory, MemberData(nameof(MoveTodoDeletedTodoTestData))]
        public void MoveTodo_DeletedTodoId_ThrowsTodoItemDeletedException(int deletedTodoId, int ordinal, int? subListId)
        {
            var fixture = new TodoListFixture();

            fixture.Sut.Invoking(l => l.MoveTodo(deletedTodoId, ordinal, subListId)).Should()
                .Throw<TodoItemDeletedException>().And.TodoId.Should().Be(deletedTodoId);
        }

        [Fact]
        public void MoveTodo_DeletedSourceSubList_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var deletedSubListTodoId = 11;
            var newOrdinal = 1;
            int? newSubListId = null;
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.MoveTodo(deletedSubListTodoId, newOrdinal, newSubListId)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Fact]
        public void MoveTodo_DeletedDestinationSubList_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newOrdinal = 1;
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, newOrdinal, deletedSubListId)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Fact]
        public void MoveTodo_NonExistentDestinationSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newOrdinal = 1;
            var nonExistentSubListId = 99;

            fixture.Sut.Invoking(l => l.MoveTodo(todoId, newOrdinal, nonExistentSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("newSubListId");
        }

        [Fact]
        public void MoveTodo_NonExistentTodoId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistentTodoId = 99;
            var newOrdinal = 1;
            var newSubListId = 1;

            fixture.Sut.Invoking(l => l.MoveTodo(nonExistentTodoId, newOrdinal, newSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }
    }
}
