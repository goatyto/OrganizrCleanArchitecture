using System;
using MediatR;
using Moq;
using Organizr.Application.Common.Behaviors;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.UnitTests.Common
{
    public abstract class RequestValidationTestBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private RequestValidationBehavior<TRequest, TResponse> _sut;
        protected RequestValidationBehavior<TRequest, TResponse> Sut
        {
            get
            {
                if (_sut == null)
                {
                    var validators = RequestValidatorFactory.Create<TRequest, TResponse>(ValidatorParams);
                    _sut = new RequestValidationBehavior<TRequest, TResponse>(validators);
                }

                return _sut;
            }
        }

        protected readonly Mock<RequestHandlerDelegate<TResponse>> RequestHandlerDelegateMock = new Mock<RequestHandlerDelegate<TResponse>>();

        public RequestValidationTestBase()
        {

        }

        protected abstract object[] ValidatorParams { get; }
    }

    public abstract class RequestValidationTestBase<TRequest> : RequestValidationTestBase<TRequest, Unit> where TRequest : IRequest
    {

    }
}
