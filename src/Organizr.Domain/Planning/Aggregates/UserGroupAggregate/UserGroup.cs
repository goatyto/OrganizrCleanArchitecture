using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroup : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string CreatorUserId { get; private set; }

        private readonly List<UserGroupMember> _membership;
        public IReadOnlyCollection<UserGroupMember> Members => _membership.AsReadOnly();

        private UserGroup()
        {
            _membership = new List<UserGroupMember>();
        }

        public static UserGroup Create(Guid id, string creatorUserId, string name, string description = null,
            IEnumerable<string> memberIds = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(creatorUserId, nameof(creatorUserId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrEmpty(memberIds, nameof(memberIds));

            var userGroup = new UserGroup();

            userGroup.Id = id;
            userGroup.CreatorUserId = creatorUserId;
            userGroup.Name = name;
            userGroup.Description = description;

            userGroup._membership.Add(new UserGroupMember(creatorUserId));

            if (memberIds != null)
            {
                foreach (var userId in memberIds.Distinct())
                {
                    if (userId == creatorUserId) continue;

                    userGroup._membership.Add(new UserGroupMember(userId));
                }
            }

            userGroup.AddDomainEvent(new UserGroupCreated(userGroup));

            return userGroup;
        }

        public void Edit(string name, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            Name = name;
            Description = description;

            AddDomainEvent(new UserGroupEdited(this));
        }

        public void AddMember(string userId)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            var newMembership = new UserGroupMember(userId);

            if (_membership.Any(m => m == newMembership))
                throw new UserAlreadyMemberException(Id, userId);

            _membership.Add(newMembership);

            AddDomainEvent(new UserGroupMemberAdded(Id, userId));
        }

        public void RemoveMember(string userId)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            if (userId == CreatorUserId)
                throw new CreatorCannotBeRemovedException(Id);

            if (_membership.All(m => m != userId))
                throw new UserNotAMemberException(Id, userId);

            _membership.Remove((UserGroupMember)userId);

            AddDomainEvent(new UserGroupMemberRemoved(Id, userId));
        }

        public TodoList CreateSharedTodoList(Guid id, string creatorUserId, string title, string description = null)
        {
            if (_membership.All(m => m.UserId != creatorUserId))
                throw new UserNotAMemberException(Id, creatorUserId);

            var todoList = TodoList.CreateShared(id, creatorUserId, Id, title, description);

            return todoList;
        }
    }
}
