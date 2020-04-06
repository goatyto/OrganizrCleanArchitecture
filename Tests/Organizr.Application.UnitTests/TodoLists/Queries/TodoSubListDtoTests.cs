using System.Collections.Generic;
using FluentAssertions;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
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

            var sut = new TodoSubListDto
            {
                Id = id,
                Title = title,
                Description = description,
                Ordinal = ordinal,
                IsDeleted = isDeleted,
                Items = items
            };

            sut.Id.Should().Be(id);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.Ordinal.Should().Be(ordinal);
            sut.IsDeleted.Should().Be(isDeleted);
            sut.Items.Should().BeEquivalentTo(items);
        }
    }
}
