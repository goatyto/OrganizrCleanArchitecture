using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.MoveTodoSubList
{
    public class MoveTodoSubListCommand: IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public int NewOrdinal { get; }

        public MoveTodoSubListCommand(Guid todoListId, int id, int newOrdinal)
        {
            TodoListId = todoListId;
            Id = id;
            NewOrdinal = newOrdinal;
        }
    }
}
