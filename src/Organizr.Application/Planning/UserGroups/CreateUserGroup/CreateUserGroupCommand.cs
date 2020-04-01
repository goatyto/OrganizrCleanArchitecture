using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;

namespace Organizr.Application.Planning.UserGroups.CreateUserGroup
{
    public class CreateUserGroupCommand : IRequest
    {
        public string Name { get; }
        public string Description { get; }
        public IEnumerable<string> GroupMemberIds { get; }

        public CreateUserGroupCommand(string name, string description, IEnumerable<string> groupMemberIds)
        {
            Name = name;
            Description = description;
            GroupMemberIds = groupMemberIds;
        }
    }

    public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand>
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IIdentityService _identityService;
        private readonly IUserGroupRepository _userGroupRepository;

        public CreateUserGroupCommandHandler(IIdGenerator idGenerator, IIdentityService identityService, IUserGroupRepository userGroupRepository)
        {
            Guard.Against.Null(idGenerator, nameof(idGenerator));
            Guard.Against.Null(identityService, nameof(identityService));
            Guard.Against.Null(userGroupRepository, nameof(userGroupRepository));

            _idGenerator = idGenerator;
            _identityService = identityService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<Unit> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroupId = _idGenerator.GenerateNext<UserGroup>();
            var currentUserId = _identityService.CurrentUserId;

            foreach (var userId in request.GroupMemberIds)
            {
                var userExists = await _identityService.UserExistsAsync(userId);

                if(!userExists)
                    throw new InvalidUserIdException(userId);
            }

            var userGroup = UserGroup.Create(userGroupId, currentUserId, request.Name, request.Description,
                request.GroupMemberIds);

            _userGroupRepository.Add(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
