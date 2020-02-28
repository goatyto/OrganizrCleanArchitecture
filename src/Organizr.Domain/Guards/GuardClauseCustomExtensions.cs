using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.GuardClauses;

namespace Organizr.Domain.Guards
{
    public static class GuardClauseCustomExtensions
    {
        /// <summary>Empties the result.</summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="guardClause">The guard clause.</param>
        /// <param name="result">The result.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentException">Required input {parameterName} produced an empty result.</exception>
        public static void NullResult<TElement>(this IGuardClause guardClause, TElement result, string parameterName)
        {
            if (result == null)
                throw new ArgumentException($"Required input {parameterName} produced an empty result.", parameterName);
        }
    }
}
