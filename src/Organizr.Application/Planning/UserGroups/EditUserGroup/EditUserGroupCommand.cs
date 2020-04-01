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
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.Planning.UserGroups.EditUserGroup
{
    public class EditUserGroupCommand : IRequest
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }

        public EditUserGroupCommand(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }

    public class EditUserGroupCommandHandler : IRequestHandler<EditUserGroupCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IResourceAuthorizationService<UserGroup> _resourceAuthorizationService;
        private readonly IUserGroupRepository _userGroupRepository;

        public EditUserGroupCommandHandler(IIdentityService identityService,
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

        public async Task<Unit> Handle(EditUserGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroupId = new UserGroupId(request.Id);
            var userGroup = await _userGroupRepository.GetAsync(userGroupId, cancellationToken);

            if(userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.Id);

            var currentUserId = _identityService.CurrentUserId;

            if(!_resourceAuthorizationService.CanModify(currentUserId, userGroup))
                throw new AccessDeniedException<UserGroup>(request.Id, currentUserId);

            userGroup.Edit(request.Name, request.Description);

            _userGroupRepository.Update(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
