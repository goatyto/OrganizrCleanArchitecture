using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoListDtoTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var id = Guid.NewGuid();
            var creatorUserId = "User1";
            var userGroupId = Guid.NewGuid();
            var title = "Title";
            var description = "Description";
            var subLists = new List<TodoSubListDto> { new TodoSubListDto() };
            var items = new List<TodoItemDto> { new TodoItemDto() };

            var sut = new TodoListDto
            {
                Id = id,
                CreatorUserId = creatorUserId,
                UserGroupId = userGroupId,
                Title = title,
                Description = description,
                SubLists = subLists,
                Items = items
            };

            sut.Id.Should().Be(id);
            sut.CreatorUserId.Should().Be(creatorUserId);
            sut.UserGroupId.Should().Be(userGroupId);
            sut.Title.Should().Be(title);
            sut.Description.Should().Be(description);
            sut.SubLists.Should().BeEquivalentTo(subLists);
            sut.Items.Should().BeEquivalentTo(items);
        }
    }
}
