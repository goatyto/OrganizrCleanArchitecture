using System;
using System.Collections.Generic;
using System.Linq;
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

        public static UserGroup Create(Guid id, string creatorUserId, string name,
            IEnumerable<string> memberIds, string description = null)
        {
            Assert.Argument.NotDefault(id, nameof(id), "User group Id cannot be default value.");
            Assert.Argument.NotEmpty(creatorUserId, nameof(creatorUserId), "Creator user Id cannot be empty.");

            AssertNameNotEmpty(name);

            Assert.Argument.NotEmpty(memberIds, nameof(memberIds), "Group member collection cannot be empty.");
            Assert.Argument.DoesNotHaveEmptyElements(memberIds, nameof(memberIds), "Group member collection cannot have empty user Ids.");

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
            AssertNameNotEmpty(name);

            Name = name;
            Description = description;

            AddDomainEvent(new UserGroupEdited(this));
        }

        public void AddMember(string userId)
        {
            Assert.Argument.NotEmpty(userId, nameof(userId), "Member user Id cannot be empty.");

            var newMembership = new UserGroupMember(userId);

            AssertGroupMemberDoesNotExist(userId);

            _membership.Add(newMembership);

            AddDomainEvent(new UserGroupMemberAdded(Id, userId));
        }

        public void RemoveMember(string userId)
        {
            Assert.Argument.NotEmpty(userId, nameof(userId), "Member user Id cannot be empty.");

            AssertGroupMemberExists(userId);
            AssertRemovedGroupMemberNotCreator(userId);

            _membership.Remove((UserGroupMember)userId);

            AddDomainEvent(new UserGroupMemberRemoved(Id, userId));
        }

        public TodoList CreateSharedTodoList(Guid id, string creatorUserId, string title, string description = null)
        {
            AssertGroupMemberExists(creatorUserId);

            var todoList = TodoList.CreateShared(id, creatorUserId, Id, title, description);

            return todoList;
        }

        private static void AssertNameNotEmpty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new UserGroupException("User group name cannot be empty.");
        }

        private void AssertGroupMemberDoesNotExist(string userId)
        {
            if(Members.Any(m=>m == userId))
                throw new InvalidOperationException($"User with Id \"{userId}\" is already a member in group \"{Id}\".");
        }

        private void AssertGroupMemberExists(string userId)
        {
            if (Members.All(m => m != userId))
                throw new InvalidOperationException($"User with Id \"{userId}\" is not a member in group \"{Id}\".");
        }

        private void AssertRemovedGroupMemberNotCreator(string removedMemberId)
        {
            if(removedMemberId == CreatorUserId)
                throw new UserGroupException("Creator user cannot be removed from group.");
        }
    }
}
