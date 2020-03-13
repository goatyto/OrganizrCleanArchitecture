using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Services;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDateUtc { get; }
        public int? ClientTimeZoneOffsetInMinutes { get; }
        public int? SubListId { get; }

        public AddTodoItemCommand(Guid todoListId, string title, string description = null, DateTime? dueDateUtc = null,
            int? clientTimeZoneOffsetInMinutes = null, int? subListId = null)
        {
            TodoListId = todoListId;
            Title = title;
            Description = description;
            DueDateUtc = dueDateUtc;
            ClientTimeZoneOffsetInMinutes = clientTimeZoneOffsetInMinutes;
            SubListId = subListId;
        }
    }

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IResourceAuthorizationService<TodoList> _resourceAuthorizationService;
        private readonly ClientDateValidator _clientDateValidator;
        private readonly ITodoListRepository _todoListRepository;

        public AddTodoItemCommandHandler(IIdentityService identityService,
            IResourceAuthorizationService<TodoList> resourceAuthorizationService,
            ClientDateValidator clientDateValidator,
            ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(identityService, nameof(identityService));
            Guard.Against.Null(resourceAuthorizationService, nameof(resourceAuthorizationService));
            Guard.Against.Null(clientDateValidator, nameof(clientDateValidator));
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _clientDateValidator = clientDateValidator;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetAsync(request.TodoListId, cancellationToken: cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            var currentUserId = _identityService.UserId;

            if (!_resourceAuthorizationService.CanModify(currentUserId, todoList))
                throw new AccessDeniedException<TodoList>(request.TodoListId, currentUserId);

            todoList.AddTodo(request.Title, request.Description, request.DueDateUtc, request.ClientTimeZoneOffsetInMinutes,
                _clientDateValidator, request.SubListId);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
