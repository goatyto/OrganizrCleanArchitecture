using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IUserGroupRepository _userGroupRepository;

        public EditUserGroupCommandHandler(IIdentityService identityService, IUserGroupRepository userGroupRepository)
        {
            Assert.Argument.NotNull(identityService, nameof(identityService));
            Assert.Argument.NotNull(userGroupRepository, nameof(userGroupRepository));

            _identityService = identityService;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<Unit> Handle(EditUserGroupCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _identityService.CurrentUserId;
            
            var userGroup = await _userGroupRepository.GetAsync(request.Id, currentUserId, cancellationToken);

            if(userGroup == null)
                throw new ResourceNotFoundException<UserGroup>(request.Id);

            userGroup.Edit(request.Name, request.Description);

            _userGroupRepository.Update(userGroup);

            await _userGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
