using System;

namespace Organizr.Domain.Interfaces
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime Created { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModified { get; set; }
    }
}