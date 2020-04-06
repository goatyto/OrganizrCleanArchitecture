using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

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
            Assert.Argument.NotNull(idGenerator, nameof(idGenerator));
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _idGenerator = idGenerator;
            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoListId = _idGenerator.GenerateNext<TodoList>();
            var currentUserId = _identityService.CurrentUserId;

            var todoList = TodoList.Create(todoListId, currentUserId, request.Title, request.Description);

            _todoListRepository.Add(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
