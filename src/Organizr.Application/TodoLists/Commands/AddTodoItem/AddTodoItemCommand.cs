﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Organizr.Application.Common.Exceptions;
using Organizr.Domain.Lists.Entities.TodoListAggregate;
using Organizr.Domain.SharedKernel;

namespace Organizr.Application.TodoLists.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest
    {
        public Guid TodoListId { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? DueDate { get; }
        public int? SubListId { get; }

        public AddTodoItemCommand(Guid todoListId, string title, string description = null, DateTime? dueDate = null, int? subListId = null)
        {
            TodoListId = todoListId;
            Title = title;
            Description = description;
            DueDate = dueDate;
            SubListId = subListId;
        }
    }

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IDateTime _dateTimeProvider;

        public AddTodoItemCommandHandler(ITodoListRepository todoListRepository, IDateTime dateTimeProvider)
        {
            Guard.Against.Null(todoListRepository, nameof(todoListRepository));
            Guard.Against.Null(dateTimeProvider, nameof(dateTimeProvider));
            
            _todoListRepository = todoListRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);

            if(todoList == null)
                throw new NotFoundException<TodoList>(request.TodoListId);

            todoList.AddTodo(request.Title, request.Description, request.DueDate, _dateTimeProvider, request.SubListId);

            _todoListRepository.Update(todoList);

            await _todoListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
