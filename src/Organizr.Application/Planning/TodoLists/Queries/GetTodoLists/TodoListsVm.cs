using System.Collections.Generic;

namespace Organizr.Application.Planning.TodoLists.Queries.GetTodoLists
{
    public class TodoListsVm
    {
        public IEnumerable<TodoListDto> TodoLists { get; set; }
    }
}
