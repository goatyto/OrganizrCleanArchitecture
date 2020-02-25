using System;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.AddTodoSubList
{
    public class AddTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }

        public AddTodoSubListCommand(Guid todoListId, string title, string description)
        {
            TodoListId = todoListId;
            Title = title;
            Description = description;
        }
    }
}
