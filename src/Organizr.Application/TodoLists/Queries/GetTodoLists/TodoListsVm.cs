using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class TodoListsVm
    {
        public IEnumerable<TodoListDto> TodoLists { get; set; }
    }
}
