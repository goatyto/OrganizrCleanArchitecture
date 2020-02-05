using System;

namespace Organizr.Domain.Interfaces
{
    public interface IAuditable
    {
        string CreatedBy { get; }
        DateTime Created { get; }
        string LastModifiedBy { get; }
        DateTime? LastModified { get; }

        void SetCreated(string createdBy, DateTime created);
        void SetLastModified(string lastModifiedBy, DateTime lastModified);
    }
}