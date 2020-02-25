using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public CreateTodoListCommandHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = new TodoList(request.OwnerId, request.Title, request.Description);

            _todoListRepository.Add(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}