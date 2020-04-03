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
            new object[]{1, 3, null },
            new object[]{4, 2, null },
            new object[]{1, 3, 1 },
            new object[]{4, 2, 1 },
            new object[]{5, 3, null },
            new object[]{9, 2, null },
        };

        [Theory, MemberData(nameof(MoveTodoValidTestData))]
        public void MoveTodo_ValidOrdinal_OrdinalChanged(int todoId, int destinationOrdinal, int? destinationSubListId)
        {
            var todoToBeMoved = _fixture.GetTodoItemById(todoId);
            var sourceSubListId = _fixture.GetSubListIdForTodoItem(todoId);

            _fixture.Sut.MoveTodo(todoId, destinationOrdinal, destinationSubListId);

            todoToBeMoved.Ordinal.Should().Be(destinationOrdinal);

            var sourceSubListItems =
                _fixture.GetTodoItems(sourceSubListId).Where(item => !item.IsDeleted)
                    .OrderBy(item => item.Ordinal).ToList();
            var destinationSubListItems =
                _fixture.Sut.Items.Where(item => !item.IsDeleted).
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
        public void MoveTodo_SameOrdinal_OrdinalDoesNotChange()
        {
            var todoId = 1;

            var todoToBeMoved = _fixture.Sut.Items.Single(item => item.Id == todoId);

            var sameOrdinal = todoToBeMoved.Ordinal;
            var sameSubListId = _fixture.GetSubListIdForTodoItem(todoId);

            _fixture.Sut.MoveTodo(todoId, sameOrdinal, sameSubListId);

            todoToBeMoved.Ordinal.Should().Be(sameOrdinal);

            for (int subListIndex = 0; subListIndex < _fixture.Sut.SubLists.Count; subListIndex++)
            {
                var activeItemsInSubList = _fixture.GetTodoItems(sameSubListId).Where(item => !item.IsDeleted)
                    .ToList();

                for (int todoItemIndex = 0; todoItemIndex < activeItemsInSubList.Count; todoItemIndex++)
                {
                    activeItemsInSubList[todoItemIndex].Ordinal.Should().Be(todoItemIndex + 1);
                }
            }

        }

        public static IEnumerable<object[]> MoveTodoInvalidOrdinalTestData = new[]
        {
            new object[]{1, 99, null },
            new object[]{4, 99, 1 },
            new object[]{9, 99, null }
        };

        [Theory, MemberData(nameof(MoveTodoInvalidOrdinalTestData))]
        public void MoveTodo_InvalidOrdinal_ThrowsTodoListException(int todoId, int invalidOrdinal, int? subListId)
        {
            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, invalidOrdinal, subListId)).Should()
                .Throw<TodoListException>().WithMessage("*ordinal*out of range*");
        }

        public static IEnumerable<object[]> MoveTodoDeletedTodoTestData = new[]
        {
            new object[]{3, 4, null },
            new object[]{3, 4, 1 },
            new object[]{8, 1, 1 },
            new object[]{8, 1, null },
        };

        [Theory, MemberData(nameof(MoveTodoDeletedTodoTestData))]
        public void MoveTodo_DeletedTodoId_ThrowsTodoListException(int deletedTodoId, int ordinal, int? subListId)
        {
            _fixture.Sut.Invoking(l => l.MoveTodo(deletedTodoId, ordinal, subListId)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedTodoId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_DeletedSourceSubList_ThrowsTodoListException()
        {
            var deletedSubListTodoId = 11;
            var newOrdinal = 1;
            int? newSubListId = null;

            _fixture.Sut.Invoking(l => l.MoveTodo(deletedSubListTodoId, newOrdinal, newSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*todo item*{deletedSubListTodoId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_DeletedDestinationSubList_ThrowsTodoListException()
        {
            var todoId = 1;
            var todoItemOrdinal = 1;
            var deletedSubListId = 2;

            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, todoItemOrdinal, deletedSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_NonExistentDestinationSubListId_ThrowsTodoListException()
        {
            var todoId = 1;
            var newOrdinal = 1;
            var nonExistentSubListId = 99;

            _fixture.Sut.Invoking(l => l.MoveTodo(todoId, newOrdinal, nonExistentSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }

        [Fact]
        public void MoveTodo_NonExistentTodoId_ThrowsTodoListException()
        {
            var nonExistentTodoId = 99;
            var newOrdinal = 1;
            var newSubListId = 1;

            _fixture.Sut.Invoking(l => l.MoveTodo(nonExistentTodoId, newOrdinal, newSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"todo item*{nonExistentTodoId}*does not exist*");
        }
    }
}
