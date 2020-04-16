using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.DeleteTodoItem
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
        private readonly IIdentityService _identityService;
        private readonly ITodoListRepository _todoListRepository;

        public DeleteTodoItemCommandHandler(IIdentityService identityService, ITodoListRepository todoListRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var todoList = await _todoListRepository.GetOwnAsync(request.TodoListId, currentUserId, cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            todoList.DeleteTodo(request.Id);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
