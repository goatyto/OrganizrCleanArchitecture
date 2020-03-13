using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ardalis.GuardClauses;

namespace Organizr.Domain.Guards
{
    public static class GuardClauseCustomExtensions
    {
        public static void NullQueryResult<TElement>(this IGuardClause guardClause, TElement result, string parameterName)
        {
            if (result == null)
                throw new ArgumentException($"Required input {parameterName} produced an empty result.", parameterName);
        }

        public static void HavingTimeComponent(this IGuardClause guardClause, DateTime dateTime, string parameterName)
        {
            if(dateTime > dateTime.Date)
                throw new ArgumentException("Required DateTime input contains time component.", parameterName);
        }

        public static void NonUtcDateTime(this IGuardClause guardClause, DateTime dateTime, string parameterName)
        {
            if(dateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Required DateTime input is not of UTC kind.", parameterName);
        }
    }
}
