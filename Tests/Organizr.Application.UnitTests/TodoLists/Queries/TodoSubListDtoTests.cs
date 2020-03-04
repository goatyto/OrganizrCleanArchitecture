using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Application.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoSubListDtoTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var id = 1;
            var title = "Title";
            var description = "Description";
            var ordinal = 1;
            var isDeleted = true;
            var items = new List<TodoItemDto> { new TodoItemDto() };

            var todoSubListDto = new TodoSubListDto
            {
                Id = id,
                Title = title,
                Description = description,
                Ordinal = ordinal,
                IsDeleted = isDeleted,
                Items = items
            };

            todoSubListDto.Id.Should().Be(id);
            todoSubListDto.Title.Should().Be(title);
            todoSubListDto.Description.Should().Be(description);
            todoSubListDto.Ordinal.Should().Be(ordinal);
            todoSubListDto.IsDeleted.Should().Be(isDeleted);
            todoSubListDto.Items.Should().BeEquivalentTo(items);
        }
    }
}
