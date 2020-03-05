using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommand: IRequest
    {
        public string Title { get; }
        public string Description { get; }

        public CreateTodoListCommand(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateTodoListCommandHandler(ICurrentUserService currentUserService, ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            Guard.Against.Null(currentUserService, nameof(currentUserService));

            _todoListRepository = todoListRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var todoList = new TodoList(currentUserId, request.Title, request.Description);

            _todoListRepository.Add(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
