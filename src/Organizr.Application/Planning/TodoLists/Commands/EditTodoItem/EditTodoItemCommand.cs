using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDateUtc { get; }
        public int? ClientTimeZoneOffsetInMinutes { get; }

        public EditTodoItemCommand(Guid todoListId, int id, string title, string description = null,
            DateTime? dueDateUtc = null, int? clientTimeZoneOffsetInMinutes = null)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
            DueDateUtc = dueDateUtc;
            ClientTimeZoneOffsetInMinutes = clientTimeZoneOffsetInMinutes;
        }
    }

    public class EditTodoItemCommandHandler : IRequestHandler<EditTodoItemCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IResourceAuthorizationService<TodoList> _resourceAuthorizationService;
        private readonly ITodoListRepository _todoListRepository;

        public EditTodoItemCommandHandler(IIdentityService identityService,
            IResourceAuthorizationService<TodoList> resourceAuthorizationService,
            ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(identityService, nameof(identityService));
            Guard.Against.Null(resourceAuthorizationService, nameof(resourceAuthorizationService));
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(EditTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoListId = new TodoListId(request.TodoListId);
            var todoList = await _todoListRepository.GetAsync(todoListId, cancellationToken: cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            var currentUserId = _identityService.CurrentUserId;

            if (!_resourceAuthorizationService.CanModify(currentUserId, todoList))
                throw new AccessDeniedException<TodoList>(request.TodoListId, currentUserId);

            var todoId = new TodoItemId(request.Id);
            todoList.EditTodo(todoId, request.Title, request.Description,
                ClientDateUtc.Create(request.DueDateUtc, request.ClientTimeZoneOffsetInMinutes));

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
