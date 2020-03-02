using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.AddTodoSubList
{
    public class AddTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }

        public AddTodoSubListCommand(Guid todoListId, string title, string description)
        {
            TodoListId = todoListId;
            Title = title;
            Description = description;
        }
    }

    public class AddTodoSubListCommandHandler : IRequestHandler<AddTodoSubListCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public AddTodoSubListCommandHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(AddTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.AddTodo(request.Title, request.Description);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
