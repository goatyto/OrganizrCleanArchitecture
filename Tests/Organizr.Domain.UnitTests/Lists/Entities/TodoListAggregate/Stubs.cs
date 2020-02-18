using System;
using System.Collections.Generic;
using System.Text;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Domain.UnitTests.Lists.Entities.TodoListAggregate
{
    public class TodoListStub : TodoList
    {
        public TodoListStub(Guid id, string ownerId, string title, string description = null,
            IEnumerable<TodoSubList> todoSubLists = null, IEnumerable < TodoItem> todoItems = null) : base(ownerId, title,
            description)
        {
            Id = id;

            foreach (var todoItem in todoItems)
            {
                _items.Add(todoItem);
            }

            foreach (var todoSubList in todoSubLists)
            {
                _subLists.Add(todoSubList);
            }
        }
    }

    public class TodoSubListStub : TodoSubList
    {
        public TodoSubListStub(int id, string title, int ordinal, string description = null, bool? isDeleted = null) : base(title, ordinal,
            description)
        {
            Id = id;

            if (isDeleted.HasValue)
                IsDeleted = isDeleted.Value;
        }
    }

    public class TodoItemStub : TodoItem
    {
        public TodoItemStub(int id, Guid mainListId, string title, TodoItemPosition position, string description = null,
            DateTime? dueDate = null, bool? isCompleted = null, bool? isDeleted = null) : base(mainListId, title,
            position, description, dueDate)
        {
            Id = id;

            if (isCompleted.HasValue)
                IsCompleted = isCompleted.Value;

            if (isDeleted.HasValue)
                IsDeleted = isDeleted.Value;
        }
    }
}
