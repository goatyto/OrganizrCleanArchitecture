using System;
using System.Collections.Generic;
using System.Text;

namespace Organizr.Application.Planning.Common.Exceptions
{
    public class InvalidUserIdException : Exception
    {
        public string UserId { get; }

        public InvalidUserIdException(string userId, Exception innerException = null) : base(
            $"Invalid user ID: {userId}", innerException)
        {
            UserId = userId;
        }
    }
}
