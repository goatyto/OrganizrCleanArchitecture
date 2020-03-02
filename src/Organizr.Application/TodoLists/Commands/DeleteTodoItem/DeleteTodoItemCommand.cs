using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
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
        private readonly ITodoListRepository _todoListRepository;

        public DeleteTodoItemCommandHandler(ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.DeleteTodo(request.Id);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
