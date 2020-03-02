using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(DeleteTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.DeleteSubList(request.Id);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}