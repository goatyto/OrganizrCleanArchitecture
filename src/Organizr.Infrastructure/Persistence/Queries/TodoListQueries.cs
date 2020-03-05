using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Data.Sqlite;
using Organizr.Application.TodoLists.Queries;
using Organizr.Application.TodoLists.Queries.GetTodoLists;

namespace Organizr.Infrastructure.Persistence.Queries
{
    public class TodoListQueries : ITodoListQueries
    {
        private readonly string _connectionString;

        public TodoListQueries(string connectionString)
        {
            Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TodoListDto>> GetTodoListsForUserAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbConnection = new SqliteConnection(_connectionString))
            {
                IEnumerable<dynamic> queryResults = await dbConnection.QueryAsync(@"");

                return new List<TodoListDto>();
            }
        }
    }
}
