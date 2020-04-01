using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubList : Entity<TodoSubListId>
    {
        protected override TodoSubListId Identity => TodoSubListId;
        public TodoSubListId TodoSubListId { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TodoSubListPosition Position { get; private set; }
        public bool IsDeleted { get; private set; }

        private readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        private TodoSubList()
        {
            _items = new List<TodoItem>();
        }

        internal TodoSubList(TodoSubListId identity, string title, TodoSubListPosition position, string description = null) : this()
        {
            TodoSubListId = identity;
            Title = title;
            Position = position;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        internal void SetPosition(TodoSubListPosition position)
        {
            Position = position;
        }

        internal void AddTodo(TodoItem todoItem)
        {
            _items.Add(todoItem);
        }

        internal void RemoveTodo(TodoItem todoItem)
        {
            _items.Remove(todoItem);
        }

        internal void Delete()
        {
            IsDeleted = true;
        }
    }
}
