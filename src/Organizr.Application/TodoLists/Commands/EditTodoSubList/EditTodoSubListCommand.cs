using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.EditTodoSubList
{
    public class EditTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }

        public EditTodoSubListCommand(Guid todoListId, int id, string title, string description)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
        }
    }

    public class EditTodoSubListCommandHandler : IRequestHandler<EditTodoSubListCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public EditTodoSubListCommandHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(EditTodoSubListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.EditSubList(request.Id, request.Title, request.Description);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
