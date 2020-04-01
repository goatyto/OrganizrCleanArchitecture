using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;

namespace Organizr.Application.Planning.TodoLists.Queries.GetTodoLists
{
    public class GetTodoListsCommand: IRequest<TodoListsVm>
    {

    }

    public class GetTodoListsCommandHandler : IRequestHandler<GetTodoListsCommand, TodoListsVm>
    {
        private readonly IIdentityService _currentUserService;
        private readonly ITodoListQueries _todoListQueries;

        public GetTodoListsCommandHandler(IIdentityService currentUserService, ITodoListQueries todoListQueries)
        {
            Guard.Against.Null(currentUserService, nameof(currentUserService));
            Guard.Against.Null(todoListQueries, nameof(todoListQueries));

            _currentUserService = currentUserService;
            _todoListQueries = todoListQueries;
        }


        public async Task<TodoListsVm> Handle(GetTodoListsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUserId;

            var todoListDtos = await _todoListQueries.GetTodoListsForUserAsync(currentUserId, cancellationToken);

            var todoListVm = new TodoListsVm()
            {
                TodoLists = todoListDtos
            };

            return todoListVm;
        }
    }
}
