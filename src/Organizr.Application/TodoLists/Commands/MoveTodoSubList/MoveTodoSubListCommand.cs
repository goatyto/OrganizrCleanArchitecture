using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.MoveTodoSubList
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
        private readonly ITodoListRepository _todoListRepository;

        public MoveTodoSubListCommandHandler(ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));

            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(MoveTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            todoList.MoveSubList(request.Id, request.NewOrdinal);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
