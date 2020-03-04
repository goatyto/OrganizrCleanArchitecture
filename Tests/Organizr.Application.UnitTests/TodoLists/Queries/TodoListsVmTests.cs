using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Organizr.Application.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class TodoListsVmTests
    {
        [Fact]
        public void Setters_Values_GettersReturnSameValues()
        {
            var todoLists = new List<TodoListDto> { new TodoListDto() };

            var todoListsVm = new TodoListsVm
            {
                TodoLists = todoLists
            };

            todoListsVm.TodoLists.Should().BeEquivalentTo(todoLists);
        }
    }
}
