using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Organizr.Application.Planning.Common.Interfaces;
using Organizr.Application.Planning.TodoLists.Queries;
using Organizr.Application.Planning.TodoLists.Queries.GetTodoLists;
using Organizr.Domain.SharedKernel;

namespace Organizr.Infrastructure.Persistence.Queries
{
    public class TodoListQueries : ITodoListQueries
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly string _connectionString;

        public TodoListQueries(IDbConnectionFactory dbConnectionFactory, string connectionString)
        {
            Assert.Argument.NotNull(dbConnectionFactory, nameof(dbConnectionFactory));
            Assert.Argument.NotEmpty(connectionString, nameof(connectionString));

            _dbConnectionFactory = dbConnectionFactory;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TodoListDto>> GetTodoListsForUserAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var todoListsQuery =
                @"SELECT    [tl].[Id],
                            [tl].[CreatorUserId],
                            [tl].[UserGroupId],
                            [tl].[Title],
                            [tl].[Description]
                    FROM [TodoLists] [tl]
                    WHERE [tl].[CreatorUserId] = @creatorUserId
                    AND [tl].[UserGroupId] IS NULL";

            var todoSubListsQuery =
                @"SELECT    [sl].[Id],
                            [sl].[Title],
                            [sl].[Description],
                            [sl].[Ordinal],
                            [sl].[IsDeleted],
                            [sl].[ParentListId]
                    FROM [TodoLists] as [tl]
                    INNER JOIN [TodoSubLists] [sl] ON [tl].[Id] = [sl].[ParentListId]
                    WHERE [tl].[CreatorUserId] = @creatorUserId
                    AND [tl].[UserGroupId] IS NULL";

            var todoItemsQuery =
                @"SELECT    [ti].[Id],
                            [ti].[Title],
                            [ti].[Description],
                            [ti].[IsCompleted],
                            [ti].[IsDeleted],
                            [ti].[DueDateUtc] as DueDate,
                            [ti].[Ordinal],
                            [ti].[ParentListId],
                            [ti].[ParentSubListId]
                    FROM [TodoLists] [tl]
                    INNER JOIN [TodoSubLists] [sl] ON [tl].[Id] = [sl].[ParentListId]
                    INNER JOIN [TodoItems] [ti] ON [tl].[Id] = [ti].[ParentListId] OR [sl].[Id] = [ti].[ParentSubListId]
                    WHERE [tl].[CreatorUserId] = @creatorUserId
                    AND [tl].[UserGroupId] IS NULL";

            using (var dbConnection = _dbConnectionFactory.Create(_connectionString))
            {
                var sql = $@"{todoListsQuery};                     
                        {todoSubListsQuery};
                        {todoItemsQuery}";

                using (var multiple = await dbConnection.QueryMultipleAsync(
                    sql,
                    new { creatorUserId = userId }))
                {
                    var todoLists = multiple.Read<TodoListDto>().ToList();
                    var todoSubLists = multiple.Read<TodoSubListDto>().ToList();
                    var todoItems = multiple.Read<TodoItemDto>().ToList();

                    foreach (var list in todoLists)
                    {
                        list.SubLists = todoSubLists.Where(sl => sl.TodoListId == list.Id).ToList();

                        foreach (var subList in todoSubLists)
                        {
                            subList.Items = todoItems
                                .Where(ti => ti.TodoListId == list.Id && ti.SubListId == subList.Id).ToList();
                        }

                        list.Items = todoItems.Where(ti => ti.TodoListId == list.Id && ti.SubListId == null).ToList();
                    }

                    return todoLists;
                }
            }
        }

        public async Task<IEnumerable<TodoListDto>> GetSharedTodoListsForGroupAsync(Guid userGroupId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var todoListsQuery =
                @"SELECT    [tl].[Id],
                            [tl].[CreatorUserId],
                            [tl].[UserGroupId],
                            [tl].[Title],
                            [tl].[Description]
                    FROM [TodoLists] [tl]
                    WHERE [tl].[UserGroupId] = @userGroupId";

            var todoSubListsQuery =
                @"SELECT    [sl].[Id],
                            [sl].[Title],
                            [sl].[Description],
                            [sl].[Ordinal],
                            [sl].[IsDeleted],
                            [sl].[ParentListId]
                    FROM [TodoLists] as [tl]
                    INNER JOIN [TodoSubLists] [sl] ON [tl].[Id] = [sl].[ParentListId]
                    WHERE [tl].[UserGroupId] = @userGroupId";

            var todoItemsQuery =
                @"SELECT    [ti].[Id],
                            [ti].[Title],
                            [ti].[Description],
                            [ti].[IsCompleted],
                            [ti].[IsDeleted],
                            [ti].[DueDateUtc] as DueDate,
                            [ti].[Ordinal],
                            [ti].[ParentListId],
                            [ti].[ParentSubListId]
                    FROM [TodoLists] [tl]
                    INNER JOIN [TodoSubLists] [sl] ON [tl].[Id] = [sl].[ParentListId]
                    INNER JOIN [TodoItems] [ti] ON [tl].[Id] = [ti].[ParentListId] OR [sl].[Id] = [ti].[ParentSubListId]
                    WHERE [tl].[UserGroupId] = @userGroupId";

            using (var dbConnection = _dbConnectionFactory.Create(_connectionString))
            {
                var sql = $@"{todoListsQuery};                     
                        {todoSubListsQuery};
                        {todoItemsQuery}";

                using (var multiple = await dbConnection.QueryMultipleAsync(
                    sql,
                    new { userGroupId }))
                {
                    var todoLists = multiple.Read<TodoListDto>().ToList();
                    var todoSubLists = multiple.Read<TodoSubListDto>().ToList();
                    var todoItems = multiple.Read<TodoItemDto>().ToList();

                    foreach (var list in todoLists)
                    {
                        list.SubLists = todoSubLists.Where(sl => sl.TodoListId == list.Id).ToList();

                        foreach (var subList in todoSubLists)
                        {
                            subList.Items = todoItems
                                .Where(ti => ti.TodoListId == list.Id && ti.SubListId == subList.Id).ToList();
                        }

                        list.Items = todoItems.Where(ti => ti.TodoListId == list.Id && ti.SubListId == null).ToList();
                    }

                    return todoLists;
                }
            }
        }
    }
}
