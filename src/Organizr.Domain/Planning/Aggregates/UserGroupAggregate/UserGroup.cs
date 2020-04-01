using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroup : Entity<UserGroupId>, IAggregateRoot
    {
        protected override UserGroupId Identity => UserGroupId;
        public UserGroupId UserGroupId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CreatorUser CreatorUser { get; private set; }

        private readonly List<UserGroupMember> _membership;
        public IReadOnlyCollection<UserGroupMember> Members => _membership.AsReadOnly();

        private UserGroup()
        {
            _membership = new List<UserGroupMember>();
        }

        public static UserGroup Create(UserGroupId id, CreatorUser creatorUser, string name, string description = null,
            IEnumerable<UserGroupMember> groupMembers = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.Null(creatorUser, nameof(creatorUser));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            var userGroup = new UserGroup();

            userGroup.UserGroupId = id;
            userGroup.CreatorUser = creatorUser;
            userGroup.Name = name;
            userGroup.Description = description;

            userGroup._membership.Add((UserGroupMember)creatorUser);

            if (groupMembers != null)
            {
                foreach (var member in groupMembers.Distinct())
                {
                    if (member == (UserGroupMember)creatorUser) continue;

                    userGroup._membership.Add(member);
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

        public void AddMember(UserGroupMember member)
        {
            Guard.Against.Null(member, nameof(member));

            AssertMemberNotInGroup(member);

            _membership.Add(member);

            AddDomainEvent(new UserGroupMemberAdded(UserGroupId, member));
        }

        public void RemoveMember(UserGroupMember member)
        {
            Guard.Against.Null(member, nameof(member));

            AssertMemberExistsInGroup(member);
            AssertMemberToBeRemovedNotCreator(member);

            _membership.Remove(member);

            AddDomainEvent(new UserGroupMemberRemoved(UserGroupId, member));
        }

        public TodoList CreateSharedTodoList(TodoListId id, CreatorUser creatorUser, string title, string description = null)
        {
            AssertMemberExistsInGroup((UserGroupMember)creatorUser);

            var todoList = TodoList.CreateShared(id, creatorUser, Identity, title, description);

            return todoList;
        }

        private bool IsCreator(UserGroupMember member)
        {
            return true;
        }

        private void AssertMemberToBeRemovedNotCreator(UserGroupMember memberToBeRemoved)
        {
            if(memberToBeRemoved == CreatorUser)
                throw new UserGroupException("Creator user cannot be removed.");
        }

        private void AssertMemberExistsInGroup(UserGroupMember member)
        {
            if(!Members.Contains(member))
                throw new UserGroupException($"User \"{member}\" is not a member in group \"{UserGroupId}\".");
        }

        private void AssertMemberNotInGroup(UserGroupMember member)
        {
            if(Members.Contains(member))
                throw new UserGroupException($"User \"{member}\" is already a member in group \"{UserGroupId}\".");
        }
    }
}
