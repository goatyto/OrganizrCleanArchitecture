using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Organizr.Domain.Lists.Entities;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Xunit;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoItemTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void AddTodo_ValidData_TodoItemAdded(int? subListId)
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.AddTodo(title, description, dueDate, subListId);

            var addedTodo = fixture.TodoList.Items.Last();

            addedTodo.Id.Should().Be(default(int));
            addedTodo.MainListId.Should().Be(fixture.TodoListId);
            addedTodo.Title.Should().Be(title);
            addedTodo.Description.Should().Be(description);
            addedTodo.DueDate.Should().Be(dueDate);
            addedTodo.Position.Should().Be(new TodoItemPosition(fixture.TodoList.Items.Count(item => item.Position.SubListId == subListId), subListId));
            addedTodo.IsCompleted.Should().Be(false);
            addedTodo.IsDeleted.Should().Be(false);
        }

        [Fact]
        public void AddTodo_NonExistantSubListId_ThrowsTodoSubListDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);
            var nonExistantSubListId = 99;

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, nonExistantSubListId)).Should()
                .Throw<TodoSubListDoesNotExistException>().And.SubListId.Should().Be(nonExistantSubListId);
        }

        [Fact]
        public void AddTodo_DeletedSubListId_ThrowsTodoSubListDeletedException()
        {
            var fixture = new TodoListFixture();

            var title = "Todo";
            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);
            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, deletedSubListId)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string title)
        {
            var fixture = new TodoListFixture();

            var description = "Todo Description";
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate)).Should()
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
            var dueDate = DateTime.Today.AddDays(1);

            fixture.TodoList.Invoking(l => l.AddTodo(title, description, dueDate, invalidSubListId)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("subListId");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public void EditTodo_ValidData_TodoEdited(int todoId)
        {
            var fixture = new TodoListFixture();

            var todoToBeEdited = fixture.TodoList.Items.Single(item => item.Id == todoId);
            var originalPosition = todoToBeEdited.Position;
            var originalIsCompleted = todoToBeEdited.IsCompleted;
            var originalIsDeleted = todoToBeEdited.IsDeleted;

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.EditTodo(todoId, newTitle, newDescription, newDueDate);


            todoToBeEdited.Id.Should().Be(todoId);
            todoToBeEdited.MainListId.Should().Be(fixture.TodoListId);
            todoToBeEdited.Title.Should().Be(newTitle);
            todoToBeEdited.Description.Should().Be(newDescription);
            todoToBeEdited.DueDate.Should().Be(newDueDate);
            todoToBeEdited.Position.Should().Be(originalPosition);
            todoToBeEdited.IsCompleted.Should().Be(originalIsCompleted);
            todoToBeEdited.IsDeleted.Should().Be(originalIsDeleted);
        }

        [Fact]
        public void EditTodo_NonExistingTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);
            var nonExistantTodoId = 99;

            fixture.TodoList.Invoking(l => l.EditTodo(nonExistantTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<TodoItemDoesNotExistException>().And.TodoId.Should().Be(nonExistantTodoId);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(7)]
        public void EditTodo_CompletedTodoId_ThrowsTodoItemCompletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<TodoItemCompletedException>().And.TodoId.Should().Be(todoId);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(8)]
        public void EditTodo_DeletedTodoId_ThrowsTodoItemDeletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<TodoItemDeletedException>().And.TodoId.Should().Be(todoId);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void EditTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int deletedSubListTodoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);
            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.EditTodo(deletedSubListTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EditTodo_NullOrWhiteSpaceTitle_ThrowsArgumentException(string newTitle)
        {
            var fixture = new TodoListFixture();

            var todoId = 1;
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(todoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("title");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void EditTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            var newTitle = "Todo";
            var newDescription = "Todo Description";
            var newDueDate = DateTime.Today.AddDays(5);

            fixture.TodoList.Invoking(l => l.EditTodo(invalidTodoId, newTitle, newDescription, newDueDate)).Should()
                .Throw<ArgumentException>().And.ParamName.Should().Be("todoId");
        }

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

            var editedTodo = fixture.TodoList.Items.Single(item => item.Id == todoId);

            fixture.TodoList.SetCompletedTodo(todoId, isCompleted);

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

            fixture.TodoList.Invoking(l => l.SetCompletedTodo(todoId, isCompleted)).Should()
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

            fixture.TodoList.Invoking(l => l.SetCompletedTodo(todoId, isCompleted)).Should()
                .Throw<TodoSubListDeletedException>().And.SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void SetCompletedTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            fixture.TodoList.Invoking(l => l.SetCompletedTodo(invalidTodoId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("todoId");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void DeleteTodo_ValidTodoId_TodoMarkedDeleted(int todoId)
        {
            var fixture = new TodoListFixture();

            var deletedTodo = fixture.TodoList.Items.Single(item => item.Id == todoId);

            fixture.TodoList.DeleteTodo(todoId);

            deletedTodo.IsDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteTodo_NonExistantTodoId_ThrowsTodoItemDoesNotExistException()
        {
            var fixture = new TodoListFixture();

            var nonExistantTodoId = 99;

            fixture.TodoList.Invoking(l => l.DeleteTodo(nonExistantTodoId)).Should().Throw<TodoItemDoesNotExistException>().And
                .TodoId.Should().Be(nonExistantTodoId);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        public void DeleteTodo_DeletedSubListTodoId_ThrowsTodoSubListDeletedException(int todoId)
        {
            var fixture = new TodoListFixture();

            var deletedSubListId = 2;

            fixture.TodoList.Invoking(l => l.DeleteTodo(todoId)).Should().Throw<TodoSubListDeletedException>().And
                .SubListId.Should().Be(deletedSubListId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeleteTodo_InvalidTodoId_ThrowsArgumentException(int invalidTodoId)
        {
            var fixture = new TodoListFixture();

            fixture.TodoList.Invoking(l => l.DeleteTodo(invalidTodoId)).Should().Throw<ArgumentException>().And
                .ParamName.Should().Be("todoId");
        }
    }
}
