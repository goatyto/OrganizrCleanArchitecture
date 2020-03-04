using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }

        public EditTodoItemCommand(Guid todoListId, int id, string title, string description = null, DateTime? dueDate = null)
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
        private readonly IDateTime _dateTimeProvider;

        public EditTodoItemCommandHandler(ITodoListRepository todoListRepository, IDateTime dateTimeProvider)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            
            _todoListRepository = todoListRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(EditTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if (todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            todoList.EditTodo(request.Id, request.Title, request.Description, request.DueDate, _dateTimeProvider);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
