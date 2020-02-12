using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Organizr.Domain.Lists.Entities;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoItemTests
    {
        public static IEnumerable<object[]> AddTodoSubListIds = new List<object[]>
        {
            new object[]{null},
            new object[]{Guid.NewGuid()}
        };

        [Theory, MemberData(nameof(AddTodoSubListIds))]
        public void AddTodo_TodoItem_TodoItemAdded(Guid? subListId)
        {
            var listId = Guid.NewGuid();
            var list = new TodoList(listId, "User1", "TodoList1");
            var originalItemCount = list.Items.Count;

            var todoId = Guid.NewGuid();
            var title = "Todo1";
            var description = "Description1";
            var dueDate = DateTime.Now.AddDays(1);

            if (subListId.HasValue)
            {
                list.AddSubList(subListId.Value, "SubList1");
                list.AddTodo(todoId, title, description, dueDate, subListId);
            }
            else
                list.AddTodo(todoId, title, description, dueDate);

            list.Items.Count.Should().Be(originalItemCount + 1);

            var insertedItem = list.Items.Last();

            insertedItem.Id.Should().Be(todoId);
            insertedItem.MainListId.Should().Be(listId);
            insertedItem.Title.Should().Be(title);
            insertedItem.Ordinal.Should().Be(list.Items.Count(item => item.SubListId == subListId));
            insertedItem.Description.Should().Be(description);
            insertedItem.DueDate.Should().Be(dueDate);
            insertedItem.SubListId.Should().Be(subListId);
        }

        [Fact]
        public void AddTodo_InvalidSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            var invalidSubListId = Guid.NewGuid();
            list.Invoking(l => l.AddTodo(Guid.NewGuid(), "TodoItem1", subListId: invalidSubListId)).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(invalidSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");
            list.Invoking(l => l.AddTodo(Guid.NewGuid(), title)).Should().Throw<ArgumentException>().And.ParamName
                .Should().Be(nameof(TodoItem.Title));
        }

        [Fact]
        public void DeleteTodo_ItemId_TodoShouldBeMarkedDeleted()
        {
            var list = new TodoList(Guid.NewGuid(), "User1", "TodoList1");

            var itemToBeDeleted = Guid.NewGuid();

            list.AddTodo(Guid.NewGuid(), "Todo1");
            list.AddTodo(itemToBeDeleted, "Todo2");

            list.DeleteTodo(itemToBeDeleted);

            list.Items.Single(item => item.Id == itemToBeDeleted).IsDeleted.Should().Be(true);
        }
    }
}
