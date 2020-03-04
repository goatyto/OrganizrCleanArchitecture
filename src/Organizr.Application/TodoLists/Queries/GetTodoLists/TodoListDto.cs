using System;
using System.Collections.Generic;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class TodoListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<TodoSubListDto> SubLists { get; set; }
        public IEnumerable<TodoItemDto> Items { get; set; }
    }
}