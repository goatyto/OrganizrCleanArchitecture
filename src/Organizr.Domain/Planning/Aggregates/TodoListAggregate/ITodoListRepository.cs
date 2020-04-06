using System;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public interface ITodoListRepository: IRepository<TodoList>
    {
        Task<TodoList> GetOwnAsync(Guid id, string creatorUserId, CancellationToken cancellationToken = default(CancellationToken));
        Task<TodoList> GetSharedAsync(Guid id, string memberUserId, CancellationToken cancellationToken = default(CancellationToken));
        void Add(TodoList todoList);
        void Update(TodoList todoList);
    }
}