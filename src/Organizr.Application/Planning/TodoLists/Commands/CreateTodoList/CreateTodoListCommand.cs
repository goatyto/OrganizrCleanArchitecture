using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;

namespace Organizr.Application.Planning.TodoLists.Commands.CreateTodoList
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
            var todoListId = new TodoListId(_idGenerator.GenerateNext<TodoList>());
            var creatorUser = new CreatorUser(_identityService.CurrentUserId);

            var todoList = TodoList.Create(todoListId, creatorUser, request.Title, request.Description);

            _todoListRepository.Add(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
