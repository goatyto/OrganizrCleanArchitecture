using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Organizr.Domain.SharedKernel;

namespace Organizr.Domain.Planning.Aggregates.TodoListAggregate
{
    public class TodoSubList : Entity<int>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsDeleted { get; private set; }
        public int Ordinal { get; private set; }

        private readonly List<TodoItem> _items;
        public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

        private TodoSubList()
        {
            _items = new List<TodoItem>();
        }

        internal TodoSubList(int id, string title, int ordinal, string description = null) : this()
        {
            Id = id;
            Title = title;
            Ordinal = ordinal;
            Description = description;
        }

        internal void Edit(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        internal void SetPosition(int ordinal)
        {
            Ordinal = ordinal;
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
