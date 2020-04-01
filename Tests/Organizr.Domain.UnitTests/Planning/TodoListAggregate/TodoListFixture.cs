using System;
using System.Collections.Generic;
using System.Linq;
using Organizr.Domain.Planning;
using Organizr.Domain.Planning.Aggregates.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.UnitTests.Planning.TodoListAggregate
{
    public class TodoListFixture : IDisposable
    {
        public TodoList Sut { get; }
        public TodoListId TodoListId => new TodoListId(Guid.NewGuid());
        private readonly DateTime _clientDateTimeToday;
        private readonly int _clientTimeZoneOffsetInMinutes;

        public TodoListFixture()
        {
            Sut = TodoList.Create(TodoListId, new CreatorUser("User1"), "Todo List 1", "Todo List 1 Description");

            var clientDateTimeNow = DateTime.UtcNow;
            
            _clientDateTimeToday = clientDateTimeNow.Date;
            _clientTimeZoneOffsetInMinutes = 0;

            Sut.AddSubList("Todo Sublist 1", "Todo Sublist 1 Description");
            Sut.AddSubList("Todo Sublist 2", "Todo Sublist 2 Description");
            Sut.AddSubList("Todo Sublist 3", "Todo Sublist 3 Description");
            Sut.AddSubList("Todo Sublist 4", "Todo Sublist 4 Description");
            Sut.AddSubList("Todo Sublist 5", "Todo Sublist 5 Description");

            Sut.AddTodo("Todo Item 1 in Main list", "Todo Item 1 in Main list Description", CreateClientDateUtcWithDaysOffset(1));
            Sut.AddTodo("Todo Item 2 in Main list", "Todo Item 2 in Main list Description", CreateClientDateUtcWithDaysOffset(2));
            Sut.AddTodo("Todo Item 3 in Main list", "Todo Item 3 in Main list Description", CreateClientDateUtcWithDaysOffset(3));
            Sut.AddTodo("Todo Item 4 in Main list", "Todo Item 4 in Main list Description", CreateClientDateUtcWithDaysOffset(4));
            Sut.AddTodo("Todo Item 5 in Main list", "Todo Item 5 in Main list Description", CreateClientDateUtcWithDaysOffset(5));

            Sut.AddTodo("Todo Item 1 in Sublist 1", "Todo Item 1 in Sublist 1 Description", CreateClientDateUtcWithDaysOffset(1), (TodoSubListId)1);
            Sut.AddTodo("Todo Item 2 in Sublist 1", "Todo Item 2 in Sublist 1 Description", CreateClientDateUtcWithDaysOffset(2), (TodoSubListId)1);
            Sut.AddTodo("Todo Item 3 in Sublist 1", "Todo Item 3 in Sublist 1 Description", CreateClientDateUtcWithDaysOffset(3), (TodoSubListId)1);
            Sut.AddTodo("Todo Item 4 in Sublist 1", "Todo Item 4 in Sublist 1 Description", CreateClientDateUtcWithDaysOffset(4), (TodoSubListId)1);
            Sut.AddTodo("Todo Item 5 in Sublist 1", "Todo Item 5 in Sublist 1 Description", CreateClientDateUtcWithDaysOffset(5), (TodoSubListId)1);

            Sut.AddTodo("Todo Item 1 in Sublist 2", "Todo Item 1 in Sublist 2 Description", CreateClientDateUtcWithDaysOffset(1), (TodoSubListId)2);
            Sut.AddTodo("Todo Item 2 in Sublist 2", "Todo Item 2 in Sublist 2 Description", CreateClientDateUtcWithDaysOffset(2), (TodoSubListId)2);
            Sut.AddTodo("Todo Item 3 in Sublist 2", "Todo Item 3 in Sublist 2 Description", CreateClientDateUtcWithDaysOffset(3), (TodoSubListId)2);

            Sut.SetCompletedTodo((TodoItemId)2);
            Sut.SetCompletedTodo((TodoItemId)7);
            Sut.SetCompletedTodo((TodoItemId)12);

            Sut.DeleteTodo((TodoItemId)3);
            Sut.DeleteTodo((TodoItemId)8);
            Sut.DeleteTodo((TodoItemId)13);

            Sut.DeleteSubList((TodoSubListId)2);
        }

        public TodoSubListId GetSubListIdForTodoItem(TodoItemId todoId)
        {
            TodoSubListId subListId = null;

            if (Sut.Items.All(it => it.TodoItemId != todoId))
            {
                foreach (var subList in Sut.SubLists)
                {
                    if (subList.Items.Any(it => it.TodoItemId == todoId))
                    {
                        subListId = subList.TodoSubListId;
                        break;
                    }
                }
            }

            return subListId;
        }

        public IEnumerable<TodoItem> GetTodoItems(TodoSubListId subListId = null)
        {
            return subListId == null ? Sut.Items : Sut.SubLists.Single(sl => sl.TodoSubListId == subListId).Items;
        }

        public TodoItem GetTodoItemById(TodoItemId todoId)
        {
            return Sut.Items.Union(Sut.SubLists.SelectMany(sl => sl.Items)).Single(ti => ti.TodoItemId == todoId);
        }

        public ClientDateUtc CreateClientDateUtcWithDaysOffset(int daysOffset)
        {
            return ClientDateUtc.Create(_clientDateTimeToday.Date.AddDays(daysOffset), _clientTimeZoneOffsetInMinutes);
        }

        public ClientDateUtc CreateClientDateUtcWithTicksOffset(int ticksOffset)
        {
            return ClientDateUtc.Create(_clientDateTimeToday.Date.AddTicks(ticksOffset), _clientTimeZoneOffsetInMinutes);
        }

        public void Dispose()
        {
            
        }
    }
}
