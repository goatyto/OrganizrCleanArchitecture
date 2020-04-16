using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : DueDateUtcCommandBase, IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }
        public int? SubListId { get; }

        public AddTodoItemCommand(Guid todoListId, string title, string description = null, DateTime? dueDateUtc = null,
            int? clientTimeZoneOffsetInMinutes = null, int? subListId = null) : base(dueDateUtc, clientTimeZoneOffsetInMinutes)
        {
            TodoListId = todoListId;
            Title = title;
            Description = description;
            SubListId = subListId;
        }
    }

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly ITodoListRepository _todoListRepository;

        public AddTodoItemCommandHandler(IIdentityService identityService, ITodoListRepository todoListRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var todoList = await _todoListRepository.GetOwnAsync(request.TodoListId, currentUserId, cancellationToken);

            if (todoList == null)
                throw new ResourceNotFoundException<TodoList>(request.TodoListId);

            todoList.AddTodo(request.Title, request.Description, ClientDateUtc.Create(request.DueDateUtc, request.ClientTimeZoneOffsetInMinutes), request.SubListId);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
