using MediatR;

namespace Organizr.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommand: IRequest
    {
        public string OwnerId { get; }
        public string Title { get; }
        public string Description { get; }

        public CreateTodoListCommand(string ownerId, string title, string description)
        {
            OwnerId = ownerId;
            Title = title;
            Description = description;
        }
    }
}
