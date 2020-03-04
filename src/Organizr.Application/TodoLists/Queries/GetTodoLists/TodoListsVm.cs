using System.Collections.Generic;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class TodoListsVm
    {
        public IEnumerable<TodoListDto> TodoLists { get; set; }
    }
}
