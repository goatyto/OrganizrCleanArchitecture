using System;

namespace Organizr.Domain.SharedKernel
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}