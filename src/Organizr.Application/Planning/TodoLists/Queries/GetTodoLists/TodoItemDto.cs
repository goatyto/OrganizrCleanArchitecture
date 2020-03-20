using System;

namespace Organizr.Application.Planning.TodoLists.Queries.GetTodoLists
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public Guid TodoListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SubListId { get; set; }
        public int Ordinal { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}