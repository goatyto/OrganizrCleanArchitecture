using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Organizr.Application.TodoLists.Queries.GetTodoLists;

namespace Organizr.Application.TodoLists.Queries
{
    public interface ITodoListQueries
    {
        Task<IEnumerable<TodoListDto>> GetTodoListsForUserAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}