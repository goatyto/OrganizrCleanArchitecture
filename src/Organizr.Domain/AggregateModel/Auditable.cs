using System;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel
{
    public class Auditable:IAuditable
    {
        public string CreatedBy { get; private set; }
        public DateTime Created { get; private set; }
        public string LastModifiedBy { get; private set; }
        public DateTime? LastModified { get; private set; }
        
        public void SetCreated(string createdBy, DateTime created)
        {
            CreatedBy = createdBy;
            Created = created;
        }

        public void SetLastModified(string lastModifiedBy, DateTime lastModified)
        {
            LastModifiedBy = lastModifiedBy;
            LastModified = lastModified;
        }
    }
}
