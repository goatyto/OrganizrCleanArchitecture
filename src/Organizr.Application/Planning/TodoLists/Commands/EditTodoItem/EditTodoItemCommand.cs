using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommand : DueDateUtcCommandBase, IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }

        public EditTodoItemCommand(Guid todoListId, int id, string title, string description = null,
            DateTime? dueDateUtc = null, int? clientTimeZoneOffsetInMinutes = null) : base(dueDateUtc, clientTimeZoneOffsetInMinutes)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
        }
    }

    public class EditTodoItemCommandHandler : IRequestHandler<EditTodoItemCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly ITodoListRepository _todoListRepository;

        public EditTodoItemCommandHandler(IIdentityService identityService, ITodoListRepository todoListRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(EditTodoItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var todoList = await _todoListRepository.GetOwnAsync(request.TodoListId, currentUserId, cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            todoList.EditTodo(request.Id, request.Title, request.Description,
                ClientDateUtc.Create(request.DueDateUtc, request.ClientTimeZoneOffsetInMinutes));

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
