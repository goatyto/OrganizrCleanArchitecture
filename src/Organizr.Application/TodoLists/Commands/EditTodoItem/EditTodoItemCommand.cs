using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Application.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }

        public EditTodoItemCommand(Guid todoListId, int id, string title, string description, DateTime? dueDate)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
        }
    }

    public class EditTodoItemCommandHandler : IRequestHandler<EditTodoItemCommand>
    {
        private readonly ITodoListRepository _todoListRepository;

        public EditTodoItemCommandHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(EditTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.EditTodo(request.Id, request.Title, request.Description, request.DueDate);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
