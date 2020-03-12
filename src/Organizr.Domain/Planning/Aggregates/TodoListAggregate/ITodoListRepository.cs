using System;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public interface ITodoListRepository: IRepository<TodoList>
    {
        Task<TodoList> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        void Add(TodoList todoList);
        void Update(TodoList todoList);
    }
}