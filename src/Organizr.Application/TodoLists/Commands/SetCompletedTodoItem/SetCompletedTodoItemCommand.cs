using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.SetCompletedTodoItem
{
    public class SetCompletedTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }
        public bool IsCompleted { get; }

        public SetCompletedTodoItemCommand(Guid todoListId, int id, bool isCompleted)
        {
            TodoListId = todoListId;
            Id = id;
            IsCompleted = isCompleted;
        }
    }
}
