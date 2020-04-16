using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Organizr.Application.Planning.Common.Exceptions;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.UserGroups.RemoveMember
{
    public class RemoveMemberCommand : IConcurrentRequest
    {
        public Guid UserGroupId { get; }
        public string MemberUserId { get; }

        public RemoveMemberCommand(Guid userGroupId, string memberUserId)
        {
            UserGroupId = userGroupId;
            MemberUserId = memberUserId;
        }
    }

    public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserGroupRepository _userGroupRepository;

        public RemoveMemberCommandHandler(IIdentityService identityService, IUserGroupRepository userGroupRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(userGroupRepository, nameof(userGroupRepository));

            _identityService = identityService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<Unit> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;

            var userGroup = await _userGroupRepository.GetAsync(request.UserGroupId, currentUserId, cancellationToken);

            if (userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.UserGroupId);

            var userExists = await _identityService.UserExistsAsync(request.MemberUserId);

            if (!userExists)
                throw new InvalidUserIdException(request.MemberUserId);

            userGroup.RemoveMember(request.MemberUserId);

            _userGroupRepository.Update(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
