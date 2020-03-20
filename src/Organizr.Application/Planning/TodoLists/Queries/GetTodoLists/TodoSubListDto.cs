using System;
using System.Collections.Generic;

namespace Organizr.Application.Planning.TodoLists.Queries.GetTodoLists
{
    public class TodoSubListDto
    {
        public int Id { get; set; }
        public Guid TodoListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<TodoItemDto> Items { get; set; }
    }
}