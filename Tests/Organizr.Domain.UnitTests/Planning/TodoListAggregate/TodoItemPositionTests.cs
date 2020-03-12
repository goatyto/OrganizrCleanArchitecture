using System;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoItemPositionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(2)]
        public void Constructor_ValidData_ValueObjectInitialized(int? subListId)
        {
            var ordinal = 1;
            var todoItemPosition = new TodoItemPosition(ordinal, subListId);

            todoItemPosition.Ordinal.Should().Be(ordinal);
            todoItemPosition.SubListId.Should().Be(subListId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Constructor_InvalidOrdinal_ThrowsArgumentException(int invalidOrdinal)
        {
            var subListId = 1;
            Func<TodoItemPosition> construct = () => new TodoItemPosition(invalidOrdinal, subListId);

            construct.Should().Throw<ArgumentException>().And.ParamName.Should().Be("ordinal");
        }
    }
}
