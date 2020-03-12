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
        public string CreatorUserId { get; }

        private readonly List<UserGroupMembership> _membership;
        public IReadOnlyCollection<UserGroupMembership> Membership => _membership.AsReadOnly();

        private readonly List<SharedTodoList> _sharedTodoLists;
        public IReadOnlyCollection<SharedTodoList> SharedTodoLists => _sharedTodoLists.AsReadOnly();

        public UserGroup(Guid id, string creatorUserId, string name, string description = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(creatorUserId, nameof(creatorUserId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            Id = id;
            CreatorUserId = creatorUserId;
            Name = name;
            Description = description;

            _membership = new List<UserGroupMembership>
            {
                new UserGroupMembership(id, creatorUserId)
            };

            _sharedTodoLists = new List<SharedTodoList>();
        }

        public void Edit(string name, string description = null)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));

            Name = name;
            Description = description;
        }

        public void AddMember(string userId)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

            var newMembership = new UserGroupMembership(Id, userId);

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

            var membershipToRemove = new UserGroupMembership(Id, userId);

            if (_membership.All(m => m != membershipToRemove))
                throw new UserNotAMemberException(Id, userId);

            _membership.Remove(membershipToRemove);

            AddDomainEvent(new UserGroupMemberRemoved(Id, userId));
        }

        public TodoList CreateSharedTodoList(Guid id, string creatorUserId, string title, string description = null)
        {
            Guard.Against.Default(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(creatorUserId, nameof(creatorUserId));
            Guard.Against.NullOrWhiteSpace(title, nameof(title));

            if (_membership.All(m => m.UserId != creatorUserId))
                throw new UserNotAMemberException(Id, creatorUserId);
            
            var newSharedTodoList = new SharedTodoList(Id, id);

            if (_sharedTodoLists.Any(tl => newSharedTodoList == tl))
                throw new SharedResourceAlreadyExistsException<TodoList>(Id, id);

            _sharedTodoLists.Add(newSharedTodoList);

            AddDomainEvent(new SharedTodoListCreated(Id, id));

            return new TodoList(id, creatorUserId, title, description);
        }

        public void DeleteSharedTodoList(Guid id)
        {
            Guard.Against.Default(id, nameof(id));

            var sharedTodoListToBeRemoved = _sharedTodoLists.SingleOrDefault(tl => tl.TodoListId == id);

            if (sharedTodoListToBeRemoved == null)
                throw new SharedResourceDoesNotExistException<TodoList>(Id, id);

            sharedTodoListToBeRemoved.Delete();

            AddDomainEvent(new SharedTodoListDeleted(Id, id));
        }
    }
}
