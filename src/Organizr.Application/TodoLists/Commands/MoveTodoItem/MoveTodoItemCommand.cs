using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.MoveTodoItem
{
    public class MoveTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int Ordinal { get; }
        public int? SubListId { get; }

        public MoveTodoItemCommand(Guid todoListId, int id, int ordinal, int? subListId)
        {
            TodoListId = todoListId;
            Id = id;
            Ordinal = ordinal;
            SubListId = subListId;
        }
    }
}
