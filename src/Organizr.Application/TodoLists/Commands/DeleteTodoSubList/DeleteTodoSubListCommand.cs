using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.DeleteTodoSubList
{
    public class DeleteTodoSubListCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }

        public DeleteTodoSubListCommand(Guid todoListId, int id)
        {
            TodoListId = todoListId;
            Id = id;
        }
    }

    public class DeleteTodoSubListCommandHandler : IRequestHandler<DeleteTodoSubListCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public DeleteTodoSubListCommandHandler(ITodoListRepository todoListRepository)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(DeleteTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            todoList.DeleteSubList(request.Id);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}