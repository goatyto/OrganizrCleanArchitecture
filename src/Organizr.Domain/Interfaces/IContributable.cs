using System.Collections.Generic;
using Organizr.Domain.Entities;

namespace Organizr.Domain.Interfaces
{
    public interface IContributable
    {
        ICollection<Contributor> Contributors { get; }
    }
}