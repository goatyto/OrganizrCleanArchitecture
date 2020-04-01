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
using Organizr.Domain.Planning;
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
        private readonly Guid UserGroupId;

        public TodoListQueriesTests(OrganizrContextFixture fixture)
        {
            _fixture = fixture;

            var todoListRepository = new TodoListRepository(_fixture.Context);

            var todoList1 = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Title1", "Description1");
            todoList1.AddSubList("TodoSubList11");
            todoList1.AddTodo("TodoItem11");
            todoList1.AddTodo("TodoItem12", subListId: (TodoSubListId)1);
            todoListRepository.Add(todoList1);

            var todoList2 = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Title2", "Description2");
            todoList2.AddSubList("TodoSubList21");
            todoList2.AddTodo("TodoItem21");
            todoList2.AddTodo("TodoItem22", subListId: (TodoSubListId)1);
            todoListRepository.Add(todoList2);

            var todoList3 = TodoList.Create(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "Title3", "Description3");
            todoList3.AddSubList("TodoSubList31");
            todoList3.AddTodo("TodoItem31");
            todoList3.AddTodo("TodoItem32", subListId: (TodoSubListId)1);
            todoListRepository.Add(todoList3);

            todoListRepository.UnitOfWork.SaveChangesAsync().Wait();

            var userGroupRepository = new UserGroupRepository(_fixture.Context);

            UserGroupId = Guid.NewGuid();
            var userGroup = UserGroup.Create((UserGroupId)UserGroupId, new CreatorUser("User1"), "Group1");
            userGroupRepository.Add(userGroup);

            var sharedTodoList1 = userGroup.CreateSharedTodoList(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "SharedTitle1", "SharedDescription1");
            sharedTodoList1.AddSubList("SharedTodoSubList11");
            sharedTodoList1.AddTodo("SharedTodoItem11");
            sharedTodoList1.AddTodo("SharedTodoItem12", subListId: (TodoSubListId)1);
            todoListRepository.Add(sharedTodoList1);

            var sharedTodoList2 = userGroup.CreateSharedTodoList(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "SharedTitle2", "SharedDescription2");
            sharedTodoList2.AddSubList("SharedTodoSubList21");
            sharedTodoList2.AddTodo("SharedTodoItem21");
            sharedTodoList2.AddTodo("SharedTodoItem22", subListId: (TodoSubListId)1);
            todoListRepository.Add(sharedTodoList2);

            var sharedTodoList3 = userGroup.CreateSharedTodoList(new TodoListId(Guid.NewGuid()), new CreatorUser("User1"), "SharedTitle3", "SharedDescription3");
            sharedTodoList3.AddSubList("SharedTodoSubList31");
            sharedTodoList3.AddTodo("SharedTodoItem31");
            sharedTodoList3.AddTodo("SharedTodoItem32", subListId: (TodoSubListId)1);
            todoListRepository.Add(sharedTodoList3);

            userGroupRepository.UnitOfWork.SaveChangesAsync().Wait();

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

        [Fact]
        public async Task GetSharedTodoListsForGroupAsync_ValidUserGroupId_RetursTodoLists()
        {
            var todoLists = await _sut.GetSharedTodoListsForGroupAsync(UserGroupId);

            todoLists.Should().HaveCount(3);
        }
    }
}
