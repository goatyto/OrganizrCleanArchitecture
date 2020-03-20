using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoListsVmTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var todoLists = new List<TodoListDto> { new TodoListDto() };

            var sut = new TodoListsVm
            {
                TodoLists = todoLists
            };

            sut.TodoLists.Should().BeEquivalentTo(todoLists);
        }
    }
}
