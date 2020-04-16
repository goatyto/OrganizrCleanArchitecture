using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.TodoLists.Commands;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.UserGroups.CreateSharedTodoList
{
    public class CreateSharedTodoListCommand : IConcurrentRequest
    {
        public Guid UserGroupId { get; }
        public string Title { get; }
        public string Description { get; }

        public CreateSharedTodoListCommand(Guid userGroupId, string title, string description = null)
        {
            UserGroupId = userGroupId;
            Title = title;
            Description = description;
        }
    }

    public class CreateSharedTodoListCommandHandler : IRequestHandler<CreateSharedTodoListCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IIdGenerator _idGenerator;
        private readonly ITodoListRepository _todoListRepository;

        public CreateSharedTodoListCommandHandler(IIdentityService identityService,
            IUserGroupRepository userGroupRepository, IIdGenerator idGenerator, ITodoListRepository todoListRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(userGroupRepository, nameof(userGroupRepository));
            Assert.Argument.NotNull(idGenerator, nameof(idGenerator));
            Assert.Argument.NotNull(todoListRepository, nameof(todoListRepository));

            _identityService = identityService;
            _userGroupRepository = userGroupRepository;
            _idGenerator = idGenerator;
            _todoListRepository = todoListRepository;
        }

        public async Task<Unit> Handle(CreateSharedTodoListCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var userGroup = await _userGroupRepository.GetAsync(request.UserGroupId, currentUserId, cancellationToken);

            if(userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.UserGroupId);

            var todoListId = _idGenerator.GenerateNext<TodoList>();

            var sharedTodoList =
                userGroup.CreateSharedTodoList(todoListId, currentUserId, request.Title, request.Description);

            _todoListRepository.Add(sharedTodoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
