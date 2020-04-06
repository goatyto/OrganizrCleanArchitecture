using System;
using FluentAssertions;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
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

            var sut = new TodoItemDto
            {
                Id = id,
                Title = title,
                Description = description,
                Ordinal = ordinal,
                IsCompleted = isCompleted,
                IsDeleted = isDeleted,
                DueDate = dueDate
            };

            sut.Id.Should().Be(id);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.Ordinal.Should().Be(ordinal);
            sut.IsCompleted.Should().Be(isCompleted);
            sut.IsDeleted.Should().Be(isDeleted);
            sut.DueDate.Should().Be(dueDate);
        }
    }
}
