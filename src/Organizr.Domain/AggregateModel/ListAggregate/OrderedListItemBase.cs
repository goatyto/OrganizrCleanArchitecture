using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public abstract class OrderedListItemBase: Auditable, IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int Ordinal { get; set; }

        public OrderedListItemBase(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
