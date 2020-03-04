using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Application.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoListDtoTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var id = Guid.NewGuid();
            var title = "Title";
            var description = "Description";
            var subLists = new List<TodoSubListDto> { new TodoSubListDto() };
            var items = new List<TodoItemDto> { new TodoItemDto() };

            var sut = new TodoListDto
            {
                Id = id,
                Title = title,
                Description = description,
                SubLists = subLists,
                Items = items
            };

            sut.Id.Should().Be(id);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.SubLists.Should().BeEquivalentTo(subLists);
            sut.Items.Should().BeEquivalentTo(items);
        }
    }
}
