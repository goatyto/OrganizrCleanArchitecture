﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Application.Common.Interfaces;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }

        public DeleteTodoItemCommand(Guid todoListId, int id)
        {
            TodoListId = todoListId;
            Id = id;
        }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IResourceAccessService _resourceAccessService;
        private readonly ITodoListRepository _todoListRepository;

        public DeleteTodoItemCommandHandler(ICurrentUserService currentUserService,
            IResourceAccessService resourceAccessService, ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(currentUserService, nameof(currentUserService));
            Guard.Against.Null(resourceAccessService, nameof(resourceAccessService));
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _currentUserService = currentUserService;
            _resourceAccessService = resourceAccessService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            if (!_resourceAccessService.CanAccess(request.TodoListId, _currentUserService.UserId))
                throw new AccessDeniedException(request.TodoListId, _currentUserService.UserId);

            todoList.DeleteTodo(request.Id);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
