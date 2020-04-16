using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.MoveTodoItem
{
    public class MoveTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int NewOrdinal { get; }
        public int? NewSubListId { get; }

        public MoveTodoItemCommand(Guid todoListId, int id, int newOrdinal, int? newSubListId = null)
        {
            TodoListId = todoListId;
            Id = id;
            NewOrdinal = newOrdinal;
            NewSubListId = newSubListId;
        }
    }

    public class MoveTodoItemCommandHandler : IRequestHandler<MoveTodoItemCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly ITodoListRepository _todoListRepository;

        public MoveTodoItemCommandHandler(IIdentityService identityService, ITodoListRepository todoListRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(MoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var todoList = await _todoListRepository.GetOwnAsync(request.TodoListId, currentUserId, cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            todoList.MoveTodo(request.Id, request.NewOrdinal, request.NewSubListId);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
