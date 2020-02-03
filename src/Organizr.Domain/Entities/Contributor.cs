using System;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.Entities
{
    public class Contributor: IEntity, IAuditable
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string ContributorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
