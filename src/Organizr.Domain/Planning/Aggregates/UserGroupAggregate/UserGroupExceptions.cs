using System;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.UserGroupAggregate
{
    public class UserGroupException : Exception
    {
        public UserGroupException(string message) : base(message)
        {
            
        }
    }
}
