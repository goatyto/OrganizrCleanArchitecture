using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel
{
    public class Contributor: Auditable, IEntity
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string ContributorId { get; set; }
    }
}
