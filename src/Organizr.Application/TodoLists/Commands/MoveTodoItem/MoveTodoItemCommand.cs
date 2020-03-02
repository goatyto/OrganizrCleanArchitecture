using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.MoveTodoItem
{
    public class MoveTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int Ordinal { get; }
        public int? SubListId { get; }

        public MoveTodoItemCommand(Guid todoListId, int id, int ordinal, int? subListId)
        {
            TodoListId = todoListId;
            Id = id;
            Ordinal = ordinal;
            SubListId = subListId;
        }
    }

    public class MoveTodoItemCommandHandler : IRequestHandler<MoveTodoItemCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public MoveTodoItemCommandHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(MoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.MoveTodo(request.Id, new TodoItemPosition(request.Ordinal, request.SubListId));

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
