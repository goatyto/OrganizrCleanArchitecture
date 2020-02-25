using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }
        public int? SubListId { get; }
    }
}
