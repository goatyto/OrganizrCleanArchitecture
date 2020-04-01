using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.UserGroups.AddMember
{
    public class AddMemberCommand : IConcurrentRequest
    {
        public Guid UserGroupId { get; }
        public string UserId { get; }

        public AddMemberCommand(Guid userGroupId, string userId)
        {
            UserGroupId = userGroupId;
            UserId = userId;
        }
    }

    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IResourceAuthorizationService<UserGroup> _resourceAuthorizationService;
        private readonly IUserGroupRepository _userGroupRepository;

        public AddMemberCommandHandler(IIdentityService identityService,
            IResourceAuthorizationService<UserGroup> resourceAuthorizationService,
            IUserGroupRepository userGroupRepository)
        {
            Guard.Against.Null(identityService, nameof(identityService));
            Guard.Against.Null(resourceAuthorizationService, nameof(resourceAuthorizationService));
            Guard.Against.Null(userGroupRepository, nameof(userGroupRepository));

            _identityService = identityService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<Unit> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var userGroupId = new UserGroupId(request.UserGroupId);
            var userGroup = await _userGroupRepository.GetAsync(userGroupId, cancellationToken);

            if (userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.UserGroupId);

            var userExists = await _identityService.UserExistsAsync(request.UserId);

            if(!userExists)
                throw new InvalidUserIdException(request.UserId);

            var member = new UserGroupMember(request.UserId);
            userGroup.AddMember(member);

            _userGroupRepository.Update(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
