using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.EditTodoItem
{
    public class EditTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }

        public EditTodoItemCommand(Guid todoListId, int id, string title, string description, DateTime? dueDate)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
        }
    }
}
