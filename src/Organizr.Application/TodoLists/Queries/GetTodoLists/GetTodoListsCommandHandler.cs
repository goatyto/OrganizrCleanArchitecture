using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class GetTodoListsCommandHandler : IRequestHandler<GetTodoListsCommand, TodoListsVm>
    {
        private readonly ITodoListQueries _todoListQueries;

        public GetTodoListsCommandHandler(ITodoListQueries todoListQueries)
        {
            _todoListQueries = todoListQueries;
        }


        public async Task<TodoListsVm> Handle(GetTodoListsCommand request, CancellationToken cancellationToken)
        {
            var todoListDtos = await _todoListQueries.GetTodoListsForUserAsync(request.UserId, cancellationToken);

            var todoListVm = new TodoListsVm()
            {
                TodoLists = todoListDtos
            };

            return todoListVm;
        }
    }
}
