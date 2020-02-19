using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoListFixture : IDisposable
    {
        public TodoList TodoList { get; private set; }

        public readonly Guid TodoListId = Guid.NewGuid();

        public TodoListFixture()
        {
            TodoList = new TodoListStub(TodoListId, "User1", "Todo List 1", "Todo List 1 Description",
                new List<TodoSubList>
                {
                    new TodoSubListStub(1, "Todo Sublist 1", 1, "Todo Sublist 1 Description"),
                    new TodoSubListStub(2, "Todo Sublist 2", 2, "Todo Sublist 2 Description", true),
                    new TodoSubListStub(3, "Todo Sublist 3", 2, "Todo Sublist 3 Description"),
                    new TodoSubListStub(4, "Todo Sublist 4", 3, "Todo Sublist 4 Description"),
                    new TodoSubListStub(5, "Todo Sublist 5", 4, "Todo Sublist 5 Description"),
                },
                new List<TodoItem>
                {
                    // main list
                    new TodoItemStub(1, TodoListId, "Todo Item 1 in Main list", new TodoItemPosition(1, null),
                        "Todo Item 1 in Sublist 1 Description", DateTime.Today.AddDays(1)),
                    new TodoItemStub(2, TodoListId, "Todo Item 2 in Main list", new TodoItemPosition(2, null),
                        "Todo Item 2 in Sublist 1 Description", DateTime.Today.AddDays(2), true),
                    new TodoItemStub(3, TodoListId, "Todo Item 3 in Main list", new TodoItemPosition(3, null),
                        "Todo Item 3 in Sublist 1 Description", DateTime.Today.AddDays(3), isDeleted: true),
                    new TodoItemStub(4, TodoListId, "Todo Item 4 in Main list", new TodoItemPosition(3, null),
                        "Todo Item 4 in Sublist 1 Description", DateTime.Today.AddDays(4)),
                    new TodoItemStub(5, TodoListId, "Todo Item 5 in Main list", new TodoItemPosition(4, null),
                        "Todo Item 5 in Sublist 1 Description", DateTime.Today.AddDays(5)),
             
                    // sublist 1
                    new TodoItemStub(6, TodoListId, "Todo Item 1 in Sublist 1", new TodoItemPosition(1, 1),
                        "Todo Item 1 in Sublist 1 Description", DateTime.Today.AddDays(1)),
                    new TodoItemStub(7, TodoListId, "Todo Item 2 in Sublist 1", new TodoItemPosition(2, 1),
                        "Todo Item 2 in Sublist 1 Description", DateTime.Today.AddDays(2), true),
                    new TodoItemStub(8, TodoListId, "Todo Item 3 in Sublist 1", new TodoItemPosition(3, 1),
                        "Todo Item 3 in Sublist 1 Description", DateTime.Today.AddDays(3), isDeleted: true),
                    new TodoItemStub(9, TodoListId, "Todo Item 4 in Sublist 1", new TodoItemPosition(3, 1),
                        "Todo Item 4 in Sublist 1 Description", DateTime.Today.AddDays(4)),
                    new TodoItemStub(10, TodoListId, "Todo Item 5 in Sublist 1", new TodoItemPosition(4, 1),
                        "Todo Item 5 in Sublist 1 Description", DateTime.Today.AddDays(5)),
                
                    // sublist 2
                    new TodoItemStub(11, TodoListId, "Todo Item 1 in Sublist 2", new TodoItemPosition(1, 2),
                        "Todo Item 1 in Sublist 1 Description", DateTime.Today.AddDays(1)),
                    new TodoItemStub(12, TodoListId, "Todo Item 2 in Sublist 2", new TodoItemPosition(2, 2),
                        "Todo Item 2 in Sublist 1 Description", DateTime.Today.AddDays(2), true),
                    new TodoItemStub(13, TodoListId, "Todo Item 3 in Sublist 2", new TodoItemPosition(3, 2),
                        "Todo Item 3 in Sublist 1 Description", DateTime.Today.AddDays(3), isDeleted: true),
                });
        }

        public void Dispose()
        {
            
        }
    }
}
