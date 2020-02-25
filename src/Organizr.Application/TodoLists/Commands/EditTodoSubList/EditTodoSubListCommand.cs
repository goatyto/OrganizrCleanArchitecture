using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.EditTodoSubList
{
    public class EditTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }

        public EditTodoSubListCommand(Guid todoListId, int id, string title, string description)
        {
            TodoListId = todoListId;
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
