using System;

namespace Organizr.Domain.Interfaces
{
    public interface IEntity<TKey> where TKey: IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IEntity : IEntity<int>
    {

    }
}