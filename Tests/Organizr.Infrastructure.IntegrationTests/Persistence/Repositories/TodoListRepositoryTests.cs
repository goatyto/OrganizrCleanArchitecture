using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.Planning.Aggregates.UserGroupAggregate;
using Organizr.Infrastructure.Persistence;
using Organizr.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Organizr.Infrastructure.IntegrationTests.Persistence.Repositories
{
    public class TodoListRepositoryTests : IDisposable
    {
        private readonly OrganizrContext _context;
        private readonly ITodoListRepository _sut;
        private readonly Guid _todoListId;
        private readonly string _creatorUserId;
        private readonly Guid _sharedTodoListId;
        private readonly string _memberUserId;

        public TodoListRepositoryTests()
        {
            _context = OrganizrContextFactory.Create();

            _sut = new TodoListRepository(_context);

            _todoListId = Guid.NewGuid();
            _creatorUserId = "User1";

            var todoList = TodoList.Create(_todoListId, _creatorUserId, "Title", "Description");

            todoList.AddSubList("SubList1");
            todoList.AddSubList("SubList2");

            todoList.AddTodo("Todo1");
            todoList.AddTodo("Todo2");

            todoList.AddTodo("Todo11", subListId: 1);
            todoList.AddTodo("Todo12", subListId: 1);

            _sut.Add(todoList);

            _sut.UnitOfWork.SaveChangesAsync().Wait();

            var userGroupRepository = new UserGroupRepository(_context);

            _memberUserId = "User2";

            var userGroup = UserGroup.Create(Guid.NewGuid(), _creatorUserId, "UserGroup", "Description",
                new List<string> { _memberUserId });

            userGroupRepository.Add(userGroup);

            userGroupRepository.UnitOfWork.SaveChangesAsync().Wait();

            _sharedTodoListId = Guid.NewGuid();

            var sharedTodoList = userGroup.CreateSharedTodoList(_sharedTodoListId, _creatorUserId, "SharedTitle", "SharedDescription");

            sharedTodoList.AddSubList("SharedTodoSubList", "SharedTodoDescription");

            sharedTodoList.AddTodo("SharedTodoItem1", "SharedTodoItemDescription1");
            sharedTodoList.AddTodo("SharedTodoItem2", "SharedTodoDescription2", subListId: 1);

            _sut.Add(sharedTodoList);

            _sut.UnitOfWork.SaveChangesAsync().Wait();
        }

        [Fact]
        public async Task GetOwnAsync_ExistingTodoListId_EntryLoaded()
        {
            var todoList = await _sut.GetOwnAsync(_todoListId, _creatorUserId);

            todoList.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSharedAsync_ExistingTodoListId_EntryLoaded()
        {
            var sharedTodoList = await _sut.GetSharedAsync(_sharedTodoListId, _memberUserId);

            sharedTodoList.Should().NotBeNull();
        }

        [Fact]
        public async Task Add_ValidData_EntrySaved()
        {
            var todoListId = Guid.NewGuid();

            var todoList = TodoList.Create(todoListId, _creatorUserId, "Title", "Description");

            _sut.Add(todoList);

            await _sut.UnitOfWork.SaveChangesAsync();

            var peristedTodoList = await _sut.GetOwnAsync(todoListId, _creatorUserId);

            peristedTodoList.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_EntityChanges_ChangesPersisted()
        {
            var persistedTodoList = await _sut.GetOwnAsync(_todoListId, _creatorUserId);

            persistedTodoList.DeleteSubList(2);

            persistedTodoList.DeleteTodo(2);
            persistedTodoList.DeleteTodo(4);

            _sut.Update(persistedTodoList);

            await _sut.UnitOfWork.SaveChangesAsync();

            persistedTodoList = await _sut.GetOwnAsync(_todoListId, _creatorUserId);

            persistedTodoList.SubLists.Single(sl => sl.Id == 2).IsDeleted.Should().Be(true);

            var allTodoItems = persistedTodoList.Items.Union(persistedTodoList.SubLists.SelectMany(sl => sl.Items)).ToList();

            allTodoItems.Single(it => it.Id == 2).IsDeleted.Should().Be(true);
            allTodoItems.Single(it => it.Id == 4).IsDeleted.Should().Be(true);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
