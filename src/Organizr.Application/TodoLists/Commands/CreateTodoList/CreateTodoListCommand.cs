using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;

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
        private readonly IIdGenerator _idGenerator;
        private readonly IIdentityService _identityService;
        private readonly ITodoListRepository _todoListRepository;

        public CreateTodoListCommandHandler(IIdGenerator idGenerator, IIdentityService identityService, ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(idGenerator, nameof(idGenerator));
            Guard.Against.Null(identityService, nameof(identityService));
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _idGenerator = idGenerator;
            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoListId = _idGenerator.GenerateNext<TodoList>();
            var currentUserId = _identityService.UserId;

            var todoList = new TodoList(todoListId, currentUserId, request.Title, request.Description);

            _todoListRepository.Add(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
