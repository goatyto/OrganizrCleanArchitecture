using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Infrastructure.Persistence;
using Organizr.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Organizr.Infrastructure.IntegrationTests.Persistence.Repositories
{
    public class TodoListRepositoryTests : IClassFixture<OrganizrContextFixture>
    {
        private readonly OrganizrContextFixture _fixture;
        private readonly ITodoListRepository _sut;
        private readonly Guid _todoListId;

        public TodoListRepositoryTests(OrganizrContextFixture fixture)
        {
            _fixture = fixture;

            _sut = new TodoListRepository(_fixture.Context);

            _todoListId = Guid.NewGuid();

            var todoList = TodoList.Create(_todoListId, "User1", "Title", "Description");

            todoList.AddSubList("SubList1");
            todoList.AddSubList("SubList2");

            todoList.AddTodo("Todo1");
            todoList.AddTodo("Todo2");

            todoList.AddTodo("Todo11", subListId: 1);
            todoList.AddTodo("Todo12", subListId: 1);

            _sut.Add(todoList);

            _sut.UnitOfWork.SaveChangesAsync().Wait();
        }

        [Fact]
        public async Task GetAsync_ExistingTodoListId_EntryLoaded()
        {
            var todoList = await _sut.GetAsync(_todoListId);

            todoList.Should().NotBeNull();
        }

        [Fact]
        public async Task Add_ValidData_EntrySaved()
        {
            var todoListId = Guid.NewGuid();

            var todoList = TodoList.Create(todoListId, "User1", "Title", "Description");

            _sut.Add(todoList);

            await _sut.UnitOfWork.SaveChangesAsync();

            var peristedTodoList = await _sut.GetAsync(todoListId);

            peristedTodoList.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_EntityChanges_ChangesPersisted()
        {
            var persistedTodoList = await _sut.GetAsync(_todoListId);

            persistedTodoList.DeleteSubList(2);

            persistedTodoList.DeleteTodo(2);
            persistedTodoList.DeleteTodo(4);

            _sut.Update(persistedTodoList);

            await _sut.UnitOfWork.SaveChangesAsync();

            persistedTodoList = await _sut.GetAsync(_todoListId);

            persistedTodoList.SubLists.Single(sl => sl.Id == 2).IsDeleted.Should().Be(true);

            persistedTodoList.Items.Single(it => it.Id == 2).IsDeleted.Should().Be(true);
            persistedTodoList.Items.Single(it => it.Id == 4).IsDeleted.Should().Be(true);
        }
    }
}
