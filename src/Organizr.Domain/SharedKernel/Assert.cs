using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Organizr.Domain.SharedKernel
{
    public static class Assert
    {
        public static IArgument Argument { get; }
    }

    public interface IArgument { }

    public static class ArgumentAssertions
    {
        public static void NotNull<T>(this IArgument argument, T value, string parameterName, string message = null)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName, message ?? $"Required argument \"{parameterName}\" is null.");
        }

        public static void NotEmpty(this IArgument argument, string value, string parameterName,
            string message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(message ?? $"Required argument \"{parameterName}\" is empty.", parameterName);
        }

        public static void NotEmpty<T>(this IArgument argument, IEnumerable<T> collection, string parameterName,
            string message = null)
        {
            if (collection == null || !collection.Any())
                throw new ArgumentException(message ?? $"Required collection \"{parameterName}\" is empty.", parameterName);
        }

        public static void NotDefault<T>(this IArgument argument, T value, string parameterName, string message = null)
        {
            if (value.Equals(default(T)))
                throw new ArgumentException(message ?? $"Required argument \"{parameterName}\" has default value.", parameterName);
        }

        public static void DoesNotHaveEmptyElements<T>(this IArgument argument, IEnumerable<T> collection, string parameterName,
            string message = null)
        {
            if (collection.Any(elem => elem == null))
                throw new ArgumentException(message ?? $"Required argument \"{parameterName}\" has empty elements.", parameterName);
        }
    }
}
