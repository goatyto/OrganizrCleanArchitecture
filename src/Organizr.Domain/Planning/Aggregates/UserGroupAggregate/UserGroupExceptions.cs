using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public abstract class UserGroupExceptionBase : Exception
    {
        public Guid UserGroupId { get; }

        protected UserGroupExceptionBase(Guid userGroupId, string message, Exception innerException = null) : base(message, innerException)
        {
            UserGroupId = userGroupId;
        }
    }

    public class UserAlreadyMemberException : UserGroupExceptionBase
    {
        public string UserId { get; }

        public UserAlreadyMemberException(Guid userGroupId, string userId, Exception innerException = null) : base(
            userGroupId, $"User with user id \"{userId}\" is already a member of group with id \"{userGroupId}\"",
            innerException)
        {
            UserId = userId;
        }
    }

    public class UserNotAMemberException : UserGroupExceptionBase
    {
        public string UserId { get; }

        public UserNotAMemberException(Guid userGroupId, string userId, Exception innerException = null) : base(
            userGroupId, $"User with user id \"{userId}\" is not a member of group with id \"{userGroupId}\"",
            innerException)
        {
            UserId = userId;
        }
    }

    public class CreatorCannotBeRemovedException : UserGroupExceptionBase
    {
        public CreatorCannotBeRemovedException(Guid userGroupId, Exception innerException = null) : base(
            userGroupId, $"Creator user cannot be removed from group", innerException)
        {

        }
    }

    public class SharedResourceAlreadyExistsException<TSharedResource> : UserGroupExceptionBase where TSharedResource : Entity<Guid>, IAggregateRoot
    {
        public Guid SharedResourceId { get; }

        public SharedResourceAlreadyExistsException(Guid userGroupId, Guid sharedResourceId,
            Exception innerException = null) : base(userGroupId,
            $"Resource of type {nameof(TSharedResource)} with id \"{sharedResourceId}\" already exists in group with id \"{userGroupId}\"",
            innerException)
        {
            SharedResourceId = sharedResourceId;
        }
    }

    public class SharedResourceDoesNotExistException<TSharedResource> : UserGroupExceptionBase where TSharedResource : Entity<Guid>, IAggregateRoot
    {
        public Guid SharedResourceId { get; }

        public SharedResourceDoesNotExistException(Guid userGroupId, Guid sharedResourceId,
            Exception innerException = null) : base(userGroupId,
            $"Resource of type {nameof(TSharedResource)} with id \"{sharedResourceId}\" does not exist in group with id \"{userGroupId}\"",
            innerException)
        {
            SharedResourceId = sharedResourceId;
        }
    }
}
