using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;
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

        public static IEnumerable<object[]> AddTodoValidMemberData = new List<object[]>
        {
            new object[] {null},
            new object[] {(TodoSubListId) 1}
        };

        [Theory, MemberData(nameof(AddTodoValidMemberData))]
        public void AddTodo_ValidData_TodoItemAdded(TodoSubListId subListId)
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(1);

            var initialTodoListCount = _fixture.Sut.Items.Union(_fixture.Sut.SubLists.SelectMany(sl => sl.Items)).Count();

            _fixture.Sut.AddTodo(title, description, dueDateUtc, subListId);

            var addedTodo = _fixture.GetTodoItems(subListId).Last();

            addedTodo.TodoItemId.Should().Be((TodoItemId)(initialTodoListCount + 1));
            addedTodo.Title.Should().Be(title);
            addedTodo.Description.Should().Be(description);
            addedTodo.DueDateUtc.Should().Be(dueDateUtc);
            addedTodo.Position.Should().Be((TodoItemPosition)_fixture.GetTodoItems(subListId).Count(item => !item.IsDeleted));
            addedTodo.IsCompleted.Should().Be(false);
            addedTodo.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddTodo_NonExistentSubListId_ThrowsTodoListException()
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDateUtc = _fixture.CreateClientDateUtcWithDaysOffset(1);
            var nonExistentSubListId = (TodoSubListId)99;

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDateUtc, nonExistentSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{nonExistentSubListId}*does not exist*");
        }

        [Fact]
        public void AddTodo_DeletedSubListId_ThrowsTodoListException()
        {
            var title = "Todo";
            var description = "Todo Description";
            var dueDate = _fixture.CreateClientDateUtcWithDaysOffset(1);
            var deletedSubListId = (TodoSubListId)2;

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate, deletedSubListId)).Should()
                .Throw<TodoListException>().WithMessage($"*sublist*{deletedSubListId}*does not exist*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var description = "Todo Description";
            var dueDate = _fixture.CreateClientDateUtcWithDaysOffset(1);

            _fixture.Sut.Invoking(l => l.AddTodo(title, description, dueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
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
