using System.Collections.Generic;
using Organizr.Domain.AggregateModel;

namespace Organizr.Domain.Interfaces
{
    public interface IContributable
    {
        IReadOnlyCollection<Contributor> Contributors { get; }
    }
}