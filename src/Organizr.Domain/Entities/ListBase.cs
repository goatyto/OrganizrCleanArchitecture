using System;
using System.Collections.Generic;
using Organizr.Domain.Entities.TodoListAggregate;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.Entities
{
    public abstract class ListBase<TItem, TGroup> : IEntity<Guid>, IAuditable, IContributable, IOwned
        where TItem : OrderedListItemBase
        where TGroup : ListGroupBase
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        public TGroup Group { get; set; }

        public ICollection<TItem> Items { get; }

        public ICollection<Contributor> Contributors { get; }

        public ListBase()
        {
            Id = Guid.NewGuid();
            Items = new HashSet<TItem>();
            Contributors = new HashSet<Contributor>();
        }
    }
}
