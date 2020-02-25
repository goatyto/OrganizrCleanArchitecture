using System;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }
        public bool IsComplated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}