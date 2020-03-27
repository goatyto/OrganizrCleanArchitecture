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
        private readonly TodoListFixture _fixture;

        public MoveTodoTests()
        {
            _fixture = new TodoListFixture();
        }

        public static IEnumerable<object[]> MoveTodoValidTestData = new[]
        {
            new object[]{1, new TodoItemPosition(3), null },
            new object[]{4, new TodoItemPosition(2), null },
            new object[]{1, new TodoItemPosition(3), 1 },
            new object[]{4, new TodoItemPosition(2), 1 },
            new object[]{5, new TodoItemPosition(3), null },
            new object[]{9, new TodoItemPosition(2), null },
        };

        [Theory, MemberData(nameof(MoveTodoValidTestData))]
        public void MoveTodo_ValidPosition_PositionChanged(int todoId, TodoItemPosition destinationPosition, int? destinationSubListId)
        {
            var todoToBeMoved = _fixture.GetTodoItemById(todoId);
            var sourceSubListId = _fixture.GetSubListIdForTodoItem(todoId);

            _fixture.Sut.MoveTodo(todoId, destinationPosition, destinationSubListId);

            todoToBeMoved.Position.Should().Be(destinationPosition);

            var sourceSubListItems =
                _fixture.GetTodoItems(sourceSubListId).Where(item => !item.IsDeleted)
                    .OrderBy(item => item.Position).ToList();
            var destinationSubListItems =
                _fixture.Sut.Items.Where(item => !item.IsDeleted).
                    OrderBy(item => item.Position).ToList();

            for (int i = 0; i < sourceSubListItems.Count; i++)
            {
                sourceSubListItems[i].Position.Should().Be(i + 1);
            }

            for (int i = 0; i < destinationSubListItems.Count; i++)
            {
                destinationSubListItems[i].Position.Should().Be(i + 1);
            }
        }

        [Fact]
        public void MoveTodo_SamePosition_PositionDoesNotChange()
        {
            var todoId = 1;

            var todoToBeMoved = _fixture.Sut.Items.Single(item => item.Id == todoId);

            var samePosition = todoToBeMoved.Position;
            var sameSubListId = _fixture.GetSubListIdForTodoItem(todoId);

            _fixture.Sut.MoveTodo(todoId, samePosition, sameSubListId);

            todoToBeMoved.Position.Should().Be(samePosition);

            for (int subListIndex = 0; subListIndex < _fixture.Sut.SubLists.Count; subListIndex++)
            {
                var activeItemsInSubList = _fixture.GetTodoItems(sameSubListId).Where(item => !item.IsDeleted)
                    .ToList();

                for (int todoItemIndex = 0; todoItemIndex < activeItemsInSubList.Count; todoItemIndex++)
                {
                    activeItemsInSubList[todoItemIndex].Position.Should().Be(todoItemIndex + 1);
                }
            }

        }

        public static IEnumerable<object[]> MoveTodoInvalidPositionTestData = new[]
        {
            new object[]{1, new TodoItemPosition(99), null },
            new object[]{4, new TodoItemPosition(99), 1 },
            new object[]{9, new TodoItemPosition(99), null }
        };

        [Theory, MemberData(nameof(MoveTodoInvalidPositionTestData))]
        public void MoveTodo_InvalidOrdinal_ThrowsTodoListException(int todoId, TodoItemPosition invalidPosition, int? subListId)
        {
            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, invalidPosition, subListId)).Should()
                .Throw<TodoListException>().WithMessage("*position*out of range*");
        }

        public static IEnumerable<object[]> MoveTodoDeletedTodoTestData = new[]
        {
            new object[]{3, new TodoItemPosition(4), null },
            new object[]{3, new TodoItemPosition(4), 1 },
            new object[]{8, new TodoItemPosition(1), 1 },
            new object[]{8, new TodoItemPosition(1), null },
        };

        [Theory, MemberData(nameof(MoveTodoDeletedTodoTestData))]
        public void MoveTodo_DeletedTodoId_ThrowsTodoListException(int deletedTodoId, TodoItemPosition position, int? subListId)
        {
            _fixture.Sut.Invoking(l => l.MoveTodo(deletedTodoId, position, subListId)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedTodoId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_DeletedSourceSubList_ThrowsTodoListException()
        {
            var deletedSubListTodoId = 11;
            var newPosition = new TodoItemPosition(1);
            int? newSubListId = null;

            _fixture.Sut.Invoking(l => l.MoveTodo(deletedSubListTodoId, newPosition, newSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedSubListTodoId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_DeletedDestinationSubList_ThrowsTodoListException()
        {
            var todoId = 1;
            var todoItemPosition = new TodoItemPosition(1);
            var deletedSubListId = 2;

            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, todoItemPosition, deletedSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_NonExistentDestinationSubListId_ThrowsTodoListException()
        {
            var todoId = 1;
            var newPosition = new TodoItemPosition(1);
            var nonExistentSubListId = 99;

            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, newPosition, nonExistentSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_NonExistentTodoId_ThrowsTodoListException()
        {
            var nonExistentTodoId = 99;
            var newPosition = new TodoItemPosition(1);
            var newSubListId = 1;

            _fixture.Sut.Invoking(l => l.MoveTodo(nonExistentTodoId, newPosition, newSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"todo item*{nonExistentTodoId}*does not exist*");
        }
    }
}
