using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning
{
    public class CreatorUser : ValueObject
    {
        public string UserId { get; private set; }

        public CreatorUser(string userId)
        {
            UserId = userId;
        }

        public static explicit operator CreatorUser(string userId) => new CreatorUser(userId);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
        }

        public override string ToString()
        {
            return UserId;
        }
    }
}
