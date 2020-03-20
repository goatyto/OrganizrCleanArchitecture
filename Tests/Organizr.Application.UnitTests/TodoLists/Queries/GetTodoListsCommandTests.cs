using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using Moq;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
using Xunit;

namespace Organizr.Application.UnitTests.TodoLists.Queries
{
    public class GetTodoListsCommandTests : TodoListQueryCommandTestBase
    {
        private readonly GetTodoListsCommandHandler _sut;

        public GetTodoListsCommandTests()
        {
            _sut = new GetTodoListsCommandHandler(TodoListQueriesMock.Object, CurrentUserServiceMock.Object);
        }

        [Fact]
        public void Handle_ValidRequest_DoesNotThrow()
        {
            var request = new GetTodoListsCommand();

            _sut.Invoking(s => s.Handle(request, CancellationToken.None)).Should().NotThrow();
        }
    }
}
