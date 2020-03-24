using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
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
            var dueDateUtc = fixture.ClientDateTimeToday.AddDays(1);

            var initialTodoListCount = fixture.Sut.Items.Union(fixture.Sut.SubLists.SelectMany(sl => sl.Items)).Count();

            fixture.Sut.AddTodo(title, description, dueDateUtc, fixture.ClientTimeZoneOffsetInMinutes,
                fixture.ClientDateValidator, subListId);

            var addedTodo = fixture.GetTodoItems(subListId).Last();

            addedTodo.Id.Should().Be(initialTodoListCount + 1);
            addedTodo.Title.Should().Be(title);
            addedTodo.Description.Should().Be(description);
            addedTodo.DueDateUtc.Should().Be(dueDateUtc);
            addedTodo.Ordinal.Should().Be(fixture.GetTodoItems(subListId).Count(item => !item.IsDeleted));
            addedTodo.IsCompleted.Should().Be(false);
            addedTodo.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddTodo_NonExistentSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = fixture.ClientDateTimeToday.AddDays(1);
            var nonExistentSubListId = 99;

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc, fixture.ClientTimeZoneOffsetInMinutes,
                    fixture.ClientDateValidator, nonExistentSubListId)).Should().Throw<ArgumentException>().And
                .ParamName
                .Should().Be("subListId");
        }

        [Fact]
        public void AddTodo_DeletedSubListId_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = fixture.ClientDateTimeToday.AddDays(1);
            var deletedSubListId = 2;

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate, fixture.ClientTimeZoneOffsetInMinutes,
                    fixture.ClientDateValidator, deletedSubListId)).Should().Throw<TodoSubListDeletedException>().And
                .SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var fixture = new TodoListFixture();

            var description = "Todo Description";
            var dueDate = fixture.ClientDateTimeToday.AddDays(1);

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate)).Should()
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
            var dueDate = fixture.ClientDateTimeToday.AddDays(1);

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate, fixture.ClientTimeZoneOffsetInMinutes,
                    fixture.ClientDateValidator, invalidSubListId)).Should().Throw<ArgumentException>().And.ParamName
                .Should().Be("subListId");
        }

        [Fact]
        public void AddTodo_DueDateInThePast_ThrowsDueDateBeforeTodayException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = fixture.ClientDateTimeToday.AddDays(-1);

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc, fixture.ClientTimeZoneOffsetInMinutes,
                    fixture.ClientDateValidator)).Should().Throw<DueDateBeforeTodayException>().And.DueDate.Should()
                .Be(dueDateUtc);
        }

        [Fact]
        public void AddTodo_DueDateWithTimeComponent_ThrowsArgumentException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = new DateTime(fixture.ClientDateTimeToday.Ticks + 1, DateTimeKind.Utc);

            fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc, fixture.ClientTimeZoneOffsetInMinutes,
                    fixture.ClientDateValidator)).Should().Throw<ArgumentException>().And.ParamName.Should()
                .Be("dueDateUtc");
        }
    }
}
