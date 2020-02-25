using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.EditTodoList
{
    public class EditTodoListCommand: IRequest
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }

        public EditTodoListCommand(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
