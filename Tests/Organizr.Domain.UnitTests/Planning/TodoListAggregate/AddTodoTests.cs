using System;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class AddTodoTests
    {
        private readonly TodoListFixture _fixture;

        public AddTodoTests()
        {
            _fixture = new TodoListFixture();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void AddTodo_ValidData_TodoItemAdded(int? subListId)
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(1);

            var initialTodoListCount = _fixture.Sut.Items.Union(_fixture.Sut.SubLists.SelectMany(sl => sl.Items)).Count();

            _fixture.Sut.AddTodo(title, description, dueDateUtc, subListId);

            var addedTodo = _fixture.GetTodoItems(subListId).Last();

            addedTodo.Id.Should().Be(initialTodoListCount + 1);
            addedTodo.Title.Should().Be(title);
            addedTodo.Description.Should().Be(description);
            addedTodo.DueDateUtc.Should().Be(dueDateUtc);
            addedTodo.Ordinal.Should().Be(_fixture.GetTodoItems(subListId).Count(item => !item.IsDeleted));
            addedTodo.IsCompleted.Should().Be(false);
            addedTodo.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddTodo_NonExistentSubListId_ThrowsInvalidOperationException()
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(1);
            var nonExistentSubListId = 99;

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc, nonExistentSubListId)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }

        [Fact]
        public void AddTodo_DeletedSubListId_ThrowsInvalidOperationException()
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDate = _fixture.CreateClientDateUtcWithDaysOffset(1);
            var deletedSubListId = 2;

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate, deletedSubListId)).Should()
                .Throw<InvalidOperationException>().WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsTodoListException(string title)
        {
            var description = "Todo Description";
            var dueDate = _fixture.CreateClientDateUtcWithDaysOffset(1);

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate)).Should()
                .Throw<TodoListException>().WithMessage("*title*cannot be empty*");
        }

        [Fact]
        public void AddTodo_DueDateInThePast_ThrowsTodoListException()
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(-1);

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc)).Should().Throw<TodoListException>()
                .WithMessage("*due date*before client today*");
        }
    }
}
