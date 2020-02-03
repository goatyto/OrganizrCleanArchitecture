using System;
using System.Collections.Generic;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.Entities
{
    public abstract class ListGroupBase : IEntity, IAuditable, IOwned, IContributable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        public ICollection<Contributor> Contributors { get; }

        public ListGroupBase()
        {
            Contributors = new HashSet<Contributor>();
        }
    }
}
