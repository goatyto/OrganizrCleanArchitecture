using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }
        public int? SubListId { get; }
    }

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IDateTime _dateTimeProvider;

        public AddTodoItemCommandHandler(ITodoListRepository todoListRepository, IDateTime dateTimeProvider)
        {
            _todoListRepository = todoListRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            todoList.AddTodo(request.Title, request.Description, request.DueDate, _dateTimeProvider, request.SubListId);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
