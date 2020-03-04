using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Application.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoItemDtoTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var id = 1;
            var title = "Title";
            var description = "Description";
            var ordinal = 1;
            var isCompleted = true;
            var isDeleted = true;
            var dueDate = DateTime.Today;

            var todoItemDto = new TodoItemDto
            {
                Id = id,
                Title = title,
                Description = description,
                Ordinal = ordinal,
                IsComplated = isCompleted,
                IsDeleted = isDeleted,
                DueDate = dueDate
            };

            todoItemDto.Id.Should().Be(id);
            todoItemDto.Title.Should().Be(title);
            todoItemDto.Description.Should().Be(description);
            todoItemDto.Ordinal.Should().Be(ordinal);
            todoItemDto.IsComplated.Should().Be(isCompleted);
            todoItemDto.IsDeleted.Should().Be(isDeleted);
            todoItemDto.DueDate.Should().Be(dueDate);
        }
    }
}
