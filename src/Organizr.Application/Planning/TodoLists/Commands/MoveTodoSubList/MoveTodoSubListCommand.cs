﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.MoveTodoSubList
{
    public class MoveTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int NewOrdinal { get; }

        public MoveTodoSubListCommand(Guid todoListId, int id, int newOrdinal)
        {
            TodoListId = todoListId;
            Id = id;
            NewOrdinal = newOrdinal;
        }
    }

    public class MoveTodoSubListCommandHandler : IRequestHandler<MoveTodoSubListCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IResourceAuthorizationService<TodoList> _resourceAuthorizationService;
        private readonly ITodoListRepository _todoListRepository;

        public MoveTodoSubListCommandHandler(IIdentityService identityService,
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

        public async Task<Unit> Handle(MoveTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoListId = new TodoListId(request.TodoListId);
            var todoList = await _todoListRepository.GetAsync(todoListId, cancellationToken: cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            var currentUserId = _identityService.CurrentUserId;

            if (!_resourceAuthorizationService.CanModify(currentUserId, todoList))
                throw new AccessDeniedException<TodoList>(request.TodoListId, currentUserId);

            var subListId = TodoSubListId.Create(request.Id);
            var subListPosition = new TodoSubListPosition(request.NewOrdinal);

            todoList.MoveSubList(subListId, subListPosition);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
