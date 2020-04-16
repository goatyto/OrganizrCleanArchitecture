using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Organizr.Application.Planning.TodoLists.Commands;
using Organizr.Application.UnitTests.Common;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Validation
{
    public class DueDateUtcCommandValidatorTests : RequestValidationTestBase<DueDateUtcCommandStub>
    {
        protected override object[] ValidatorParams => null;

        public static IEnumerable<object[]> HandleValidMemberData = new List<object[]>
        {
            new object[]{DateTime.UtcNow.Date, 0},
            new object[]{DateTime.UtcNow.Date.AddDays(1).AddMinutes(60), 60},
            new object[]{null, null}
        };

        [Theory, MemberData(nameof(HandleValidMemberData))]
        public void Handle_ValidData_DoesNotThrow(DateTime? dueDateUtc, int? clientTimeZoneOffsetInMinutes)
        {
            var request = new DueDateUtcCommandStub(dueDateUtc, clientTimeZoneOffsetInMinutes);

            Sut.Invoking(c => c.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .NotThrow();
        }

        public static IEnumerable<object[]> HandleInvalidDueDateUtcMemberData = new List<object[]>
        {
            new object[]{DateTime.UtcNow.Date.AddTicks(1), 0},
            new object[]{DateTime.UtcNow.Date.AddDays(-1).AddMinutes(60), 60},
            new object[]{new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Local).AddMinutes(-60), -60}
        };

        [Theory, MemberData(nameof(HandleInvalidDueDateUtcMemberData))]
        public void Handle_InvalidDueDateUtc_ThrowsValidationException(DateTime? invalidDueDateUtc,
            int? clientTimeZoneOffsetInMinutes)
        {
            var request = new DueDateUtcCommandStub(invalidDueDateUtc, clientTimeZoneOffsetInMinutes);

            Sut.Invoking(c => c.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().Contain(failure =>
                    failure.PropertyName == nameof(DueDateUtcCommandStub.DueDateUtc));
        }

        public static IEnumerable<object[]> HandleInvalidClientTimeZoneOffsetInMinutesMemberData = new List<object[]>
        {
            new object[]{DateTime.UtcNow.Date, null},
            new object[]{DateTime.UtcNow.Date.AddMinutes(999), 999},
            new object[]{DateTime.UtcNow.Date.AddMinutes(-999), -999},
            new object[]{DateTime.UtcNow.Date.AddMinutes(123), 123},
        };

        [Theory, MemberData(nameof(HandleInvalidClientTimeZoneOffsetInMinutesMemberData))]
        public void Handle_InvalidClientTimeZoneOffsetInMinutes_ThrowsValidationException(DateTime? dueDateUtc,
            int? invalidClientTimeZoneOffsetInMinutes)
        {
            var request = new DueDateUtcCommandStub(dueDateUtc, invalidClientTimeZoneOffsetInMinutes);

            Sut.Invoking(c => c.Handle(request, CancellationToken.None, RequestHandlerDelegateMock.Object)).Should()
                .Throw<ValidationException>().And.Errors.Should().Contain(failure =>
                    failure.PropertyName == nameof(DueDateUtcCommandStub.ClientTimeZoneOffsetInMinutes));
        }
    }

    public class DueDateUtcCommandStub : DueDateUtcCommandBase, IRequest
    {
        public DueDateUtcCommandStub(DateTime? dueDateUtc, int? clientTimeZoneOffsetInMinutes) : base(dueDateUtc,
            clientTimeZoneOffsetInMinutes)
        {

        }
    }

    public class DueDateUtcCommandHandlerStub : IRequestHandler<DueDateUtcCommandStub>
    {
        public Task<Unit> Handle(DueDateUtcCommandStub request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }

    public class DueDateUtcCommandValidatorStub : DueDateUtcCommandValidatorBase<DueDateUtcCommandStub>
    {

    }
}
