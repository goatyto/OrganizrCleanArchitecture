﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Organizr.Domain.Lists.Entities.TodoListAggregate;

namespace Organizr.Infrastructure.Data
{
    public class OrganizrContext: DbContext
    {
        public OrganizrContext()
        {
            
        }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
