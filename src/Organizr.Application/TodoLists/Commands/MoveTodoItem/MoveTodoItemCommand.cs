using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.MoveTodoItem
{
    public class MoveTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int NewOrdinal { get; }
        public int? SubListId { get; }

        public MoveTodoItemCommand(Guid todoListId, int id, int newOrdinal, int? subListId = null)
        {
            TodoListId = todoListId;
            Id = id;
            NewOrdinal = newOrdinal;
            SubListId = subListId;
        }
    }

    public class MoveTodoItemCommandHandler : IRequestHandler<MoveTodoItemCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public MoveTodoItemCommandHandler(ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(MoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            todoList.MoveTodo(request.Id, new TodoItemPosition(request.NewOrdinal, request.SubListId));

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
