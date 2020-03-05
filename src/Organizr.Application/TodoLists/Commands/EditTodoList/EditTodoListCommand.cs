using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.EditTodoList
{
    public class EditTodoListCommand: IRequest
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }

        public EditTodoListCommand(Guid id, string title, string description = null)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }

    public class EditTodoListCommandHandler : IRequestHandler<EditTodoListCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IResourceAccessService _resourceAccessService;
        private readonly ITodoListRepository _todoListRepository;

        public EditTodoListCommandHandler(ICurrentUserService currentUserService,
            IResourceAccessService resourceAccessService, ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(currentUserService, nameof(currentUserService));
            Guard.Against.Null(resourceAccessService, nameof(resourceAccessService));
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _currentUserService = currentUserService;
            _resourceAccessService = resourceAccessService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(EditTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.Id, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.Id);

            if (!_resourceAccessService.CanAccess(request.Id, _currentUserService.UserId))
                throw new AccessDeniedException(request.Id, _currentUserService.UserId);

            todoList.Edit(request.Title, request.Description);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
