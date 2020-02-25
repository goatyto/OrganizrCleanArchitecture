using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class GetTodoListsCommand: IRequest<TodoListsVm>
    {
        public string UserId { get; }

        public GetTodoListsCommand(string userId)
        {
            UserId = userId;
        }
    }
}
