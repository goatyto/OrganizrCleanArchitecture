using System;
using MediatR;

namespace Organizr.Application.TodoLists.Commands.DeleteTodoSubList
{
    public class DeleteTodoSubListCommand : IRequest
    {
        public Guid TodoListId { get; }
        public int Id { get; }

        public DeleteTodoSubListCommand(Guid todoListId, int id)
        {
            TodoListId = todoListId;
            Id = id;
        }
    }
}