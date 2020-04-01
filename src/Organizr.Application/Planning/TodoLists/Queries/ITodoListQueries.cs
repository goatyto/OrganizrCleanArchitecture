using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;

namespace Organizr.Application.Planning.TodoLists.Queries
{
    public interface ITodoListQueries
    {
        Task<IEnumerable<TodoListDto>> GetTodoListsForUserAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TodoListDto>> GetSharedTodoListsForGroupAsync(Guid userGroupId,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}