using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Organizr.Application.UnitTests.Common
{
    public static class RequestValidatorFactory
    {
        public static IEnumerable<IValidator<TRequest>> Create<TRequest, TResponse>([CanBeNull] params object[] args) where TRequest : IRequest<TResponse>
        {
            return Assembly.GetAssembly(typeof(TRequest)).GetTypes()
                .Where(type => type.IsClass && typeof(IValidator<TRequest>).IsAssignableFrom(type))
                .Select(type => (IValidator<TRequest>) Activator.CreateInstance(type, args));
        }

        public static IEnumerable<IValidator<TRequest>> Create<TRequest>([CanBeNull] params object[] args)
            where TRequest : IRequest
        {
            return Create<TRequest, Unit>(args);
        }
    }
}
