using System;
using System.Threading;
using System.Threading.Tasks;
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
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(resourceAuthorizationService, nameof(resourceAuthorizationService));
            Assert.Argument.NotNull(userGroupRepository, nameof(userGroupRepository));

            _identityService = identityService;
            _resourceAuthorizationService = resourceAuthorizationService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<Unit> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var userGroup = await _userGroupRepository.GetAsync(request.UserGroupId, cancellationToken);

            if(userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.UserGroupId);

            var userExists = await _identityService.UserExistsAsync(request.UserId);

            if(!userExists)
                throw new InvalidUserIdException(request.UserId);

            userGroup.AddMember(request.UserId);

            _userGroupRepository.Update(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
