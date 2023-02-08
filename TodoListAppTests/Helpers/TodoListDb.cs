using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListAppTests.Helpers
{
    public class TodoListDb : WebApplicationFactory<TodoListDbContext>
    {
        
        public TodoListDbContext CreateDbContext()
        {

            var options = new DbContextOptionsBuilder<TodoListDbContext>().UseInMemoryDatabase("TodoListDb").Options;


            return new TodoListDbContext(options);
        }

    }
}
