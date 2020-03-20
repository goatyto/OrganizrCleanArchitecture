using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.TodoLists.Queries;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Infrastructure.Persistence.Queries;
using Organizr.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Organizr.Infrastructure.IntegrationTests.Persistence.Queries
{
    public class TodoListQueriesTests : IClassFixture<OrganizrContextFixture>
    {
        private readonly OrganizrContextFixture _fixture;
        private readonly ITodoListQueries _sut;

        public TodoListQueriesTests(OrganizrContextFixture fixture)
        {
            _fixture = fixture;

            var todoListRepository = new TodoListRepository(_fixture.Context);

            var todoList1 = TodoList.Create(Guid.NewGuid(), "User1", "Title1", "Description1");
            todoList1.AddSubList("TodoSubList11");
            todoList1.AddTodo("TodoItem11");
            todoList1.AddTodo("TodoItem12", subListId: 1);
            todoListRepository.Add(todoList1);

            var todoList2 = TodoList.Create(Guid.NewGuid(), "User1", "Title2", "Description2");
            todoList2.AddSubList("TodoSubList21");
            todoList2.AddTodo("TodoItem21");
            todoList2.AddTodo("TodoItem22", subListId: 1);
            todoListRepository.Add(todoList2);

            var todoList3 = TodoList.Create(Guid.NewGuid(), "User1", "Title3", "Description3");
            todoList3.AddSubList("TodoSubList31");
            todoList3.AddTodo("TodoItem31");
            todoList3.AddTodo("TodoItem32", subListId: 1);
            todoListRepository.Add(todoList3);

            todoListRepository.UnitOfWork.SaveChangesAsync().Wait();

            var userGroupRepository = new UserGroupRepository(_fixture.Context);

            var userGroup = UserGroup.Create(Guid.NewGuid(), "User1", "Group1");

            var contextDbConnection = _fixture.Context.Database.GetDbConnection();

            var dbConnectionFactoryMock = new Mock<IDbConnectionFactory>();
            dbConnectionFactoryMock.Setup(m => m.Create(contextDbConnection.ConnectionString))
                .Returns(contextDbConnection);

            _sut = new TodoListQueries(dbConnectionFactoryMock.Object, contextDbConnection.ConnectionString);

            SqlMapper.AddTypeHandler(new GuidHandler());
        }

        [Fact]
        public async Task GetTodoListsForUserAsync_ValidUserId_RetursTodoLists()
        {
            var todoLists = await _sut.GetTodoListsForUserAsync("User1");

            todoLists.Should().HaveCount(3);
        }

        //[Fact]
        //public async Task GetSharedTodoListsForGroupAsync_ValidUserGroupId_RetursTodoLists()
        //{
        //    var todoLists = await _sut.GetSharedTodoListsForGroupAsync("User1");

        //    todoLists.Should().HaveCount(3);
        //}
    }
}
