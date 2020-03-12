using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Common.Interfaces;

namespace Organizr.Application.TodoLists.Queries.GetTodoLists
{
    public class GetTodoListsCommand: IRequest<TodoListsVm>
    {

    }

    public class GetTodoListsCommandHandler : IRequestHandler<GetTodoListsCommand, TodoListsVm>
    {
        private readonly ITodoListQueries _todoListQueries;
        private readonly IIdentityService _currentUserService;

        public GetTodoListsCommandHandler(ITodoListQueries todoListQueries, IIdentityService currentUserService)
        {
            _todoListQueries = todoListQueries;
            _currentUserService = currentUserService;
        }


        public async Task<TodoListsVm> Handle(GetTodoListsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var todoListDtos = await _todoListQueries.GetTodoListsForUserAsync(currentUserId, cancellationToken);

            var todoListVm = new TodoListsVm()
            {
                TodoLists = todoListDtos
            };

            return todoListVm;
        }
    }
}
